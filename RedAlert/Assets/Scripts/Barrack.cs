
using UnityEngine;
using System.Collections;

public class Barrack : MonoBehaviour {

	public Vector3 targetPos;
	public GameObject soldier;	//	士兵预设体
	public Transform soldierPos;	//	士兵产生位置

	private Building b;
	private BuildingController bc;
	private bool didAdd = false;	//	是否已经加入兵营列表

	private GameController gc;
	//	创建士兵
	public void CreateSoldier () {
		GameObject s = Instantiate (soldier, soldierPos.position, Quaternion.identity) as GameObject;
		s.GetComponent <NavMeshAgent> ().SetDestination (targetPos);
		gc.soldiers.Add (s.GetComponent <Move> ());
	}

	void OnMouseDown () {
		if (b.state == BuildingStates.Normal) {
			//	设置当前兵营为主兵营
			bc.SetMainBarrack (this);
		}
	}

	void Awake () {
		b = GetComponent <Building> ();
		bc = GameObject.FindGameObjectWithTag ("GameController").GetComponent <BuildingController> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	void Update () {
		//	当兵营建造过之后
		if (b.state == BuildingStates.Normal) {
			//	如果尚未加入兵营列表
			if (!didAdd) {
				//	加入到兵营列表
				bc.AddBarrack (this);
				didAdd = true;
			}
		}
	}

	void OnDestroy () {
		//	销毁兵营时如果已经加入兵营列表
		if (didAdd) {
			//	从兵营列表中移除
			bc.RemoveBarrack (this);
		}
	}
}
