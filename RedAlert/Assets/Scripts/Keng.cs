
using UnityEngine;
using System.Collections;

public class Keng : MonoBehaviour {
	
	private float timer = 0f;	//	计时器
	private Building b;

	void Awake () {
		b = GetComponent <Building> ();
	}
	
	void Update () {
		if (b.state == BuildingStates.Normal) {
			//	每 1 秒加 1 钱
			if (timer >= 2f) {
				Global.money += Global.currentMoneyLevel;
				timer = 0f;
			} else {
				timer += Time.deltaTime;
			}
		}
	}
}
