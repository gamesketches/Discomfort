﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {


	GameObject lid;	
	// Use this for initialization
	void Start () {
		lid = transform.GetChild(0).gameObject;
		lid.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Poop") {
			ReceivePoop(other.gameObject);
		}
	}

	void ReceivePoop(GameObject obj) {
		Destroy(obj);
		lid.SetActive(true);
	}
}