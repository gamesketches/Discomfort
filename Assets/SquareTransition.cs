using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransferDirection {Up, Down, Left, Right};
[ExecuteInEditMode]
public class SquareTransition : MonoBehaviour {

	public TransferDirection transferDirection;
	public float transitionTime;
	public Vector3 playerTransferVector;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, playerTransferVector, Color.blue);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			StartCoroutine(SwitchRoom(other.gameObject));
		}
	}

	IEnumerator SwitchRoom(GameObject player) {
		player.tag = "Untagged";
		Vector3 cameraVector;
		switch(transferDirection) {
			case TransferDirection.Up:
				cameraVector = new Vector3(0, 20, 0);
				break;
			case TransferDirection.Down:
				cameraVector = new Vector3(0, -20, 0);
				break;
			case TransferDirection.Left:
				cameraVector = new Vector3(-20, 0, 0);
				break;
			case TransferDirection.Right:
				cameraVector = new Vector3(20, 0, 0);
				break;
			default:
				Debug.LogError("illegal direction sent");
				cameraVector = Vector3.zero;
				break;
		}
		Vector3 startPos = Camera.main.transform.position;
		Vector3 playerStartPos = player.transform.position;
		for(float t = 0; t < transitionTime; t += Time.deltaTime) {
			Camera.main.transform.position = Vector3.Slerp(startPos, startPos + cameraVector, t / transitionTime);
			player.transform.position = Vector3.Lerp(playerStartPos, playerTransferVector, t / transitionTime);
			yield return null;
		}
		player.tag = "Player";
	}
}
