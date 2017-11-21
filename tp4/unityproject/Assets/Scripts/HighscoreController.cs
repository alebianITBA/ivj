using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class HighscoreController : MonoBehaviour {
    private List<Score> highscores = new List<Score> ();
	private int lastScore;

	public static HighscoreController Instance = null;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}

        DontDestroyOnLoad (gameObject);
        highscores = new List<Score> ();
        Load ();
    }

	public int getLastScore() {
		return lastScore;
	}

	public void setLastScore(int score) {
		lastScore = score;	
	}

	public void SetScore(string playerName, int score) {
		highscores.Add (new Score(playerName, score));
		highscores.Sort ((Score x, Score y) => y.score.CompareTo(x.score));
		Save ();
	}

	public List<Score> GetHighscores() {
		return highscores;
	}

	void OnApplicationQuit() {
		Save ();
	}

	void Save() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/highscores.dat", FileMode.OpenOrCreate);
		bf.Serialize (file, highscores);
		file.Close ();
	}

	void Load() {
		if (File.Exists (Application.persistentDataPath + "/highscores.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/highscores.dat", FileMode.Open);
			highscores = (List<Score>) bf.Deserialize (file);
			file.Close ();
		}
	}
}
