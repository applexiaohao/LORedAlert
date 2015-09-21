
using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour {

	private Building b;
	private bool didAdd = false;

	void Start () {
		b = GetComponent <Building> ();
	}
	
	void Update () {
		if (b.state == BuildingStates.Normal) {
			if (!didAdd) {
				Global.factoryCount++;
				didAdd = true;
			}
		}
	}

	void OnDestroy () {
		if (didAdd) {
			Global.factoryCount--;
		}
	}
}
