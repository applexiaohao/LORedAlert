
using UnityEngine;
using System.Collections;

public class UIEventController : MonoBehaviour {

	public GameObject barrackPre;	//	兵营预设体
	public GameObject flagPre;	//	旗帜预设体
	public GameObject factoryPre;	//	工厂预设体
	public GameObject kengPre;	//	矿坑预设体
	public GameObject woodPre;	//	伐木场预设体

	public BuildButton [] buts;

	public void DidClickUpLevelMoney () {
		if (Global.money >= Global.nextLevelMoney) {
			Global.money -= Global.nextLevelMoney;
			Global.nextLevelMoney *= 2;
			Global.currentMoneyLevel++;
		}
	}

	public void DidClickUpLevelWood () {
		if (Global.wood >= Global.nextLevelWood) {
			Global.wood -= Global.nextLevelWood;
			Global.nextLevelWood *= 2;
			Global.currentWoodLevel++;
		}
	}

	//	已经点击出售按钮
	public void DidClickRemoveButton () {
		//	出售状态取反
		Global.isRemove = !Global.isRemove;
	}

	//	已经点击了建造矿坑按钮
	public void DidClickBuildKengButton () {
		//	建造矿坑
		StartCoroutine (Build (BuildingTypes.Keng, 3f, 3));
	}

	IEnumerator Build (BuildingTypes type, float time, int index) {
		int tempT = (int)time;
		buts [index].time = tempT;
		for (float timer = 0f;timer < time;timer += Time.deltaTime) {
			tempT = (int)(time - timer) + 1;
			buts [index].time = tempT;
			yield return null;
		}
		buts [index].time = 0;
		CreateBuilding (type);
	}

	//	已经点击了建造伐木场按钮
	public void DidClickBuildWoodButton () {
		//	建造兵营
		StartCoroutine (Build (BuildingTypes.Wood, 7f, 4));
	}

	//	已经点击了建造兵营按钮
	public void DidClickBuildBarrackButton () {
		//	建造兵营
		StartCoroutine (Build (BuildingTypes.Barrack, 5f, 0));
	}

	//	已经点击了建造旗帜按钮
	public void DidClickBuildFlagButton () {
		//	建造旗帜
		StartCoroutine (Build (BuildingTypes.Flag, 2f, 1));
	}

	//	已经点击了建造工厂按钮
	public void DidClickBuildFactoryButton () {
		//	建造工厂
		StartCoroutine (Build (BuildingTypes.Factory, 6f, 2));
	}

	//	创建建筑物
	private void CreateBuilding (BuildingTypes t) {
		//	判断是否处于正常状态
		if (Global.state == GlobalStates.Normal) {
			//	创建游戏对象用的预设体
			GameObject tempPre = null;
			//	判断建筑物类型
			switch (t) {
			case BuildingTypes.Barrack:
				//	建造兵营
				tempPre = barrackPre;
				break;
			case BuildingTypes.Flag:
				//	建造旗帜
				tempPre = flagPre;
				break;
			case BuildingTypes.Factory:
				//	建造工厂
				tempPre = factoryPre;
				break;
			case BuildingTypes.Keng:
				//	建造矿坑
				tempPre = kengPre;
				break;
			case BuildingTypes.Wood:
				//	建造伐木场
				tempPre = woodPre;
				break;
			}
			//	用预设体创建建筑物
			GameObject obj = Instantiate (tempPre, new Vector3 (1000f, 1000f, 1000f), Quaternion.identity) as GameObject;
			//	设置建筑物为建造状态
			obj.GetComponent <Building> ().state = BuildingStates.Build;
			//	设置当前状态为建造状态
			Global.state = GlobalStates.Build;
		}
	}
}
