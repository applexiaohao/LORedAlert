
using UnityEngine;
using System.Collections;

//	整个游戏的状态
public enum GlobalStates {
	Normal = 0,	//	正常状态
	Build,	//	建造状态
}

public class Global : MonoBehaviour {

	public static int currentMoneyLevel = 1;
	public static int nextLevelMoney = 10;
	public static int currentWoodLevel = 1;
	public static int nextLevelWood = 10;
	public static int factoryCount = 0;
	public static int money = 500;	//	当前金币数
	public static int wood = 100;	//	当前木材数
	public static bool isRemove = false;	//	是否正在拆迁
	public static int mapWidth = 100;	//	地图宽度
	public static int mapLength = 100;	//	地图长度
	public static float gridSize = 0.9f;	//	格子大小
	public static GlobalStates state = GlobalStates.Normal;	//	当前状态

	//	通过世界坐标点获取相对的格子位置
	public static Vector3 GetLocalPos (Vector3 globalPos) {
		return new Vector3 ((int)(globalPos.x + mapWidth / 2), 0f, (int)(globalPos.z + mapLength / 2));
	}

	//	通过相对位置获取世界坐标
	public static Vector3 GetGlobalPos (Vector3 localPos) {
		return new Vector3 (localPos.x - mapWidth / 2 + 0.5f, 0f, localPos.z - mapLength / 2 + 0.5f);
	}
}








