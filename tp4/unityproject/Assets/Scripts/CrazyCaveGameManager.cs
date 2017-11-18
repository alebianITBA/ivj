using UnityEngine;

public class CrazyCaveGameManager : MonoBehaviourSingleton<CrazyCaveGameManager> {
	public GameObject mainCamera;
	[HideInInspector]
	public GameObject player;

	void Start() {
		SoundManager.PlayBackground ((int)SndIdGame.BACKGROUND_MUSIC);

		CrazyCaveLevelManager.Instance.CreateNewLevel ();
		player = Drawer.Instance.CreatePlayer (CrazyCaveLevelManager.Instance.level.PlayerStartingPoint());
	}

	void Update() {
		CameraFollowPlayer ();
		CheckPlayerOutsideLevel ();
		Drawer.Instance.DrawMinimap (CrazyCaveLevelManager.Instance.level, player);
	}

	private void CameraFollowPlayer() {
		if (player != null) {
			mainCamera.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -100);
		}
	}

	private void CheckPlayerOutsideLevel() {
		Vector3 playerPosition = player.transform.position;
		int direction = -1;

		if (playerPosition.x > Drawer.Instance.MaxVisibleX(CrazyCaveLevelManager.Instance.level)) {
			direction = (int)Level.Direction.West;
		}
		if (playerPosition.x < 0) {
			direction = (int)Level.Direction.East;
		}
		if (playerPosition.y > Drawer.Instance.MaxVisibleY(CrazyCaveLevelManager.Instance.level)) {
			direction = (int)Level.Direction.South;
		}
		if (playerPosition.y < 0) {
			direction = (int)Level.Direction.North;
		}
			
		if (direction != -1) {
			// We got out of the level so we create a new one
			CrazyCaveLevelManager.Instance.CreateNewLevel ();
			Drawer.Instance.RepositionPlayer (player, (Level.Direction)direction, CrazyCaveLevelManager.Instance.level);
		}
	}
}

