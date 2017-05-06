﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public enum FocusDepth {Drawer, Files, Options};
public class MenuvaniaCursor : MonoBehaviour {

	public GameObject[] drawers;
	FocusDepth focusDepth;
	int currentDrawer;
	public Transform focusedItem;
	// Use this for initialization
	void Start () {
		drawers = GameObject.FindGameObjectsWithTag("Drawer");
		currentDrawer = 0;
		focusedItem = drawers[currentDrawer].transform;
		RepositionCursor();
		focusDepth = FocusDepth.Drawer;
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.ActiveDevice.Action1.WasPressed || Input.GetKeyDown(KeyCode.Space)) {
			ConfirmButton();
		}
		else if(InputManager.ActiveDevice.Action3.WasPressed || Input.GetKeyDown(KeyCode.Q)) {
			CancelButton();
		}
		else if(InputManager.ActiveDevice.DPadLeft.WasPressed || Input.GetKeyDown(KeyCode.LeftArrow)) {
			MoveLeft();
		}
		else if(InputManager.ActiveDevice.DPadRight.WasPressed || Input.GetKeyDown(KeyCode.RightArrow)) {
			MoveRight();
		}
		else if(InputManager.ActiveDevice.DPad.Up.WasPressed || Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveUp();
		}
		else if(InputManager.ActiveDevice.DPad.Down.WasPressed || Input.GetKeyDown(KeyCode.DownArrow)) {
			MoveDown();
		}
		RepositionCursor();
	}

	void ConfirmButton() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
				focusDepth++;
				break;
			case FocusDepth.Files:
				focusDepth++;
				break;
			case FocusDepth.Options:
				Debug.Log(focusedItem.gameObject.GetComponentInChildren<Text>().text);
				return;
			default:
				focusDepth++;
				break;
		}
		UpdateFocusedItem();
	}

	void CancelButton() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				return;
			case FocusDepth.Files:
				drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
				focusDepth--;
				break;
			case FocusDepth.Options:
				focusDepth--;
				break;
			default:
				focusDepth--;
				break;
		}
		UpdateFocusedItem();
	}

	void MoveLeft() {
		drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		currentDrawer--;
		if(currentDrawer < 0) {
			currentDrawer = drawers.Length - 1;
		}
		drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		UpdateFocusedItem();
	}

	void MoveRight() {
		drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		currentDrawer++;
		if(currentDrawer >= drawers.Length) {
			currentDrawer = 0;
		}
		drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		UpdateFocusedItem();
	}

	void MoveUp() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				return;
			case FocusDepth.Files:
				Transform[] options = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options;
				if(focusedItem.GetSiblingIndex() == options.Length - 1) {
					focusedItem = options[0];
				}
				else {
					focusedItem = options[focusedItem.GetSiblingIndex() + 1];
				}
				break;
			case FocusDepth.Options:
				Transform[] verbs = focusedItem.parent.gameObject.GetComponent<FolderBehavior>().options;
				if(focusedItem.GetSiblingIndex() == 1) {
					focusedItem = verbs[verbs.Length - 1];
				}
				else {
					focusedItem = verbs[focusedItem.GetSiblingIndex() - 2];
				}
				break;
			default:
				return;
		}
	}

	void MoveDown() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				return;
			case FocusDepth.Files:
			Transform[] options = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options;
				if(focusedItem.GetSiblingIndex() == 0) {
					focusedItem = options[options.Length - 1];
				}
				else {
					focusedItem = options[focusedItem.GetSiblingIndex() - 1];
				}
				break;
			case FocusDepth.Options:
				Transform[] verbs = focusedItem.parent.gameObject.GetComponent<FolderBehavior>().options;
				if(focusedItem.GetSiblingIndex() == verbs.Length) {
					focusedItem = verbs[0];
				}
				else {
					focusedItem = verbs[focusedItem.GetSiblingIndex()];
				}
				break;
			default:
				return;
		}
	}
	void UpdateFocusedItem() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				focusedItem = drawers[currentDrawer].transform;
				break;
			case FocusDepth.Files:
				focusedItem = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options[0];
				break;
			case FocusDepth.Options:
				focusedItem = focusedItem.gameObject.GetComponent<FolderBehavior>().options[0];
				break;
			default:
				focusedItem = drawers[currentDrawer].transform;
				break;
		}
	}

	void RepositionCursor() {
		transform.position = focusedItem.position - new Vector3(0, 10, 0);
	}
}
