using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviourSingleton<StateManager> {
	public enum States { Menu, InGame, Pause };

	public States currentState = States.Menu;

	public void pauseGame() {
		if (currentState == States.InGame) {
			currentState = States.Pause;
		} else {
			Debug.LogError("Can't pause if not in game");
		}
	}

	public void startGame() {
		if (currentState == States.Menu) {
			currentState = States.InGame;
		} else {
			Debug.LogError ("Can't start game if not in menu");
		}
	}

	public void backToMainMenu() {
		if (currentState == States.Pause) {
			currentState = States.Menu;
		} else {
			Debug.LogError ("Can't go to main menu unless in pause");
		}
	}
}
