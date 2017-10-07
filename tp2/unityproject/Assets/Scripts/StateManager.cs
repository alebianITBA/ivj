using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviourSingleton<StateManager> {
	public enum States { Menu, InGame, Pause };

	// TODO: Make the state start in Menu
	public States currentState = States.InGame;

	public void PauseGame() {
		if (currentState == States.InGame) {
			currentState = States.Pause;
		} else {
			LogInvalidTransition (States.Pause);
		}
	}

	public void ContinueGame() {
		if (currentState == States.Pause) {
			currentState = States.InGame;
		} else {
			LogInvalidTransition (States.InGame);
		}
	}

	public void StartGame() {
		if (currentState == States.Menu) {
			currentState = States.InGame;
		} else {
			LogInvalidTransition (States.InGame);
		}
	}

	public void BackToMainMenu() {
		if (currentState == States.Pause) {
			currentState = States.Menu;
		} else {
			LogInvalidTransition (States.Menu);
		}
	}

	public bool Paused() {
		return currentState.Equals(States.Pause);
	}

	private void LogInvalidTransition(States toState) {
		Debug.LogError ("Current state is: " + currentState.ToString () + ", cannot transition to: " + toState.ToString ());
	}
}
