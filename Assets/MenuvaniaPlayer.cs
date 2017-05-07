using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class MenuvaniaPlayer : MonoBehaviour {

	Rigidbody2D rigidBody;
	public float speed;
	private int direction;
	private float movementSpeed;
	// Use this for initialization
	void Start () {
		direction = 1;
		rigidBody = GetComponent<Rigidbody2D>();
		movementSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = transform.position;
		rigidBody.MovePosition(new Vector2(playerPos.x + movementSpeed * speed * Time.deltaTime * direction, playerPos.y));
	}

	public void PerformAction(string action) {
		Invoke(action, 0);
	}

	void Forward() {
		movementSpeed = 1;
	}

	void Back() {
		movementSpeed = -1;
	}

	void Stop() {
		movementSpeed = 0;
	}
}
