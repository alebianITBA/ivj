using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviourSingleton<StateManager> {
	// Enums
	public enum States { Menu, Striking, InGame, WaitingForNextTurn, Pause, Finished };
	public enum GameModes { OnePlayer, TwoPlayers };
	// Constants
	private static Color ACTIVE_COLOR = new Color32( 0xFF, 0xE2, 0x59, 0xFF );
	private static Color INACTIVE_COLOR = new Color32( 0xE0, 0x83, 0x28, 0xFF );
	// Game logic
	// TODO: Make the state start in Menu
	private Player[] Players;
	public int CurrentPlayerIdx;
	public States currentState = States.InGame;
	public GameModes currentGameMode = GameModes.TwoPlayers;
	public CueManager cue;
	public Player winner;
	private bool whiteInPocket = false;
	private WhiteBall white;
	private bool firstBall = false;
	private bool setInGame = false;
	private int updateCounts = 0;
	private float currentPlayerEnergy = 0.0f;
	private static float ENERGY = 100.0f;
	private static float MAX_ENERGY = 30000.0f;
	// Game variables
	public GameObject pauseCanvas;
	public GameObject finishedCanvas;
	public GameObject energyBar;
	public Text playerOneText;
	public Text playerTwoText;
	public Text WinnerText;

	void Start() {
		this.Players = new Player[2];
		this.Players[0] = new Player ("One");
		this.Players[1] = new Player ("Two");
		this.white = BallManager.Instance.whiteBall	.GetComponent<WhiteBall>();
		CurrentPlayer ().SetMovements (1);
	}

	void Update() {
		Debug.Log (currentState);
		if (currentState != States.Menu) {
			if (currentState != States.Finished) {
				finishedCanvas.SetActive (false);
				// Set Texts
				playerOneText.gameObject.SetActive (true);
				playerOneText.text = PlayerOne().DescriptionText(PlayerOnePlaying());
				if (currentGameMode == GameModes.TwoPlayers) {
					playerTwoText.gameObject.SetActive (true);
					playerTwoText.text = PlayerTwo().DescriptionText(PlayerTwoPlaying());
				}
				// Bold
				if (PlayerOnePlaying()) {
					playerOneText.fontStyle = FontStyle.Bold;
					playerTwoText.fontStyle = FontStyle.Normal;
					playerOneText.fontSize = 16;
					playerTwoText.fontSize = 14;
					playerOneText.color = ACTIVE_COLOR;
					playerTwoText.color = INACTIVE_COLOR;
				} else if (PlayerTwoPlaying()) {
					playerOneText.fontStyle = FontStyle.Normal;
					playerTwoText.fontStyle = FontStyle.Bold;
					playerOneText.fontSize = 14;
					playerTwoText.fontSize = 16;
					playerOneText.color = INACTIVE_COLOR;
					playerTwoText.color = ACTIVE_COLOR;
				}
				// Change player

			} else {
				finishedCanvas.SetActive (true);
				WinnerText.text = "Winner " + CurrentPlayer().ToString() + "!";
			}
		} else {
			playerOneText.gameObject.SetActive (false);
			playerTwoText.gameObject.SetActive (false);
		}
	}

	void LateUpdate() {
		if (this.updateCounts > 0)
			this.updateCounts--;
		switch (currentState) {
			case States.InGame:
			if (this.updateCounts<= 0 && BallManager.Instance.Still ()) {
					this.currentState = States.WaitingForNextTurn;
				}
				break;
		case States.WaitingForNextTurn:
			bool penalize = false;
			if (this.whiteInPocket) {
				this.white.transform.position = BallManager.Instance.DefaultWhitePosition ();
				this.whiteInPocket = false;
				penalize = true;
				this.whiteInPocket = false;
			}

			Ball.BallTypes type = white.GetFirstCollided ();
			white.ResetFirstCollided ();

			penalize = penalize || CurrentPlayer ().checkFirstCollided (type, firstBall);
			if (firstBall)
				firstBall = false;
				if (penalize) {
					SwitchPlayer (penalize);
					return;
				}

				if (CurrentPlayer ().Movements <= 0) {
					SwitchPlayer (penalize);
				}

				this.readyToStrike ();
				break;
			case States.Striking:
				if (Input.GetKey (KeyCode.Space)) {
					energyBar.SetActive (true);
					CueManager.Instance.speed = 1.0f;
					currentPlayerEnergy += ENERGY;
					if (currentPlayerEnergy > MAX_ENERGY) {
						currentPlayerEnergy = 0.0f;
					}
					energyBar.GetComponent<GUIBarScript>().SetNewValue(currentPlayerEnergy / MAX_ENERGY);
				}
				if (Input.GetKeyUp (KeyCode.Space)) {
					BasicSoundManager.Instance.PlayCueHitSound ();
					energyBar.SetActive (false);
					CueManager.Instance.speed = 0.0f;
					white.GetComponent<Rigidbody> ().AddForce (direction () * currentPlayerEnergy);
					currentPlayerEnergy = 0.0f;
					this.ReduceMovements ();
				this.updateCounts = 5;
					this.strike();
					//StateManager.Instance.strike ();
				}
				break;
			case States.Pause:
				if (Input.GetKeyDown (KeyCode.Escape)) {
					this.ContinueGame ();
				}
				break;
			case States.Menu:
				break;
			default:
				break;
			}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.PauseGame ();
		}
	}
	private Vector3 direction()
	{
		Vector3 forward = CameraManager.Instance.camera.transform.forward;
		Vector3 direction = new Vector3 (forward.x, 0.0f, forward.z);
		return direction;
	}

	public void PauseGame() {
		if (currentState == States.InGame || currentState == States.Striking) {
			currentState = States.Pause;
			pauseCanvas.SetActive(true);
		} else {
			LogInvalidTransition (States.Pause);
		}
	}

	public void readyToStrike() {
		if (currentState == States.WaitingForNextTurn) {
			cue.ResetCue ();
			cue.SetVisible(true);
			currentState = States.Striking;
		} else {
			LogInvalidTransition (States.Striking);
		}
	}

	public void strike() {
		if (currentState == States.Striking) {
			cue.SetVisible (false);
			currentState = States.InGame;
		}
		else {
			LogInvalidTransition (States.InGame);
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
			CurrentPlayerIdx = 0;
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
				CurrentPlayerIdx = 0;
			} else {
				CurrentPlayerIdx = 1;
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
		this.white = (WhiteBall) white;
	}

	private void BlackInPocket(Ball black, PocketCollider.Pocket pocketId) {
		// Win logic
		if (PlayerOnePlaying() && PlayerOne().LastPocket == pocketId) {
			winner = PlayerOne();
			currentState = States.Finished;
		} else if (PlayerTwoPlaying() && PlayerTwo().LastPocket == pocketId) {
			winner = PlayerTwo();
			currentState = States.Finished;
		}
		// Lose logic
		if (currentState != States.Finished) {
			currentState = States.Finished;
			if (PlayerOnePlaying()) {
				winner = PlayerTwo();
			} else {
				winner = PlayerOne();
			}
		}
		BallManager.Instance.RemoveBall (black);
		Destroy (black.gameObject);
	}

	private void BallInPocket(Ball ball, PocketCollider.Pocket pocketId) {
		// Set ball type to players
		if (PlayerOne ().BallType == Ball.BallTypes.None) {
			this.firstBall = true;
			if (PlayerOnePlaying ()) {
				PlayerOne ().SetBallType (ball.type);
				PlayerTwo ().SetBallType (Ball.Opposite (ball.type));
			} else {
				PlayerTwo ().SetBallType (ball.type);
				PlayerOne ().SetBallType (Ball.Opposite (ball.type));
			}
		}
		// Sum score
		PlayerOne().IncreaseScore(ball.type);
		PlayerTwo().IncreaseScore(ball.type);
		// Set pocket to put black
		if (CurrentPlayer().Score == 7) {
			CurrentPlayer ().SetLastPocket (pocketId);
		}
		// Restart movement
		if (ball.type == CurrentPlayer().BallType) {
			CurrentPlayer ().SetMovements(1);
		}
		// Remove
		BallManager.Instance.RemoveBall (ball);
		Destroy (ball.gameObject);
	}

	private Player CurrentPlayer() {
		return Players[CurrentPlayerIdx];
	}

	private Player PlayerOne() {
		return Players[0];
	}

	private Player PlayerTwo() {
		return Players[1];
	}

	public bool PlayerOnePlaying() {
		return CurrentPlayer ().Equals (PlayerOne ());
	}

	public bool PlayerTwoPlaying() {
		return CurrentPlayer ().Equals (PlayerTwo ());
	}

	private void SwitchPlayer(bool penalize) {
		CurrentPlayer ().SetMovements (0);
		CurrentPlayerIdx = OtherPlayerIndex();
		int movements = 1;
		if (penalize)
			movements = 2;
		CurrentPlayer ().SetMovements (movements);
		this.readyToStrike ();
	}

	// Returns the other player that is not the currentPlayer
	private Player Rival() {
		return Players[OtherPlayerIndex()];
	}

	private int OtherPlayerIndex() {
		return CurrentPlayerIdx == 1 ? 0 : 1;
	}

	public void ReduceMovements() {
		CurrentPlayer ().DecreaseMovements ();
	}

	public void PutBallBackInTable(GameObject ballGo) {
		Ball ball = ballGo.GetComponent<Ball>();

		switch (ball.type) {
		case Ball.BallTypes.White:
			ball.transform.position = BallManager.Instance.DefaultWhitePosition ();
			whiteInPocket = false;
			break;
		default:
			ball.transform.position = BallManager.Instance.DefaultBlackPosition ();
			break;
		}
	}
}
