
using UnityEngine;
using System.Collections;

public class Wood : MonoBehaviour {
	
	private Building b;
	private float timer = 0f;

	void Awake () {
		b = GetComponent <Building> ();
	}
	
	void Update () {
		if (b.state == BuildingStates.Normal) {
			if (timer >= 2f) {
				Global.wood += Global.currentWoodLevel;
				timer = 0f;
			} else {
				timer += Time.deltaTime;
			}
		}
	}
}
