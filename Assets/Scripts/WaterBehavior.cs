using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour {

	Rigidbody2D player;
	public bool sucking;
	Transform swirl;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
		swirl = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		if(sucking) {
			Vector2 suckForce = new Vector2(transform.position.x - player.position.x, transform.position.y - player.transform.position.y);
			player.AddForce(suckForce * 1.5f);
		}
		swirl.Rotate(0, 0, -90 * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			StartCoroutine(SuckInPlayer(other.transform));
		}
	}

	IEnumerator SuckInPlayer(Transform player) {
		player.gameObject.GetComponent<AsokobanPlayer>().speed = 0;
		player.parent = transform;
		Vector3 startPos = player.localPosition;
		for(float t = 0; t < 1; t += Time.deltaTime) {
			player.localScale = Vector3.Lerp(new Vector3(1, 1, 1), Vector3.zero, t);
			player.Rotate(0, 0, -270 * Time.deltaTime);
			yield return null;
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Asokoban");
	}
}
