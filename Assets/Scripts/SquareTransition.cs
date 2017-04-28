using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransferDirection {Up, Down, Left, Right};
[ExecuteInEditMode]
public class SquareTransition : MonoBehaviour {

	public TransferDirection transferDirection;
	public float transitionTime;
	public Vector3 playerTransferVector;
	public float targetRotation;
	public WaterBehavior roomHole;
	public WaterBehavior neighborHole;
	// Use this for initialization
	void Start () {
		SetRoomHole(false);
		SetNeighborHole(false);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, playerTransferVector, Color.blue);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			SetRoomHole(false);
			SetNeighborHole(true);
			StartCoroutine(SwitchRoom(other.gameObject));
		}
	}

	IEnumerator SwitchRoom(GameObject player) {
		player.tag = "Untagged";
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
		Quaternion startRotation = Camera.main.transform.rotation;
		Quaternion targetRot = Quaternion.Euler(0, 0, targetRotation);
		for(float t = 0; t <= transitionTime; t += Time.deltaTime) {
			Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRot, t / transitionTime);
			Camera.main.transform.position = Vector3.Slerp(startPos, startPos + cameraVector, t / transitionTime);
			player.transform.position = Vector3.Lerp(playerStartPos, playerTransferVector, t / transitionTime);
			yield return null;
		}
		Camera.main.transform.rotation = targetRot;
		Camera.main.transform.position = startPos + cameraVector;
		player.tag = "Player";
	}

	void SetRoomHole(bool state) {
		if(roomHole != null) {
			roomHole.sucking = state;
		}
	}

	void SetNeighborHole(bool state) {
		if(neighborHole != null) {
			neighborHole.sucking = state;
		}
	}
}
