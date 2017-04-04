using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour {

	public class Entry {
		public string name;
		public float score;

		public Entry(string newName, float newScore) {
			name = newName;
			score = newScore;
		}

		public string GetEntry() {
			if(score != 0) {
				return string.Concat(name, " ", score.ToString("F"));
			}
			else {
				return string.Concat(name, " ----");
			}
		}

		public string scoreVal() {
			return score.ToString();
		}
	}

	public Text HighScoreDisplay;
	public int numEntries = 10;
	public List<Entry> entries;
	private string HighScoreHeader;
	private string nameEntry;
	private float scoreEntry;
	bool enteringScore;

	// Use this for initialization
	void Start () {
		enteringScore = false;
		HighScoreHeader = SceneManager.GetActiveScene().name + "HighScore";
		entries = new List<Entry>();
		string key;
		for(int i = 0; i < numEntries; i++) {
			key = HighScoreHeader + i.ToString();
			if(PlayerPrefs.HasKey(key + "score")) {
				entries.Add(new Entry(PlayerPrefs.GetString(key + "name"), PlayerPrefs.GetFloat(key + "score")));
			}
			else {
				entries.Add(new Entry("Sam", 0));
			}
		}
	}

	void Update() {
		if(enteringScore) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				CreateNewEntry(nameEntry, scoreEntry);
				enteringScore = false;
				StartCoroutine(DisplayAndReset());
			}
			else if(Input.GetKeyDown(KeyCode.Backspace)) {
				nameEntry = nameEntry.Substring(0, nameEntry.Length - 1);
				HighScoreDisplay.text = "Fast Time!\n Enter Your name:\n " + nameEntry;
			}
			else {
				HighScoreDisplay.text = "Fast Time!\n Enter Your name:\n " + nameEntry;
				nameEntry = string.Concat(nameEntry, Input.inputString);
			}
		}
	}

	public void SetScores() {
		string key;
		for(int i = 0; i < numEntries; i++) {
			key = HighScoreHeader + i.ToString();
			PlayerPrefs.SetString(key + "name", entries[i].name);
			PlayerPrefs.SetFloat(key + "score", entries[i].score);
		}
	}

	public void PrintScores() {
		string scoreList = "High Scores:\n";
		foreach(Entry entry in entries) {
			scoreList = string.Concat(scoreList, entry.GetEntry(), "\n");
		}
		HighScoreDisplay.text = scoreList;
	}

	public void ClearLeaderBoard() {
		string key;
		for(int i = 0; i < numEntries; i++) {
			key = HighScoreHeader + i.ToString();
			PlayerPrefs.DeleteKey(key + "name");
			PlayerPrefs.DeleteKey(key + "score");
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
	
	public bool MadeHighScoreList(int score) {
		return score > entries[numEntries -1].score;
	}

	public void CreateNewEntry(string name, float score) {
		Entry newChallenger = new Entry(name, score);
		for(int i = 0; i < entries.Count; i++) {
			if(entries[i].score < newChallenger.score) {
				entries.Insert(i, newChallenger);
				break;
			}
		}
		SetScores();
		entries.Remove(entries[numEntries]);
	}

	public void InputNewName(float score) {
		scoreEntry = score;
		StartCoroutine(DelayInputOneFrame());
	}

	IEnumerator DelayInputOneFrame() {
		yield return null;
		enteringScore = true;
	}

	IEnumerator DisplayAndReset() {
		PrintScores();
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
