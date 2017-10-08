using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviourSingleton<StateManager> {
	// Enums
	public enum States { Menu, Striking, InGame, Pause, Finished };
	public enum GameModes { OnePlayer, TwoPlayers };
	public enum Players { PlayerOne, PlayerTwo };
	// Constants
	private static Color ACTIVE_COLOR = new Color32( 0xFF, 0xE2, 0x59, 0xFF );
	private static Color INACTIVE_COLOR = new Color32( 0xE0, 0x83, 0x28, 0xFF );
	// Game logic
	// TODO: Make the state start in Menu
	public States currentState = States.InGame;
	public GameModes currentGameMode = GameModes.TwoPlayers;
	public Players currentPlayer = Players.PlayerOne;
	public int currentPlayerMoves = 1;
	public CueManager cue;
	public Dictionary<Players, int> scores = InitializeScores();
	public Dictionary<Players, Ball.BallTypes> ballTypes = InitializeBallTypes();
	public PocketCollider.Pocket lastPlayerOnePocket;
	public PocketCollider.Pocket lastPlayerTwoPocket;
	public Players winner;
	private bool whiteInPocket = false;
	private Ball white;

	// Game variables
	public GameObject pauseCanvas;
	public GameObject finishedCanvas;
	public Text playerOneText;
	public Text playerTwoText;
	public Text WinnerText;

	void Update() {
		if (currentState != States.Menu) {
			if (currentState != States.Finished) {
				finishedCanvas.SetActive (false);
				// Set Texts
				playerOneText.gameObject.SetActive (true);
				playerOneText.text = PlayerText (Players.PlayerOne);
				if (currentGameMode == GameModes.TwoPlayers) {
					playerTwoText.gameObject.SetActive (true);
					playerTwoText.text = PlayerText (Players.PlayerTwo);
				}
				// Bold
				if (currentPlayer == Players.PlayerOne) {
					playerOneText.fontStyle = FontStyle.Bold;
					playerTwoText.fontStyle = FontStyle.Normal;
					playerOneText.fontSize = 16;
					playerTwoText.fontSize = 14;
					playerOneText.color = ACTIVE_COLOR;
					playerTwoText.color = INACTIVE_COLOR;
				} else if (currentPlayer == Players.PlayerTwo) {
					playerOneText.fontStyle = FontStyle.Normal;
					playerTwoText.fontStyle = FontStyle.Bold;
					playerOneText.fontSize = 14;
					playerTwoText.fontSize = 16;
					playerOneText.color = INACTIVE_COLOR;
					playerTwoText.color = ACTIVE_COLOR;
				}
				// Change player
				if (BallManager.Instance.Still ()) {
					if (currentPlayerMoves <= 0) {
						SwitchPlayer ();
						currentPlayerMoves++;
					}
					if (whiteInPocket) {
						this.white.transform.position = BallManager.Instance.DefaultWhitePosition ();
						currentPlayerMoves++;
						this.whiteInPocket = false;

					}
				}
			} else {
				finishedCanvas.SetActive (true);
				WinnerText.text = "Winner " + PlayerString(currentPlayer) + "!";
			}
		} else {
			playerOneText.gameObject.SetActive (false);
			playerTwoText.gameObject.SetActive (false);
		}
	}

	public void PauseGame() {
		if (currentState == States.InGame || currentState == States.Striking) {
			currentState = States.Pause;
			pauseCanvas.SetActive(true);
		} else {
			LogInvalidTransition (States.Pause);
		}
	}

	public void ReadyToStrike() {
		if (currentState == States.InGame) {
			cue.ResetCue ();
			cue.SetVisible(true);
			currentState = States.Striking;
		} else {
			LogInvalidTransition (States.Striking);
		}
	}

	public void Strike() {
		if (currentState == States.Striking) {
			cue.SetVisible (false);
			currentState = States.InGame;
		}
		else {
			LogInvalidTransition (States.Striking);
		}
	}

	public void ContinueGame() {
		if (currentState == States.Pause) {
			currentState = States.InGame;
			pauseCanvas.SetActive(false);
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

	public bool Striking() {
		return currentState.Equals (States.Striking);
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
		whiteInPocket = true;
		this.white = white;
	}

	private void BlackInPocket(Ball black, PocketCollider.Pocket pocketId) {
		// Win logic
		if (currentPlayer == Players.PlayerOne && lastPlayerOnePocket == pocketId) {
			winner = Players.PlayerOne;
			currentState = States.Finished;
		} else if (currentPlayer == Players.PlayerTwo && lastPlayerTwoPocket == pocketId) {
			winner = Players.PlayerTwo;
			currentState = States.Finished;
		}
		// Lose logic
		if (currentState != States.Finished) {
			currentState = States.Finished;
			if (currentPlayer == Players.PlayerOne) {
				winner = Players.PlayerTwo;
			} else {
				winner = Players.PlayerOne;
			}
		}
		BallManager.Instance.RemoveBall (black);
		Destroy (black.gameObject);
	}

	private void BallInPocket(Ball ball, PocketCollider.Pocket pocketId) {
		Ball.BallTypes playerOneBallType = ballTypes[Players.PlayerOne];
		Ball.BallTypes playerTwoBallType = ballTypes[Players.PlayerTwo];
		// Set ball type to players
		if (playerOneBallType == Ball.BallTypes.None) {
			if (currentPlayer == Players.PlayerOne) {
				ballTypes[Players.PlayerOne] = ball.type;
				if (ball.type == Ball.BallTypes.Solid) {
					ballTypes[Players.PlayerTwo] = Ball.BallTypes.Striped;
				} else {
					ballTypes[Players.PlayerTwo] = Ball.BallTypes.Solid;
				}
			} else {
				ballTypes[Players.PlayerTwo] = ball.type;
				if (ball.type == Ball.BallTypes.Solid) {
					ballTypes[Players.PlayerOne] = Ball.BallTypes.Striped;
				} else {
					ballTypes[Players.PlayerOne] = Ball.BallTypes.Solid;
				}
			}
		}
		// Sum score
		if (playerOneBallType == Ball.BallTypes.Striped) {
			if (ball.type == Ball.BallTypes.Striped) {
				IncreasePlayerScore (Players.PlayerOne);
			} else {
				IncreasePlayerScore (Players.PlayerTwo);
			}
		} else {
			if (ball.type == Ball.BallTypes.Striped) {
				IncreasePlayerScore (Players.PlayerTwo);
			} else {
				IncreasePlayerScore (Players.PlayerOne);
			}
		}
		// Set pocket to put black
		if (scores[currentPlayer] == 7) {
			if (currentPlayer == Players.PlayerOne) {
				lastPlayerOnePocket = pocketId;
			} else {
				lastPlayerTwoPocket = pocketId;
			}
		}
		// Restart movement
		if (ball.type == ballTypes[currentPlayer]) {
			currentPlayerMoves++;
		}
		// Remove
		BallManager.Instance.RemoveBall (ball);
		Destroy (ball.gameObject);
	}

	private void SwitchPlayer() {
		if (currentPlayer == Players.PlayerOne) {
			currentPlayer = Players.PlayerTwo;
		} else {
			currentPlayer = Players.PlayerOne;
		}
	}

	private static Dictionary<Players, int> InitializeScores() {
		Dictionary<Players, int> result = new Dictionary<Players, int> ();
		result.Add (Players.PlayerOne, 0);
		result.Add (Players.PlayerTwo, 0);
		return result;
	}

	private static Dictionary<Players, Ball.BallTypes> InitializeBallTypes() {
		Dictionary<Players, Ball.BallTypes> result = new Dictionary<Players, Ball.BallTypes> ();
		result.Add (Players.PlayerOne, Ball.BallTypes.None);
		result.Add (Players.PlayerTwo, Ball.BallTypes.None);
		return result;
	}

	private string PlayerText(Players player) {
		string ans = "";
		ans += PlayerString (player);
		ans += " | Score " + scores[player].ToString();
		ans += " | Ball Type ";
		ans += ballTypes [player].ToString();
		if (currentPlayer == player) {
			ans += " | Movements " + currentPlayerMoves.ToString();
		}
		return ans;
	}

	public void ReduceMovements() {
		currentPlayerMoves--;
	}

	private string PlayerString(Players player) {
		if (player == Players.PlayerOne) {
			return "Player One";
		} else {
			return "Player Two";
		}
	}

	private void IncreasePlayerScore(Players player) {
		scores [player] = scores [player] + 1;
	}
}
