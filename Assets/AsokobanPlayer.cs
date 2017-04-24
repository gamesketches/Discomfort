using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsokobanPlayer : MonoBehaviour {

	Rigidbody2D rigidBody;
	Animator animationController;
	SpriteRenderer renderer;
	BoxCollider2D collider;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		rigidBody.MovePosition(rigidBody.position + (new Vector2(hori, vert) * Time.deltaTime));
		animationController.SetInteger("XMovement", (int)hori);
		animationController.SetInteger("YMovement", (int)vert);
		if(hori > 0) {
			renderer.flipX = true;
			Vector2 baseOffset = collider.offset;
			baseOffset.x = Mathf.Abs(baseOffset.x);
			collider.offset = baseOffset;
		}
		else if(hori < 0) {
			renderer.flipX = false;
			Vector2 baseOffset = collider.offset;
			baseOffset.x = Mathf.Abs(baseOffset.x) * -1;
			collider.offset = baseOffset;
		}
	}
}
