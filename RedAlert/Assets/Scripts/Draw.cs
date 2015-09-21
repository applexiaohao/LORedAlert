
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Draw : MonoBehaviour {

	public Material redMat;	//	红色的材质
	public Material greenMat;	//	绿色的材质

	private bool shouldDrawRect = false;
	private Vector3 startPos;
	private GameController gc;

	private Dictionary <string, Grid [,]> gridDic;	//	格子字典

	//	添加需要绘制的格子
	public string AddGrids (Grid [,] grids) {
		//	生成一个标识符
		string key = Time.time.ToString ();
		//	向字典中添加数据
		gridDic.Add (key, grids);
		//	返回标识符
		return key;
	}

	//	移除不需要继续绘制的格子
	public void RemoveGrids (string key) {
		//	从字典中移除数据
		gridDic.Remove (key);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			startPos = Input.mousePosition;
			shouldDrawRect = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			shouldDrawRect = false;
		}
	}

	void Awake () {
		//	初始化成员变量
		gridDic = new Dictionary <string, Grid [,]> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent <GameController> ();
	}

	void OnPostRender () {
		//	遍历字典中每一对 Key - Value
		foreach (KeyValuePair <string, Grid [,]> keyValue in gridDic) {
			//	遍历二维数组中每一个格子
			foreach (Grid g in keyValue.Value) {
				//	绘制格子
				DrawGrid (g);
			}
		}
		if (shouldDrawRect) {
			Vector3 endPos = Input.mousePosition;
			GL.PushMatrix ();
			greenMat.SetPass (0);
			GL.LoadPixelMatrix ();
			GL.Begin (GL.QUADS);
			GL.Vertex3 (startPos.x, startPos.y, 0f);
			GL.Vertex3 (startPos.x, endPos.y, 0f);
			GL.Vertex3 (endPos.x, endPos.y, 0f);
			GL.Vertex3 (endPos.x, startPos.y, 0f);
			GL.End ();
			GL.PopMatrix ();
			foreach (Move m in gc.selectedSoldiers) {
				m.selected = false;
			}
			gc.selectedSoldiers.Clear ();
			foreach (Move m in gc.soldiers) {
				Vector3 pos = m.transform.position;
				Vector3 tempPos = Camera.main.WorldToScreenPoint (pos);
				Vector3 p1 = startPos;
				Vector3 p2 = endPos;
				if (startPos.x < endPos.x) {
					p1.x = endPos.x;
					p2.x = startPos.x;
				}
				if (startPos.y < endPos.y) {
					p1.y = endPos.y;
					p2.y = startPos.y;
				}
				if (tempPos.x > p2.x && tempPos.x < p1.x && tempPos.y > p2.y && tempPos.y < p1.y) {
					gc.selectedSoldiers.Add (m);
					m.selected = true;
				}
			}
		}
	}

	//	绘制格子
	private void DrawGrid (Grid g) {
		//	保存上下文
		GL.PushMatrix ();
		//	设置着色器块
		if (g.enable) {
			//	格子是有效的用绿色绘制
			greenMat.SetPass (0);
		} else {
			//	格子是无效的用红色绘制
			redMat.SetPass (0);
		}
		//	开始绘制
		GL.Begin (GL.QUADS);
		//	计算出格子的中心点坐标
		Vector3 center = Global.GetGlobalPos (g.pos);
		//	设置顶点
		GL.Vertex3 (center.x - Global.gridSize / 2f, 0.01f, center.z + Global.gridSize / 2f);
		GL.Vertex3 (center.x + Global.gridSize / 2f, 0.01f, center.z + Global.gridSize / 2f);
		GL.Vertex3 (center.x + Global.gridSize / 2f, 0.01f, center.z - Global.gridSize / 2f);
		GL.Vertex3 (center.x - Global.gridSize / 2f, 0.01f, center.z - Global.gridSize / 2f);
		//	结束绘制
		GL.End ();
		//	恢复上下文
		GL.PopMatrix ();
	}
}
