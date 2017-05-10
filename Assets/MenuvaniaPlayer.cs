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
	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		direction = 1;
		rigidBody = GetComponent<Rigidbody2D>();
		movementSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = transform.position;
		rigidBody.MovePosition(new Vector2(playerPos.x + (movementSpeed * speed * Time.deltaTime * direction), playerPos.y));
	}

	public void PerformAction(string action) {
		Invoke(action, 0);
	}

	void Forward() {
		if(movementSpeed == 0) {
			animator.SetTrigger("Walk");
		}
		movementSpeed = 1;
	}

	void Turn() {
		direction *= -1;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.flipX = !renderer.flipX;
	}

	void Back() {
		Debug.Log("walk it back");
		if(movementSpeed == 0) {
			animator.SetTrigger("Walk");
		}
		movementSpeed = -1;
	}

	void Stop() {
		if(movementSpeed != 0) {
			animator.SetTrigger("Idle");
		}
		movementSpeed = 0;
	}

	void Attack() {
		movementSpeed = 0;
		animator.SetTrigger("Attack");
	}

	void Jump() {
		animator.SetTrigger("Jump");
		rigidBody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
	}

	void Block() {
		movementSpeed = 0;
		animator.SetTrigger("Block");
	}
}
