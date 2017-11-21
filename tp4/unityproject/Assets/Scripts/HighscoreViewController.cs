using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreViewController : MonoBehaviour {
	public Text text;

	void Start() {
        List<Score> highscores = HighscoreController.Instance.GetHighscores ();
		string scores = "";
		foreach (Score score in highscores) {
			scores += score.name + ": \t" + score.score + "\n";
		}
		text.text = scores;
	}
}
