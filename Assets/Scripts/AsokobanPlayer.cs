﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsokobanPlayer : MonoBehaviour {

	Rigidbody2D rigidBody;
	Animator animationController;
	Animator shadowController;
	SpriteRenderer renderer;
	BoxCollider2D collider;
	public float speed;
	AudioSource audioSource;
	AudioSource footSteps;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<Animator>();
		shadowController = transform.GetChild(0).gameObject.GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
		audioSource = GetComponent<AudioSource>();
		footSteps = GetComponents<AudioSource>()[1];
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		if((hori != 0 || vert != 0) && !footSteps.isPlaying) {
			footSteps.Play();
		}
		else if(hori == 0 && vert == 0) {
			footSteps.Stop();
		}
		if(Input.GetKeyDown(KeyCode.R)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

		rigidBody.MovePosition(rigidBody.position + (new Vector2(hori, vert) * speed * Time.deltaTime));
		animationController.SetInteger("XMovement", (int)Input.GetAxisRaw("Horizontal"));
		animationController.SetInteger("YMovement", (int)Input.GetAxisRaw("Vertical"));
		shadowController.SetInteger("XMovement", (int)Input.GetAxisRaw("Horizontal"));
		shadowController.SetInteger("YMovement", (int)Input.GetAxisRaw("Vertical"));
		if(hori > 0) {
			renderer.flipX = true;
			Vector2 baseOffset = collider.offset;
			transform.GetChild(0).localPosition = new Vector3(0.38f, -0.9f, 0);
			baseOffset.x = Mathf.Abs(baseOffset.x);
			collider.offset = baseOffset;
		}
		else if(hori < 0) {
			renderer.flipX = false;
			transform.GetChild(0).localPosition = new Vector3(0.85f, -0.9f, 0);
			Vector2 baseOffset = collider.offset;
			baseOffset.x = Mathf.Abs(baseOffset.x) * -1;
			collider.offset = baseOffset;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Poop" && !audioSource.isPlaying) {
			audioSource.Play();
			if(col.transform.position.y < transform.position.y) {
				renderer.sortingOrder = 2;
			}
			else {
				renderer.sortingOrder = 4;
			}
		}
	}

	void OnCollisionStay2D(Collision2D col) {
		if(col.gameObject.tag == "Poop" && !audioSource.isPlaying) {
			audioSource.Play();
			if(col.transform.position.y < transform.position.y) {
				renderer.sortingOrder = 2;
			}
			else {
				renderer.sortingOrder = 4;
			}
		}
	}

	void OnCollisionExit2D(Collision2D col) {
			renderer.sortingOrder = 4;
	}
}
