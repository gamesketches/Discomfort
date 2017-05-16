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
	Vector3 checkpoint;
	GameObject hitbox;
	bool grounded;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		grounded = true;
		checkpoint = transform.position;
		animator = GetComponent<Animator>();
		direction = 1;
		rigidBody = GetComponent<Rigidbody2D>();
		movementSpeed = 0;
		hitbox = transform.GetChild(0).gameObject;
		hitbox.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = transform.position;
		//rigidBody.MovePosition(new Vector2(playerPos.x + (movementSpeed * speed * Time.deltaTime * direction), playerPos.y + Physics.gravity.y));
		transform.Translate(movementSpeed * speed * Time.deltaTime * direction, 0, 0);
		if(grounded && movementSpeed != 0) {
			if(!audioSource.isPlaying) 
				audioSource.Play();
		}
		else {
			audioSource.Stop();
		}
	}

	public void SetCheckpoint(Vector3 position) {
		checkpoint = position;
	}

	public void Die() {
		movementSpeed = 0;
		rigidBody.velocity = Vector2.zero;
		animator.SetTrigger("Idle");
		StartCoroutine(ReturnToCheckpoint());
	}

	IEnumerator ReturnToCheckpoint() {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.enabled = false;
		Vector3 startPos = transform.position;
		for(float t = 0; t <= 1; t += Time.deltaTime) {
			transform.position = Vector3.Slerp(startPos, checkpoint, t);
			yield return null;
		}
		renderer.enabled = true;
		transform.position = checkpoint;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Ground") {
			grounded = true;
			if(movementSpeed != 0) {
				animator.SetTrigger("Walk");
			}
		}
		else if(col.gameObject.tag == "Poop") {
			Die();
		}
	}

	#region Verbs

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
		hitbox.SetActive(true);
		Invoke("DisableHitbox", animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}

	void DisableHitbox() {
		hitbox.SetActive(false);
	}

	void Jump() {
		if(grounded) {
			grounded = false;
			animator.SetTrigger("Jump");
			rigidBody.AddForce(new Vector2(0, 50), ForceMode2D.Impulse);
		}
	}

	void Hop() {
		if(grounded) {
			grounded = false;
			animator.SetTrigger("Jump");
			rigidBody.AddForce(new Vector2(20, 28), ForceMode2D.Impulse);
		}
	}

	void BigJump() {
		if(grounded) {
			grounded = false;
			animator.SetTrigger("Jump");
			rigidBody.AddForce(new Vector2(0, 100), ForceMode2D.Impulse);
		}
	}

	void Block() {
		movementSpeed = 0;
		animator.SetTrigger("Block");
		gameObject.tag = "Finish";
		Invoke("Unblock", animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}

	void Unblock() {
		gameObject.tag = "Player";
	}

	#endregion
}
