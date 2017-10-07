using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviourSingleton<BallManager> {

	public GameObject table;
	public GameObject ballPrefab;
	public GameObject whiteBall;

	public static float BALL_SIZE = 1.0f;
	private static float LEVEL_DISTANCE = Mathf.Sqrt(BALL_SIZE - Mathf.Pow(BALL_SIZE / 2, 2));

	public Texture2D whiteTexture;
	public Texture2D ball1Texture;
	public Texture2D ball2Texture;
	public Texture2D ball3Texture;
	public Texture2D ball4Texture;
	public Texture2D ball5Texture;
	public Texture2D ball6Texture;
	public Texture2D ball7Texture;
	public Texture2D ball8Texture;
	public Texture2D ball9Texture;
	public Texture2D ball10Texture;
	public Texture2D ball11Texture;
	public Texture2D ball12Texture;
	public Texture2D ball13Texture;
	public Texture2D ball14Texture;
	public Texture2D ball15Texture;

	public List<Ball> Balls = new List<Ball>();

	// Use this for initialization
	void Awake () {
		CreateBalls ();
	}

	private void CreateBalls() {
		Ball white = InstantiateBall ("white", Ball.BallTypes.White, DefaultWhitePosition(), whiteTexture);
		Balls.Add(white);
		whiteBall = white.gameObject;

		Ball black = InstantiateBall ("black", Ball.BallTypes.Black, new Vector3 (table.transform.position.x, TableYDistance(), TableZDistance()), ball8Texture);
		Balls.Add (black);

		// Add rest of balls relative to black's position
		Vector3 blackPosition = black.gameObject.transform.position;

		Ball ball6 = InstantiateBall("ball6", Ball.BallTypes.Solid, new Vector3(blackPosition.x - BALL_SIZE, blackPosition.y, blackPosition.z), ball6Texture);
		Balls.Add (ball6);

		Ball ball14 = InstantiateBall("ball14", Ball.BallTypes.Striped, new Vector3(blackPosition.x + BALL_SIZE, blackPosition.y, blackPosition.z), ball14Texture);
		Balls.Add (ball14);

		Ball ball11 = InstantiateBall("ball11", Ball.BallTypes.Striped, new Vector3(blackPosition.x - BALL_SIZE / 2, blackPosition.y, blackPosition.z - LEVEL_DISTANCE), ball11Texture);
		Balls.Add (ball11);

		Ball ball3 = InstantiateBall("ball3", Ball.BallTypes.Solid, new Vector3(blackPosition.x + BALL_SIZE / 2, blackPosition.y, blackPosition.z - LEVEL_DISTANCE), ball3Texture);
		Balls.Add (ball3);

		Ball ball1 = InstantiateBall("ball1", Ball.BallTypes.Solid, new Vector3(blackPosition.x, blackPosition.y, blackPosition.z - LEVEL_DISTANCE * 2), ball1Texture);
		Balls.Add (ball1);

		Ball ball15 = InstantiateBall("ball15", Ball.BallTypes.Striped, new Vector3(blackPosition.x - BALL_SIZE / 2, blackPosition.y, blackPosition.z + LEVEL_DISTANCE), ball15Texture);
		Balls.Add (ball15);

		Ball ball4 = InstantiateBall("ball4", Ball.BallTypes.Solid, new Vector3(blackPosition.x + BALL_SIZE / 2, blackPosition.y, blackPosition.z + LEVEL_DISTANCE), ball4Texture);
		Balls.Add (ball4);

		Ball ball13 = InstantiateBall("ball13", Ball.BallTypes.Striped, new Vector3(blackPosition.x - 1.5f * BALL_SIZE, blackPosition.y, blackPosition.z + LEVEL_DISTANCE), ball13Texture);
		Balls.Add (ball13);

		Ball ball9 = InstantiateBall("ball9", Ball.BallTypes.Striped, new Vector3(blackPosition.x + 1.5f * BALL_SIZE, blackPosition.y, blackPosition.z + LEVEL_DISTANCE), ball9Texture);
		Balls.Add (ball9);

		Ball ball7 = InstantiateBall("ball7", Ball.BallTypes.Solid, new Vector3(blackPosition.x - 2.0f * BALL_SIZE, blackPosition.y, blackPosition.z + 2.0f * LEVEL_DISTANCE), ball7Texture);
		Balls.Add (ball7);

		Ball ball10 = InstantiateBall("ball10", Ball.BallTypes.Striped, new Vector3(blackPosition.x - BALL_SIZE, blackPosition.y, blackPosition.z + 2.0f * LEVEL_DISTANCE), ball10Texture);
		Balls.Add (ball10);

		Ball ball2 = InstantiateBall("ball2", Ball.BallTypes.Solid, new Vector3(blackPosition.x, blackPosition.y, blackPosition.z + 2.0f * LEVEL_DISTANCE), ball2Texture);
		Balls.Add (ball2);

		Ball ball5 = InstantiateBall("ball5", Ball.BallTypes.Solid, new Vector3(blackPosition.x + BALL_SIZE, blackPosition.y, blackPosition.z + 2.0f * LEVEL_DISTANCE), ball5Texture);
		Balls.Add (ball5);

		Ball ball12 = InstantiateBall("ball12", Ball.BallTypes.Striped, new Vector3(blackPosition.x + 2.0f * BALL_SIZE, blackPosition.y, blackPosition.z + 2.0f * LEVEL_DISTANCE), ball12Texture);
		Balls.Add (ball12);
	}

	private Ball InstantiateBall(string id, Ball.BallTypes type, Vector3 position, Texture2D texture) {
		GameObject go = GameObject.Instantiate(ballPrefab) as GameObject;
		Ball ball = go.GetComponent<Ball>();

		ball.id = id;
		ball.type = type;
		ball.transform.position = position;

		ball.GetComponent<Renderer> ().material.mainTexture = texture;
		// So it shows the textures' number up
		ball.transform.Rotate (0, 90, 90);

		return ball;
	}

	private float TableZDistance() {
		return table.GetComponent<Renderer> ().bounds.size.z * 0.30f;
	}

	private float TableYDistance() {
		return table.GetComponent<Renderer> ().bounds.size.y;
	}

	public Vector3 DefaultWhitePosition() {
		return new Vector3 (table.transform.position.x, TableYDistance (), -1 * TableZDistance ());
	}

	public bool Still() {
		foreach (Ball b in Balls) {
			Rigidbody rb = b.GetComponentInChildren<Rigidbody> ();
			if (!(rb.IsSleeping () || rb.velocity.Equals (Vector3.zero))) {
				return false;
			}
		}
		return true;
	}
}
