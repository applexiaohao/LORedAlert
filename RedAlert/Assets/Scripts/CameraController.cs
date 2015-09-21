
using UnityEngine;
using System.Collections;

//	摄像机移动方向
public enum MoveDirection {
	None = 0,		//	空 0
	Up = 1 << 0,	//	上 1
	Down = 1 << 1,	//	下 2
	Left = 1 << 2,	//	左 4
	Right = 1 << 3,	//	右 8
}

public class CameraController : MonoBehaviour {

	public GameObject target;	//	当前目标

	public float maxDistance;	//	摄像机最大距离
	public float minDistance;	//	摄像机最小距离
	public float moveSpeed;	//	摄像机移动速度
	public float rotateSpeed;	//	摄像机旋转速度

	private int w = 0;	//	屏幕的宽
	private int h = 0;	//	屏幕的高
	private bool didClickRightButton = false;	//	鼠标右键是否按下
	private MoveDirection moveDirection = MoveDirection.None;	//	移动方向
	private Vector3 direction = Vector3.zero;	//	摄像机移动方向向量
	private Vector3 lastMousePos = Vector3.zero;	//	上一帧鼠标的位置

	void Awake () {
		//	获取屏幕的宽高
		w = Screen.width;
		h = Screen.height;
	}

	void Update () {
		LockTarget ();
		RotateCamera ();
		ScaleCamera ();
		//	当旋转摄像机时不能够移动摄像机
		if (!didClickRightButton) {
			UpdateDirection ();
			MoveCamera ();
		}
	}

	//	锁定目标
	private void LockTarget () {
		//	如果按下了空格键并且当前有目标
		if (Input.GetKeyDown (KeyCode.Space) && target != null) {
			//	找到摄像机 Z 轴正方向与地面交点
			//	创建一个射线
			//	起始点为摄像机当前位置，发射方向为摄像机 Z 轴正方向
			Ray ray = new Ray (transform.position, transform.forward);
			//	获取所有碰撞信息
			RaycastHit [] hitInfos = Physics.RaycastAll (ray);
			//	遍历数组
			for (int i = 0;i < hitInfos.Length;i++) {
				//	取出碰撞信息
				RaycastHit hitInfo = hitInfos [i];
				//	判断是否是地面碰撞
				if (hitInfo.transform.CompareTag ("Ground")) {
					//	获取碰撞点
					Vector3 currentCenter = hitInfo.point;
					//	获取摄像机的相对位置
					Vector3 tempPos = transform.position - currentCenter;
					//	计算目标点的位置
					Vector3 targetPos = target.transform.position;
					targetPos.y = 0f;
					//	计算出摄像机新的位置
					transform.position = tempPos + targetPos;
					break;
				}
			}
		}
	}

	//	旋转摄像机
	private void RotateCamera () {
		//	更新鼠标右键状态
		if (Input.GetMouseButtonDown (1)) {
			//	记录鼠标当前位置
			lastMousePos = Input.mousePosition;
			didClickRightButton = true;
		}
		if (Input.GetMouseButtonUp (1)) {
			didClickRightButton = false;
		}
		//	如果按下了鼠标右键
		if (didClickRightButton) {
			//	获取鼠标当前位置
			Vector3 currentMousePos = Input.mousePosition;
			//	计算出鼠标的偏移量
			Vector3 mouseOffset = currentMousePos - lastMousePos;
			//	使用鼠标的偏移量旋转摄像机
			//	创建一个射线
			//	起始点为摄像机当前位置，发射方向为摄像机 Z 轴正方向
			Ray ray = new Ray (transform.position, transform.forward);
			//	获取所有碰撞信息
			RaycastHit [] hitInfos = Physics.RaycastAll (ray);
			//	遍历数组
			for (int i = 0;i < hitInfos.Length;i++) {
				//	取出碰撞信息
				RaycastHit hitInfo = hitInfos [i];
				//	判断是否是地面碰撞
				if (hitInfo.transform.CompareTag ("Ground")) {
					//	在旋转摄像机之前保存摄像机状态
					Vector3 tempPos = transform.position;
					Quaternion tempRot = transform.rotation;
					//	旋转摄像机
					transform.RotateAround (hitInfo.point, Vector3.up, mouseOffset.x * Time.deltaTime * rotateSpeed);
					transform.RotateAround (hitInfo.point, transform.right, -mouseOffset.y * Time.deltaTime * rotateSpeed);
					//	检测摄像机位置是否在范围内
					if (transform.position.y > maxDistance || transform.position.y < minDistance) {
						//	恢复到旋转之前的状态
						transform.position = tempPos;
						transform.rotation = tempRot;
					}
					break;
				}
			}
			//	更新 lastMousePos
			lastMousePos = currentMousePos;
		}
	}

	//	缩放摄像机
	private void ScaleCamera () {
		//	获取鼠标滚轮滚动
		float mouseScr = Input.GetAxis ("Mouse ScrollWheel");
		//	如果滚动了滚轮
		if (mouseScr != 0f) {
			//	获取滚动方向
			mouseScr = mouseScr > 0 ? 1f : -1f;
			//	保存移动前摄像机的位置
			Vector3 tempPos = transform.position;
			//	移动摄像机
			transform.position += transform.forward * Time.deltaTime * moveSpeed * mouseScr;
			//	判断摄像机是否超出高度范围
			if (transform.position.y < minDistance || transform.position.y > maxDistance) {
				//	摄像机回到移动前的位置
				transform.position = tempPos;
			}
		}
	}

	//	更新移动方向
	private void UpdateDirection () {
		//	将当前移动方向设为空
		moveDirection = MoveDirection.None;
		//	获取鼠标当前位置
		Vector3 mousePos = Input.mousePosition;
		//	检测摄像机上下移动
		if (Input.GetKey (KeyCode.UpArrow) || mousePos.y >= h) {
			//	叠加向上移动的状态
			moveDirection |= MoveDirection.Up;
		} else if (Input.GetKey (KeyCode.DownArrow) || mousePos.y <= 0) {
			//	叠加向下移动的状态
			moveDirection |= MoveDirection.Down;
		}
		//	检测摄像机左右移动
		if (Input.GetKey (KeyCode.LeftArrow) || mousePos.x <= 0) {
			//	叠加向左移动的状态
			moveDirection |= MoveDirection.Left;
		} else if (Input.GetKey (KeyCode.RightArrow) || mousePos.x >= w) {
			//	叠加向右移动的状态
			moveDirection |= MoveDirection.Right;
		}
	}

	//	移动摄像机
	private void MoveCamera () {
		//	判断摄像机是否应当移动
		if (moveDirection != MoveDirection.None) {
			//	重置方向向量
			direction = Vector3.zero;
			//	判断是否包含上下移动的方向
			if ((moveDirection & MoveDirection.Up) != 0) {
				//	获取摄像机正前方
				Vector3 tempForward = transform.forward;
				//	将正前方向量映射到 XZ 平面上
				tempForward.y = 0f;
				//	获取单位向量
				tempForward.Normalize ();
				//	将方向向量与向前移动的向量叠加
				direction += tempForward;
			} else if ((moveDirection & MoveDirection.Down) != 0) {
				//	获取摄像机正前方
				Vector3 tempForward = transform.forward;
				//	将正前方向量映射到 XZ 平面上
				tempForward.y = 0f;
				//	获取单位向量
				tempForward.Normalize ();
				//	将方向向量与向后移动的向量叠加
				direction += -tempForward;
			}
			//	判断是否包含左右移动的方向
			if ((moveDirection & MoveDirection.Left) != 0) {
				//	将方向向量与向上移动的向量叠加
				direction += -transform.right;
			} else if ((moveDirection & MoveDirection.Right) != 0) {
				//	将方向向量与向下移动的向量叠加
				direction += transform.right;
			}
			//	单位化方向向量
			direction.Normalize ();
			//	移动摄像机
			transform.position += direction * Time.deltaTime * moveSpeed;
		}
	}
}
