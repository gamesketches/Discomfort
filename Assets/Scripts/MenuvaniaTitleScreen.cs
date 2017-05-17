using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MenuvaniaTitleScreen : MonoBehaviour {

	public Vector3 playerViewPos;
	bool started;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.ActiveDevice.AnyButton.WasPressed && !started) {
			StartCoroutine(MoveDown());
			started = true;
		}	
	}

	IEnumerator MoveDown() {
		Vector3 startPos = transform.localPosition;
		for(float t = 0; t <= 1; t += Time.deltaTime) {
			transform.localPosition = Vector3.Lerp(startPos, playerViewPos, t);
			yield return null;
		}
	}
}
