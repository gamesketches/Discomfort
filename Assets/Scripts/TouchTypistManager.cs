﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTypistManager : MonoBehaviour {


	delegate void ManipulationFunction();

	private class Phrase {
		public string textContent;
		public Text[] individualLetters;
		public KeyCode leftHeldKey;
		public KeyCode rightHeldKey;
		public float timeBonus;
		public Vector3 position;
		public ManipulationFunction myFunc;

		public Phrase(string content, KeyCode left, KeyCode right, float time, Vector2 newPosition, ManipulationFunction newFunc) {
			textContent = content;
			leftHeldKey = left;
			rightHeldKey = right;
			timeBonus = time;
			position = new Vector3(newPosition.x, newPosition.y, 0);
			myFunc = newFunc;
		}
	}

	public delegate void OnBoardUI();
	public static event OnBoardUI OnBoardEvent;

	Queue<Phrase> phrases;
	GameObject timer;
	public Text currentText;
	public Text leftHoldText;
	public Text rightHoldText;
	RectTransform paperSprite;
	IEnumerator fingers;
	Vector3[] keyPositions;
	Vector3 paperShadowStartPos;
	Vector3 paperLocalPos;
	public float offsetOnType;
	Phrase currentPhrase;
	int currentPhraseIndex;
	float currentTime = 1;
    public float textSlidingTime;
	AudioSource audioSource;
	Transform[] keys;
	bool playing;
    bool timerRunning = false;

	HighScoreManager highScoreList;

	// Use this for initialization
	IEnumerator Start () {
		playing = false;
		SetUpFingers();
		keys = GameObject.Find("Keys").GetComponentsInChildren<Transform>();
		paperSprite = GameObject.Find("Paper").GetComponent<RectTransform>();
		paperLocalPos = paperSprite.localPosition;
		paperShadowStartPos = paperSprite.transform.parent.gameObject.GetComponent<UISlideIn>().targetPosition;
		highScoreList = GetComponent<HighScoreManager>();
		SetUpPhrases();

		currentPhrase = phrases.Dequeue();
		currentText.text = currentPhrase.textContent;
		currentText.enabled = false;
		leftHoldText.text = "";
		rightHoldText.text = "";
        currentTime = 0.4f;
		currentPhraseIndex = 0;

		audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("TouchTypist/slide");
        timer = GameObject.Find("clockLeg");

		GameObject logo = GameObject.Find("Logo");
		GameObject logoText = GameObject.Find("LogoText");
		while(!Input.GetKeyDown(KeyCode.Return)) {
			currentTime += Time.deltaTime;
			yield return null;
		}

		OnBoardEvent();
        AddTime(currentPhrase.timeBonus, 0.4f);
        playing = true;
		//logo.SetActive(false);
		logoText.SetActive(false);
		currentText.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
		if(playing) {
            UpdateTimer();
			if( currentTime > 0 && KeysStillHeld()){
				InputLetter();
			}
		}
	}

    void UpdateTimer() {
        timer.transform.localRotation = Quaternion.Euler(0, 0, 180 - (6 * currentTime));
        if (timerRunning) {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) {
                playing = false;
                currentText.text = "OUT OF TIME,\n enter to retry";
                leftHoldText.color = Color.clear;
                rightHoldText.color = Color.clear;

                StartCoroutine(ResetGame());
                }
           }
    }

    void InputLetter() {
		if(Input.GetKeyDown(CurrentCharacter())) {
			MoveFinger(CurrentCharacter());
			currentPhraseIndex += 1;
			paperSprite.Translate(offsetOnType, 0, 0);
			paperSprite.parent.Translate(offsetOnType / 1.2f, 0, 0);
			if(currentPhraseIndex == currentPhrase.textContent.Length) {
				SwitchPhrase();
			}
			else if(currentPhrase.textContent[currentPhraseIndex] == ' '){
				currentPhraseIndex += 1;
			}
			currentText.text = string.Concat("<color=black>", currentPhrase.textContent.Substring(0, currentPhraseIndex), "</color>", currentPhrase.textContent.Substring(currentPhraseIndex));

		}
	}

	void SwitchPhrase() {
		if(phrases.Count >= 1) {
			StartCoroutine(FinishPhrase());
		}
		else {
			StartCoroutine(ResetGame());
		}

	}

	void MoveFinger(string letter) {
		FingerBehavior curFinger = (FingerBehavior)fingers.Current;
		Vector3 position = new Vector3(Random.Range(-3f, 2.5f), Random.Range(-2.6f, -4.5f));
		foreach(Transform key in keys) {
			if(key.name == letter.ToLower()) {
				position = key.position;
				break;
			}
		}
		curFinger.MoveToTarget(position);
		if(!fingers.MoveNext()) {
			fingers.Reset();
			fingers.MoveNext();
		}
	}

	IEnumerator FinishPhrase() {
        timerRunning = false;
		currentText.text = string.Concat("<color=black>", currentPhrase.textContent, "</color>");
		Phrase nextPhrase = phrases.Peek();
		audioSource.Play();
        float lerpTime = audioSource.clip.length;
		Vector3 offset;
		if(nextPhrase.rightHeldKey == KeyCode.None) {
			offset = new Vector3(0, 100);
		}
		else {
			offset = new Vector3(0, 200);
		}
        StartCoroutine(AddTime(nextPhrase.timeBonus, lerpTime + 1));
        RectTransform paperShadow = GameObject.Find("paperShadow").GetComponent<RectTransform>();
		Vector3 shadowStart = paperShadow.transform.position;
		Vector3 startPos = paperSprite.transform.parent.position;
		Vector3 endPos = paperShadowStartPos;//Vector3.zero + new Vector3(0, paperSprite.transform.position.y, paperSprite.transform.position.z);
		for(float t = 0; t < lerpTime; t += Time.deltaTime) {
			paperSprite.transform.localPosition = Vector3.Lerp(paperSprite.transform.localPosition, paperLocalPos, t / lerpTime);
			paperSprite.transform.parent.position = Vector3.Lerp(startPos, endPos, t / lerpTime);
			yield return null;
		}
        paperSprite.transform.localPosition = paperLocalPos;
        paperSprite.transform.parent.position = endPos;
		ShedText(currentText);
		currentText.rectTransform.transform.Translate(-offset / 2);
		foreach(GameObject text in GameObject.FindGameObjectsWithTag("FinishedText")) {
				StartCoroutine(MoveText(text.GetComponent<Text>(), offset));
			}
		currentPhrase = phrases.Dequeue();
        leftHoldText.text = HeldKeyText(currentPhrase.leftHeldKey);
        rightHoldText.text = HeldKeyText(currentPhrase.rightHeldKey);

        currentText.text = currentPhrase.textContent;
		yield return StartCoroutine(MoveText(currentText, offset / 2));
		currentPhraseIndex = 0;
        timerRunning = true;
    }

	IEnumerator MoveText(Text theText, Vector3 offset) {
		float t = 0;
		Vector3 startPos = theText.rectTransform.transform.position;
		Vector3 endPos = startPos + offset;
		while(t < textSlidingTime) {
			currentTime += Time.deltaTime;
			theText.rectTransform.transform.position = Vector3.Lerp(startPos, endPos, 
                                                                Mathf.SmoothStep(0,1,t / textSlidingTime));
			t += Time.deltaTime;
			yield return null;
		}
	}

	bool KeysStillHeld() {
		if(currentPhraseIndex >= currentPhrase.textContent.Length) {
			return false;
		}
		UpdateHeldKeyColors();
		if(Input.anyKey && KeyHeld(currentPhrase.leftHeldKey) && KeyHeld(currentPhrase.rightHeldKey)) {
			return true;
		}
		return false;
	}

	string CurrentCharacter() {
		if(currentPhraseIndex >= currentPhrase.textContent.Length) {
			return " ";
		}
		return currentPhrase.textContent[currentPhraseIndex].ToString().ToLower();
	}

	void UpdateHeldKeyColors() {
		if(currentPhrase.leftHeldKey == KeyCode.None) {
			leftHoldText.color = Color.clear;
		}
		else if(KeyHeld(currentPhrase.leftHeldKey)) {
			leftHoldText.color = Color.red;
		}
		else {
			leftHoldText.color = Color.blue;
		}

		if(currentPhrase.rightHeldKey == KeyCode.None) {
			rightHoldText.color = Color.clear;
		}
		else if(KeyHeld(currentPhrase.rightHeldKey)) {
			rightHoldText.color = Color.red;
		}
		else {
			rightHoldText.color = Color.blue;
		}
	}

	bool KeyHeld(KeyCode inputKey) {
		if(inputKey == KeyCode.None || Input.GetKey(inputKey)) {
			return true;
		}
		else {
			return false;
		}
	}

	void SetUpFingers() {
		GameObject[] fingerArray = GameObject.FindGameObjectsWithTag("Finger");
		FingerBehavior[] fingerScripts = new FingerBehavior[fingerArray.Length];
		for(int i = 0; i < fingerArray.Length; i++) {
			fingerScripts[i] = fingerArray[i].GetComponent<FingerBehavior>();
		}
		fingers = fingerScripts.GetEnumerator();
		fingers.MoveNext();
	}

	void SetUpPhrases() {
		phrases = new Queue<Phrase>();
        phrases.Enqueue(new Phrase("Type these letters", KeyCode.None, KeyCode.None, 14, Vector2.zero, null));
        phrases.Enqueue(new Phrase ("And Mind The Timer", KeyCode.None, KeyCode.None, 4, Vector2.zero, null));
		phrases.Enqueue(new Phrase("Blue letters must be held", KeyCode.K, KeyCode.None, 4, Vector2.zero, null));
		phrases.Enqueue(new Phrase("Letting go is starting over", KeyCode.J, KeyCode.None, 8, Vector2.zero, null));
		phrases.Enqueue(new Phrase("Now it begins", KeyCode.R, KeyCode.U, 2, Vector2.zero, null));
        SetUpPostTutorialPhrases();
	}

	void SetUpPostTutorialPhrases() {
		phrases.Enqueue(new Phrase("The man is also filial piety", KeyCode.W, KeyCode.V,4, Vector3.zero, null));
		phrases.Enqueue(new Phrase("and good guilty of those who", KeyCode.P, KeyCode.Z, 4, Vector3.zero, null));
		phrases.Enqueue(new Phrase("fresh bad guilty", KeyCode.X, KeyCode.P, 7, Vector3.zero, null));
		phrases.Enqueue(new Phrase("and good for chaos", KeyCode.U, KeyCode.K, 6, Vector3.zero, null));
		phrases.Enqueue(new Phrase("not the there", KeyCode.S, KeyCode.J, 2, Vector3.zero, null));
		phrases.Enqueue(new Phrase("Gentleman of this", KeyCode.X, KeyCode.R, 4, Vector3.zero, null));
		phrases.Enqueue(new Phrase ("the legislation and students", KeyCode.Q, KeyCode.V, 9, Vector2.zero, null));
		phrases.Enqueue(new Phrase("You Win!", KeyCode.None, KeyCode.None, 0, Vector2.zero, null));
	}

	IEnumerator AddTime(float timeBonus, float lerpTime) {
        SpriteRenderer timerRenderer = timer.GetComponent<SpriteRenderer>();
        timerRenderer.color = new Color(101f / 255f, 255f / 255f, 140f / 255f);
        float startTime = currentTime;
		float endTime = currentTime + timeBonus;
		for(float t = 0; t < lerpTime; t += Time.deltaTime) {
            currentTime = Mathf.Lerp(startTime, endTime, t / lerpTime);
			yield return null;
		}
        currentTime = endTime;
        timerRenderer.color = Color.black;
    }

	IEnumerator ResetGame() {
		Debug.Log("Resetting");
		//yield return new WaitForSeconds(4);
		for(float t = 4; t > 0; t -= Time.deltaTime) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				if(phrases.Count < 9) {
					phrases.Clear();
					SetUpPostTutorialPhrases();
				}
				else {
					phrases.Clear();
					SetUpPhrases();
				}
				currentTime = 2;
				currentPhrase = new Phrase("Good luck!", KeyCode.None, KeyCode.None, 0, Vector3.zero, null);
				leftHoldText.color = Color.blue;
				rightHoldText.color = Color.blue;
				StartCoroutine(FinishPhrase());
				playing = true;
				break;
			}
			yield return null;
		}
		if(!playing) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}

	void ShedText(Text copyText) {
		Text oldText = Instantiate(copyText, copyText.transform.parent) as Text;
		oldText.tag = "FinishedText";
		oldText.color = Color.black;
	}

	string HeldKeyText(KeyCode heldKey) {
		if(heldKey == KeyCode.None) {
			return "";
		}
		else return heldKey.ToString();
	}
}
