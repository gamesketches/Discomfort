using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlideIn : MonoBehaviour {

	public Vector3 targetPosition;
	public float lerpTime;
	public bool canvasElement;
	// Use this for initialization
	void Start () {
		TouchTypistManager.OnBoardEvent += StartSlideIn;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartSlideIn() {
		if(canvasElement) {
			StartCoroutine(SlideIn());
		}
		else {
			StartCoroutine(SpriteSlideIn());
		}
	}

	IEnumerator SlideIn() {
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		Vector3 startPosition = rectTransform.transform.position;
		targetPosition.z = startPosition.z;
		for(float t = 0; t < lerpTime; t += Time.deltaTime) {
			rectTransform.transform.position = Vector3.Lerp(startPosition, targetPosition, t / lerpTime);
			yield return null;
		}
		rectTransform.transform.position = targetPosition;
	}

	IEnumerator SpriteSlideIn() {
		Vector3 startPosition = transform.position;
		targetPosition.z = startPosition.z;

		for(float t = 0; t < lerpTime; t += Time.deltaTime) {
			transform.position = Vector3.Lerp(startPosition, targetPosition, t / lerpTime);
			yield return null;
		}
		transform.position = targetPosition;
	}
}
