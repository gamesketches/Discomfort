﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsokobanTitleScreen : MonoBehaviour {

	public float rotationAmount;
	public float oscillationTime;
	AnimationCurve rotationCurve;
	public float zoomInTime = 1;
	public Image[] UIImages;
	public Text UIText;
	Vector3 cameraTarget;
	float cameraSize;
	bool started;
	bool ended;
	// Use this for initialization
	void Start () {
		started = false;
		ended = false;
		cameraTarget = Camera.main.transform.position;
		cameraSize = Camera.main.orthographicSize;
		Camera.main.transform.position = new Vector3(0, 0, -10);
		Camera.main.orthographicSize = 28;
		rotationCurve = new AnimationCurve();
		rotationCurve.AddKey(0, 0);
		rotationCurve.AddKey(oscillationTime, rotationAmount);
		rotationCurve.AddKey(oscillationTime * 2, -rotationAmount);
		rotationCurve.AddKey(oscillationTime * 3, 0);
		rotationCurve.postWrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		if(!started) {
			transform.rotation = Quaternion.Euler(0, 0, rotationCurve.Evaluate(Time.time));
			if(Input.anyKeyDown) {
				foreach(Image img in UIImages) {
					img.CrossFadeAlpha(0.0f, 0.5f, false);
				}
				UIText.CrossFadeAlpha(0.0f, 0.5f, false);
				StartCoroutine(Onboarding());
				started = true;
			}
		}
		else if(!ended){
			if(GameObject.FindGameObjectsWithTag("Poop").Length == 0) {
				StartCoroutine(Offboarding());
				foreach(Image img in UIImages) {
					img.CrossFadeAlpha(1.0f, 0.5f, false);
				}
				UIText.CrossFadeAlpha(1.0f, 0.5f, false);
				UIText.text = "You win!";
				ended = true;
			}
		}
	}

	IEnumerator Onboarding() {
		for(float t = 0; t <= zoomInTime; t += Time.deltaTime) {
			Camera.main.transform.position = Vector3.Lerp(new Vector3(0, 0, -10), cameraTarget, t / zoomInTime);
			Camera.main.orthographicSize = Mathf.SmoothStep(28, cameraSize, t / zoomInTime);
			yield return null;
		}
	}

	IEnumerator Offboarding() {
		for(float t = 0; t <= zoomInTime; t += Time.deltaTime) {
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 0, -10), t / zoomInTime);
			Camera.main.orthographicSize = Mathf.SmoothStep(cameraSize, 28, t / zoomInTime);
			yield return null;
		}
		yield return new WaitForSeconds(4);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Asokoban");
	}
}
