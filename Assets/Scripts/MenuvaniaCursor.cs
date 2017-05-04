using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public enum FocusLevel {Drawer, Files, Options};
public class MenuvaniaCursor : MonoBehaviour {

	public GameObject[] drawers;
	int focusDepth;
	int currentDrawer;
	public Transform focusedItem;
	// Use this for initialization
	void Start () {
		drawers = GameObject.FindGameObjectsWithTag("Drawer");
		currentDrawer = 0;
		focusedItem = drawers[currentDrawer].transform;
		RepositionCursor();
		focusDepth = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.ActiveDevice.Action1.WasPressed) {
			ConfirmButton();
		}
		else if(InputManager.ActiveDevice.Action3.WasPressed) {
			CancelButton();
		}
		else if(InputManager.ActiveDevice.DPadLeft.WasPressed) {
			MoveLeft();
		}
		else if(InputManager.ActiveDevice.DPadRight.WasPressed) {
			MoveRight();
		}
		else if(InputManager.ActiveDevice.DPad.Up.WasPressed) {
			MoveUp();
		}
		else if(InputManager.ActiveDevice.DPad.Down.WasPressed) {
			MoveDown();
		}
		RepositionCursor();
	}

	void ConfirmButton() {
		switch(focusDepth) {
			case 0:
				drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
				focusDepth++;
				break;
			case 1:
				//focusedItem = focusedItem.gameObject.GetComponent<Dropdown>().options[0];
				//focusDepth++;
				break;
			case 2:
				return;
			default:
				focusDepth++;
				break;
		}
		UpdateFocusedItem();
	}

	void CancelButton() {
		switch(focusDepth) {
			case 0:
				return;
			case 1:
				drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
				focusDepth--;
				break;
			default:
				focusDepth--;
				break;
		}
		UpdateFocusedItem();
	}

	void MoveLeft() {
		currentDrawer--;
		if(currentDrawer < 0) {
			currentDrawer = drawers.Length - 1;
		}

		UpdateFocusedItem();
	}

	void MoveRight() {
		currentDrawer++;
		if(currentDrawer >= drawers.Length) {
			currentDrawer = 0;
		}

		UpdateFocusedItem();
	}

	void MoveUp() {
		switch(focusDepth) {
			case 0:
				return;
			case 1:
				Transform[] options = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options;
				if(focusedItem.GetSiblingIndex() == options.Length - 1) {
					focusedItem = options[0];
				}
				else {
					focusedItem = options[focusedItem.GetSiblingIndex() + 1];
				}
				break;
			default:
				return;
		}
	}

	void MoveDown() {
		switch(focusDepth) {
			case 0:
				return;
			case 1:
			Transform[] options = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options;
				if(focusedItem.GetSiblingIndex() == 0) {
					focusedItem = options[options.Length];
				}
				else {
					focusedItem = options[focusedItem.GetSiblingIndex() - 1];
				}
				break;
			default:
				return;
		}
	}
	void UpdateFocusedItem() {
		switch(focusDepth) {
			case 0:
				focusedItem = drawers[currentDrawer].transform;
				break;
			case 1:
				focusedItem = drawers[currentDrawer].GetComponent<DrawerBehaviour>().options[0];
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
