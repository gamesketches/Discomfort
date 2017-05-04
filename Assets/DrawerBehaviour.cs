using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerBehaviour : MonoBehaviour {

	private Vector3 startPosition;
	public static float movement = -100;
	public static float movementTime = 0.3f;
	bool toggled;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleDrawer() {
		if(toggled) {
			StartCoroutine(ChangePosition(startPosition + new Vector3(0, movement, 0)));	
		}
		else {
			StartCoroutine(ChangePosition(startPosition));
		}
		toggled = !toggled;
	}

	IEnumerator ChangePosition(Vector3 destination) {
		Vector3 currentPosition = transform.position;
		for(float t = 0; t <= movementTime; t += Time.deltaTime) {
			transform.position = Vector3.Slerp(currentPosition, destination, t / movementTime);
			Debug.Log(transform.position);
			yield return null;
		}
	}
}
