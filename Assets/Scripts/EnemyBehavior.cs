using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	Vector3 pos1;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		pos1 = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Slerp(pos1, pos1 + offset, Mathf.Abs(Mathf.Sin(Time.time)));
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			other.GetComponent<MenuvaniaPlayer>().Die();
		}
		else if(other.tag == "Hitbox") {
			Destroy(gameObject);
		}
	}
}
