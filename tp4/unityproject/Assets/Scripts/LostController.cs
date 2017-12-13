using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LostController : MonoBehaviour {

	public Text text;
	public InputField input;
	// Use this for initialization
	void Awake () {
		text.text = "score: " + HighscoreController.Instance.getLastScore();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Submit() {
		if (input.text != null) {
			HighscoreController.Instance.SetScore (input.text, HighscoreController.Instance.getLastScore());
			SceneManager.LoadScene (3);
		}
	}
}
