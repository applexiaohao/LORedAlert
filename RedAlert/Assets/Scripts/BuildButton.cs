
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {

	public string title;
	public int money;
	public int wood;
	public int time = 0;
	public bool isLevelUpMoney = false;
	public bool isLevelUpWood = false;


	private Text t;

	void Awake () {
		t = GetComponentInChildren <Text> ();
	}
	
	void Update () {
		if (isLevelUpMoney) {
			money = Global.nextLevelMoney;
		}
		if (isLevelUpWood) {
			wood = Global.nextLevelWood;
		}
		string str = title + "<b>(<color=";
		if (Global.money < money) {
			str += "#ff0000ff";
		} else {
			str += "#000000ff";
		}
		str += ">" + money.ToString () + "</color>,<color=";
		if (Global.wood < wood) {
			str += "#ff0000ff";
		} else {
			str += "#000000ff";
		}
		str += ">" + wood.ToString () + "</color>)</b>";
		if (time != 0) {
			str += "<color=#008000ff><b>(" + time + ")</b></color>";
		}
		t.text = str;
	}
}
