
using UnityEngine;
using System.Collections;

//	建筑物类型
public enum BuildingTypes {
	Barrack = 0,	//	兵营
	Flag,	//	旗帜
	Factory,	//	工厂
	Keng,	//	矿坑
	Wood,	//	伐木场
}

//	建筑物的状态
public enum BuildingStates {
	Normal = 0,	//	正常状态
	Build,	//	建造状态
}

public class Building : MonoBehaviour {

	public int w;	//	建筑物的宽度
	public int l;	//	建筑物的长度
	public BuildingStates state;	//	当前状态
	public BuildingTypes type;	//	建筑物类型
	public Grid [,] grids;	//	格子数组
	public int price;	//	建筑物价格
	public int wood;

	private Draw draw;	//	绘制格子
	private bool didDraw = false;	//	是否已经绘制格子
	private string gridsID;	//	格子 ID
	private BuildingController bc;	//	建筑物控制器
	private bool canBuild = true;

	//	当鼠标点击当前游戏对象时触发
	void OnMouseUpAsButton () {
		//	如果当前是要出售
		if (Global.isRemove) {
			//	从建筑物列表中移除
			bc.buildings.Remove (this);
			//	重置出售状态
			Global.isRemove = false;
			//	回收金币
			Global.money += (int)(price * 0.5f);
			//	销毁建筑物
			Destroy (gameObject);
		}
	}

	void Awake () {
		//	获取建筑物控制器
		bc = GameObject.FindGameObjectWithTag ("GameController").GetComponent <BuildingController> ();
		//	获取主摄像机上的 Draw 脚本
		draw = Camera.main.GetComponent <Draw> ();
		//	初始化成员变量
		grids = new Grid [w, l];
		//	使用循环给数组元素赋值
		for (int i = 0;i < w;i++) {
			for (int j = 0;j < l;j++) {
				//	创建格子
				Grid g = new Grid (Vector3.zero);
				//	放入数组
				grids [i, j] = g;
			}
		}
	}

	void Update () {
		//	建筑物的状态机
		FSM ();
	}

	//	建筑物的状态机
	private void FSM () {
		//	判断当前状态
		switch (state) {
		case BuildingStates.Normal:
			//	正常状态下
			Normal ();
			break;
		case BuildingStates.Build:
			//	建造状态下
			Build ();
			break;
		}
	}

	//	正常状态下
	private void Normal () {

	}

	//	建造状态下
	private void Build () {
		//	获取从摄像机发出经过鼠标当前位置的射线
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//	获取碰撞信息
		RaycastHit [] hitInfos = Physics.RaycastAll (ray);
		//	遍历所有碰撞信息
		foreach (RaycastHit hitInfo in hitInfos) {
			//	如果跟地面碰撞
			if (hitInfo.transform.tag == "Ground") {
				//	重新设置当前建筑物位置
				transform.position = hitInfo.point;
			}
		}
		//	根据建筑物当前位置计算出相对的格子位置
		Vector3 fixedPos = Global.GetLocalPos (transform.position);
		//	获取当前建筑物的参考格子下标
		int tempI = w / 2;
		int tempJ = l / 2;
		//	重置能否建造的状态
		canBuild = true;
		//	依次更新每个格子的相对位置
		for (int i = 0;i < w;i++) {
			for (int j = 0;j < l;j++) {
				//	获取格子
				Grid g = grids [i, j];
				//	更新格子相对位置
				g.pos = new Vector3 (fixedPos.x + i - tempI, 0f, fixedPos.z + j - tempJ);
				//	更新格子是否有效
				g.enable = bc.InspectGrid (g.pos);
				//	更新建筑物能否建造的状态
				if (!g.enable) {
					canBuild = false;
				}
			}
		}
		//	获取下标为 [0, 0] 的格子中心点
		Vector3 tempPos = Global.GetGlobalPos (grids [0, 0].pos);
		//	获取所有格子的中心位置
		tempPos.x = tempPos.x + w / 2f - 0.5f;
		tempPos.z = tempPos.z + l / 2f - 0.5f;
		//	更新建筑物的位置
		transform.position = tempPos;
		//	如果当前尚未绘制格子
		if (!didDraw) {
			//	绘制格子
			gridsID = draw.AddGrids (grids);
			//	更新绘制状态
			didDraw = true;
		}
		//	按下鼠标左键建造
		if (Input.GetMouseButtonDown (0) && canBuild) {
			if (Global.money >= price && Global.wood >= wood) {
				//	扣除金钱
				Global.money -= price;
				Global.wood -= wood;
				//	设置游戏状态为正常状态
				Global.state = GlobalStates.Normal;
				//	停止绘制格子
				draw.RemoveGrids (gridsID);
				didDraw = false;
				//	设置建筑物状态为正常状态
				state = BuildingStates.Normal;
				//	加入到建筑物控制器
				bc.buildings.Add (this);
			} else {
				GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent <GameController> ();
				if (Global.money < price) {
					gc.MoneyAlert ();
				}
				if (Global.wood < wood) {
					gc.WoodAlert ();
				}
			}
		}
		//	按下鼠标右键取消建造
		if (Input.GetMouseButtonDown (1)) {
			//	设置游戏状态为正常状态
			Global.state = GlobalStates.Normal;
			//	停止绘制格子
			draw.RemoveGrids (gridsID);
			didDraw = false;
			//	销毁当前建筑物
			Destroy (gameObject);
		}
	}
}
