using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderBehavior : MonoBehaviour {

	public Transform[] options;
	static float fanTime = 0.3f;
	private float totalTravelDistance;
	// Use this for initialization
	void Start () {
		totalTravelDistance = (options[0].position.y - options[options.Length - 1].position.y) / options.Length;
		foreach(Transform option in options) {
			option.position = options[0].position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FanOut() {
		StartCoroutine(FanOutCoroutine());
	}

	public void FanIn() {
		StartCoroutine(FanInCoroutine());
	}

	IEnumerator FanOutCoroutine() {
		for(float t = 0; t < fanTime; t += Time.deltaTime) {
			for(int i = 0; i < options.Length; i++) {
				options[i].position = Vector3.Lerp(options[i].position, options[0].position - new Vector3(0, totalTravelDistance * i, 0), t / fanTime);
			}
			yield return null;
		}
	}

	IEnumerator FanInCoroutine() {
		for(float t = 0; t < fanTime; t += Time.deltaTime) {
			for(int i = 0; i < options.Length; i++) {
				options[i].position = Vector3.Lerp(options[i].position, options[0].position, t / fanTime);
			}
			yield return null;
		}
	}
}
