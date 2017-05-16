using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsokobanTitleScreen : MonoBehaviour {

	public float rotationAmount;
	public float oscillationTime;
	AnimationCurve rotationCurve;
	public Image[] UIImages;
	public Text UIText;
	// Use this for initialization
	void Start () {
		rotationCurve = new AnimationCurve();
		rotationCurve.AddKey(0, 0);
		rotationCurve.AddKey(oscillationTime, rotationAmount);
		rotationCurve.AddKey(oscillationTime * 2, -rotationAmount);
		rotationCurve.AddKey(oscillationTime * 3, 0);
		rotationCurve.postWrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(0, 0, rotationCurve.Evaluate(Time.time));
		if(Input.anyKeyDown) {
			foreach(Image img in UIImages) {
				img.CrossFadeAlpha(0.0f, 0.5f, false);
			}
			UIText.CrossFadeAlpha(0.0f, 0.5f, false);
		}
	}

}
