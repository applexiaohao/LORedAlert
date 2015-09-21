
using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public bool selected = false;
	private NavMeshAgent nma;
	private GameController gc;

	void Awake () {
		nma = GetComponent <NavMeshAgent> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent <GameController> ();
	}

	public void MoveTo (Vector3 pos) {
		nma.SetDestination (pos);
	}

	void Update () {
		if (selected) {
			GetComponent <Renderer> ().material.color = Color.red;
		} else {
			GetComponent <Renderer> ().material.color = Color.white;
		}
	}

	void OnMouseUpAsButton () {
		if (gc.selectedSoldiers.Count != 0) {
			foreach (Move m in gc.selectedSoldiers) {
				m.selected = false;
			}
			gc.selectedSoldiers.Clear ();
		}
		selected = true;
		gc.selectedSoldiers.Add (this);
	}
}
