using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MenuvaniaTitleScreen : MonoBehaviour {

	public Vector3 playerViewPos;
	bool started;
    public float lerpTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown(KeyCode.Space) || InputManager.ActiveDevice.AnyButton.WasPressed) && !started) {
			StartCoroutine(MoveDown());
			started = true;
		}	
	}

	IEnumerator MoveDown() {
		Vector3 startPos = transform.localPosition;
		for(float t = 0; t <= 1; t += Time.deltaTime) {
			transform.localPosition = Vector3.Lerp(startPos, playerViewPos, Mathf.SmoothStep(0,1,t / lerpTime)); 
			yield return null;
		}
	}
}
