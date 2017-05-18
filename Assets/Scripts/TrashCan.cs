using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {


	GameObject lid;
	AudioSource audioSource;
	Transform player;
	SpriteRenderer renderer;
	SpriteRenderer lidRenderer;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		audioSource = GetComponent<AudioSource>();
		lid = transform.GetChild(0).gameObject;
		lidRenderer = lid.GetComponent<SpriteRenderer>();
		lid.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(player.position.y > transform.position.y) {
			renderer.sortingOrder = 5;
		}
		else {
			renderer.sortingOrder = 1;
		}
		lidRenderer.sortingOrder = renderer.sortingOrder + 1;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Poop") {
			ReceivePoop(other.gameObject);
		}
	}

	void ReceivePoop(GameObject obj) {
		Destroy(obj);
		lid.SetActive(true);
		lidRenderer.sortingOrder = renderer.sortingOrder;
		audioSource.clip = Resources.LoadAll<AudioClip>("Asokoban")[Random.Range(0, 6)];
		audioSource.Play();
	}
}
