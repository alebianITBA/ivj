using UnityEngine;

public class CrazyCaveGameManager : MonoBehaviourSingleton<CrazyCaveGameManager> {
	public GameObject mainCamera;
	[HideInInspector]
	public GameObject player;

	void Start() {
		SoundManager.PlayBackground ((int)SndIdGame.BACKGROUND_MUSIC);

		//CrazyCaveLevelManager.Instance.CreateNewLevel (Level.Direction.Center);
		player = Drawer.Instance.CreatePlayer (CrazyCaveLevelManager.Instance.GetLevel().PlayerStartingPoint());
	}

	void Update() {
		CameraFollowPlayer ();
		CheckPlayerOutsideLevel ();
		Drawer.Instance.DrawMinimap (CrazyCaveLevelManager.Instance.GetLevel(), player);
	}

	private void CameraFollowPlayer() {
		if (player != null) {
			mainCamera.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -100);
		}
	}

	private void CheckPlayerOutsideLevel() {
		Vector3 playerPosition = player.transform.localPosition;
		Level.Direction direction = (Level.Direction) (-1);

		if (playerPosition.x > Drawer.Instance.MaxVisibleX(CrazyCaveLevelManager.Instance.GetLevel())) {
			direction = Level.Direction.West;
		}
		if (playerPosition.x < 0) {
			direction = Level.Direction.East;
		}
		if (playerPosition.y > Drawer.Instance.MaxVisibleY(CrazyCaveLevelManager.Instance.GetLevel())) {
			direction = Level.Direction.South;
		}
		if (playerPosition.y < 0) {
			direction = Level.Direction.North;
		}
			
		if (direction != (Level.Direction) (-1)) {
			// We got out of the level so we create a new one
			CrazyCaveLevelManager.Instance.CreateNewLevel (direction);
			Drawer.Instance.RepositionPlayer (player, (Level.Direction)direction, CrazyCaveLevelManager.Instance.GetLevel(), CrazyCaveLevelManager.Instance.GetHolder());
		}
	}
}

