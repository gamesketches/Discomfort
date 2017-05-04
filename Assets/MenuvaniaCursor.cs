using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuvaniaCursor : MonoBehaviour {

	public GameObject[] drawers;
	int focusDepth;
	Transform focusedItem;
	// Use this for initialization
	void Start () {
		drawers = GameObject.FindGameObjectsWithTag("Drawer");
		focusedItem = drawers[0].transform;
		RepositionCursor();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			focusedItem.gameObject.GetComponent<DrawerBehaviour>().ToggleDrawer();
		}
		RepositionCursor();
	}

	void RepositionCursor() {
		transform.position = focusedItem.position - new Vector3(0, 10, 0);
	}
}
