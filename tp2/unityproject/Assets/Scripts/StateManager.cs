using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviourSingleton<StateManager> {
	public enum States { Menu, InGame, Pause };
	public enum GameModes { OnePlayer, TwoPlayers };
	public enum Players { PlayerOne, PlayerTwo };

	// TODO: Make the state start in Menu
	public States currentState = States.InGame;
	public GameModes currentGameMode;
	public Players currentPlayer;
	public int currentPlayerMoves = 1;

	public Dictionary<Players, int> scores = new Dictionary<Players, int>();
	public PocketCollider.Pocket lastPlayerOnePocket;
	public PocketCollider.Pocket lastPlayerTwoPocket;

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

	public void StartOnePlayerGame() {
		if (currentState == States.Menu) {
			currentPlayer = Players.PlayerOne;
			currentGameMode = GameModes.OnePlayer;
			currentState = States.InGame;
		} else {
			LogInvalidTransition (States.InGame);
		}
	}

	public void StartTwoPlayersGame() {
		if (currentState == States.Menu) {
			currentGameMode = GameModes.TwoPlayers;
			currentState = States.InGame;
			float rand = Random.value;
			if (rand >= 0.5) {
				currentPlayer = Players.PlayerOne;
			} else {
				currentPlayer = Players.PlayerTwo;
			}
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

	public void BallEnteredInPocket(GameObject ballGo, PocketCollider.Pocket pocketId) {
		Ball ball = ballGo.GetComponent<Ball>();

		switch (ball.type) {
			case Ball.BallTypes.White:
				WhiteInPocket (ball, pocketId);
				break;
			case Ball.BallTypes.Black:
				BlackInPocket (ball, pocketId);
				break;
			default:
				BallInPocket (ball, pocketId);
				break;
		}
	}

	private void WhiteInPocket(Ball white, PocketCollider.Pocket pocketId) {
		white.transform.position = BallManager.Instance.DefaultWhitePosition ();
		SwitchPlayer ();
		currentPlayerMoves = 2;
	}

	private void BlackInPocket(Ball white, PocketCollider.Pocket pocketId) {

	}

	private void BallInPocket(Ball white, PocketCollider.Pocket pocketId) {
		
	}

	private void SwitchPlayer() {
		if (currentPlayer == Players.PlayerOne) {
			currentPlayer = Players.PlayerTwo;
		} else {
			currentPlayer = Players.PlayerOne;
		}
	}
}
