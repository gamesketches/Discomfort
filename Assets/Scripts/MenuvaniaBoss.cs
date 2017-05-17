using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuvaniaBoss : MonoBehaviour {

	public Text titleText;
	Vector3 cameraPosition;
	// Use this for initialization
	void Start () {
		cameraPosition = Camera.main.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			other.GetComponent<MenuvaniaPlayer>().Die();
		}
		else if(other.tag == "Hitbox") {
			StartCoroutine(EndState());
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	IEnumerator EndState() {
		titleText.text = "You Win!";
		Vector3 startPos = Camera.main.transform.localPosition;
		for(float t = 0; t < 1; t += Time.deltaTime){
			Camera.main.transform.localPosition = Vector3.Lerp(startPos, cameraPosition, t);
			yield return null;
		}

		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("Menuvania");
	}
}
