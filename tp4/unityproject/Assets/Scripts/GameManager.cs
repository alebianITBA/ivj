using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
	void Start() {
		LevelManager.Instance.CreateNewLevel ();
	}

	void Update() {
		
	}
}
