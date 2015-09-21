
using UnityEngine;
using System.Collections;

public class Grid {

	public Vector3 pos;	//	格子的相对位置
	public bool enable;	//	格子是否有效

	//	构造方法
	public Grid (Vector3 pos, bool enable = true) {
		Init (pos, enable);
	}

	//	初始化方法
	private void Init (Vector3 pos, bool enable) {
		//	初始化成员变量
		this.pos = pos;
		this.enable = enable;
	} 
}
