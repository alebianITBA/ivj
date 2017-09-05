using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreViewController : MonoBehaviour {
	public Text text;

	// Use this for initialization
	void Awake () {
	}

	void Start() {
		List<Score> highscores = HighscoreController.instance.GetHighscores ();
		string scores = "";
		foreach (Score score in highscores) {
			scores += score.name + ": \t" + score.score + "\n";
		}
		text.text = scores;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
