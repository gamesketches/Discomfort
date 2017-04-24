using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsokobanPlayer : MonoBehaviour {

	Rigidbody2D rigidBody;
	Animator animationController;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		rigidBody.MovePosition(rigidBody.position + (new Vector2(hori, vert) * Time.deltaTime));
		animationController.SetInteger("XMovement", (int)hori);
		animationController.SetInteger("YMovement", (int)vert);
	}
}
