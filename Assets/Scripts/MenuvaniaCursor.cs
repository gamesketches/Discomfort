using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public enum FocusDepth {Drawer, Files, Options};
public class MenuvaniaCursor : MonoBehaviour {

	public List<GameObject> drawers;
	GameObject[] allDrawers;
	FocusDepth focusDepth;
	int currentDrawer;
	public Transform focusedItem;
	GameObject player;
	bool started;
	// Use this for initialization
	IEnumerator Start () {
		started = false;
		allDrawers = GameObject.FindGameObjectsWithTag("Drawer");
		drawers = new List<GameObject>();
		drawers.Add(allDrawers[0]);
		currentDrawer = 0;
		focusedItem = drawers[currentDrawer].transform;
		RepositionCursor();
		focusDepth = FocusDepth.Drawer;
		player = GameObject.FindGameObjectWithTag("Player");
		while(!InputManager.ActiveDevice.AnyButton.WasPressed) {
			yield return null;
		}
		focusedItem.GetComponent<DrawerBehaviour>().UnlockDrawer();
		started = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(started) {
			if(InputManager.ActiveDevice.Action1.WasPressed || Input.GetKeyDown(KeyCode.Space)) {
				ConfirmButton();
			}
			else if(InputManager.ActiveDevice.Action2.WasPressed || Input.GetKeyDown(KeyCode.Q)) {
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
	}

	void ConfirmButton() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
				focusDepth++;
				break;
			case FocusDepth.Files:
				focusedItem.GetComponent<FolderBehavior>().FanOut();
				focusDepth++;
				break;
			case FocusDepth.Options:
				player.GetComponent<MenuvaniaPlayer>().PerformAction(focusedItem.gameObject.GetComponentInChildren<Text>().text);
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
				focusedItem.parent.gameObject.GetComponent<FolderBehavior>().FanIn();
				focusDepth--;
				break;
			default:
				focusDepth--;
				break;
		}
		UpdateFocusedItem();
	}

	void MoveLeft() {
		if(focusDepth == FocusDepth.Options) {
			focusedItem.parent.gameObject.GetComponent<FolderBehavior>().FanIn();
			focusDepth--;
		}
		if(focusDepth == FocusDepth.Files) {
			drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		}
		currentDrawer--;
		if(currentDrawer < 0) {
			currentDrawer = drawers.Count - 1;
		}
		if(focusDepth == FocusDepth.Files) {
			drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		}
		UpdateFocusedItem();
	}

	void MoveRight() {
		if(focusDepth == FocusDepth.Options) {
			focusedItem.parent.gameObject.GetComponent<FolderBehavior>().FanIn();
			focusDepth--;
		}
		if(focusDepth == FocusDepth.Files) {
			drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		}
		currentDrawer++;
		if(currentDrawer >= drawers.Count) {
			currentDrawer = 0;
		}
		if(focusDepth == FocusDepth.Files) {
			drawers[currentDrawer].GetComponent<DrawerBehaviour>().ToggleDrawer();
		}
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

		Debug.Log(focusedItem.name);
	}

	void RepositionCursor() {
		switch(focusDepth) {
			case FocusDepth.Drawer:
				transform.rotation = Quaternion.identity;
				transform.position = focusedItem.position - new Vector3(0, 1, 0); 
				break;
			case FocusDepth.Files:
				transform.rotation = Quaternion.Euler(0, 0, -90);
				transform.position = focusedItem.position - new Vector3(1, 0, 0);
				break;
			case FocusDepth.Options:
				transform.rotation = Quaternion.Euler(0, 0, -90);
				transform.position = focusedItem.position - new Vector3(1, 0, 0);
				break;
		};
	}

	public void UnlockDrawer(string drawerName) {
		foreach(GameObject drawer in allDrawers) {
			if(drawer.name == drawerName) {
				drawer.GetComponent<DrawerBehaviour>().UnlockDrawer();
				drawers.Add(drawer);
				break;
			}
		}
	}
}
