
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text moneyText;	//	显示金币的文本框
	public Text woodText;

	public ArrayList soldiers;
	public ArrayList selectedSoldiers;

	void Awake () {
		soldiers = new ArrayList ();
		selectedSoldiers = new ArrayList ();
	}

	public void MoneyAlert () {
		StartCoroutine (Alert ());
	}

	public void WoodAlert () {
		StartCoroutine (Alert (false));
	}

	IEnumerator Alert (bool isMoney = true) {
		Text t = null;
		if (isMoney) {
			t = moneyText;
		} else {
			t = woodText;
		}
		Color start = t.color;
		Color begin = start;
		Color target = Color.red;
		for (float timer = 0f;timer <= 1f;timer += Time.deltaTime) {
			Color c = Color.Lerp (begin, target, (timer % (1f / 4f)) * 4f);
			t.color = c;
			if (c == target) {
				Color temp = target;
				target = begin;
				begin = temp;
			}
			yield return null;
		}
		t.color = start;
	}

	void Update () {
		//	刷新金币显示
		moneyText.text = Global.money.ToString ();
		woodText.text = Global.wood.ToString ();
		//	如果点了鼠标右键
		if (Input.GetMouseButtonDown (1)) {
			//	如果处于拆迁状态
			if (Global.isRemove) {
				//	取消出售
				Global.isRemove = false;
			}

			if (selectedSoldiers.Count != 0) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				//	获取所有碰撞信息
				RaycastHit [] hitInfos = Physics.RaycastAll (ray);
				//	遍历数组
				for (int i = 0;i < hitInfos.Length;i++) {
					//	取出碰撞信息
					RaycastHit hitInfo = hitInfos [i];
					//	判断是否是地面碰撞
					if (hitInfo.transform.CompareTag ("Ground")) {
						//	获取碰撞点并移动
						MoveSelectedSoldiers (hitInfo.point);
					}
				}
			}
		}
	}

	//移动所有被选中士兵
	void MoveSelectedSoldiers (Vector3 targetPos) {
		foreach (Move m in selectedSoldiers) {
			m.MoveTo(targetPos);
		}
	}
}
