using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour {

	Rigidbody2D player;
	public bool sucking;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(sucking) {
			Vector2 suckForce = new Vector2(transform.position.x - player.position.x, transform.position.y - player.transform.position.y);
			player.AddForce(suckForce);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Asokoban");
		}
	}
}
