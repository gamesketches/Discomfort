using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuvaniaBoss : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			other.GetComponent<MenuvaniaPlayer>().Die();
		}
		else if(other.tag == "Hitbox") {
			SceneManager.LoadScene("Menuvania");
		}
	}
}
