using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float lookSpeed = 3;
	private Vector2 rotation = Vector2.zero;
	public Transform target;
	private Vector3 velocity = Vector3.zero;
	public float smoothTime = 0.3f;

	void Start(){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update(){
		transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
	}

	void LateUpdate(){
		rotation.y += Input.GetAxis("Mouse X");
		rotation.x += -Input.GetAxis("Mouse Y");
		transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, rotation.y * lookSpeed, 0);
	}

}
