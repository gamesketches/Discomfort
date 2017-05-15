using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerBehaviour : MonoBehaviour {

	private Vector3 startPosition;
	public static float movement = -100;
	public static float movementTime = 0.3f;
	public Transform[] options;
	bool toggled;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		audioSource = GetComponent<AudioSource>();
		LoadNextAudioClip();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleDrawer() {
		audioSource.Play();
		if(!toggled) {
			StartCoroutine(ChangePosition(startPosition + new Vector3(0, movement, 0)));	
		}
		else {
			StartCoroutine(ChangePosition(startPosition));
		}
		toggled = !toggled;
	}

	IEnumerator ChangePosition(Vector3 destination) {
		Vector3 currentPosition = transform.localPosition;
		Debug.Log(currentPosition);
		Debug.Log(destination);
		for(float t = 0; t <= movementTime; t += Time.deltaTime) {
			transform.localPosition = Vector3.Slerp(currentPosition, destination, t / movementTime);
			yield return null;
		}
		transform.localPosition = destination;
		LoadNextAudioClip();
	}

	void LoadNextAudioClip() {
		if(transform.localPosition == startPosition) {
			audioSource.clip = Resources.LoadAll<AudioClip>("Menuvania/Sounds/DrawerOpen")[Random.Range(0, 4)];
		}
		else {
			audioSource.clip = Resources.LoadAll<AudioClip>("Menuvania/Sounds/DrawerClosed")[Random.Range(0, 4)];
		}
	}
}
