using UnityEngine;

public class CrazyCaveLevelManager : MonoBehaviourSingleton<CrazyCaveLevelManager> {
	[HideInInspector]
	public Level level;
	[HideInInspector]
	GameObject boardHolder = null;
	[HideInInspector]
	GameObject accessoriesHolder = null;

	public int levelXSize;
	public int levelYSize;
	public int zombieSpawningPoints = GameLogic.DEFAULT_ZOMBIE_SPAWNING_PONTS;
	public int bulletSpawningPoints = GameLogic.DEFAULT_AMMO_SPAWNING_PONTS;
	// Automata generator attributes
	public int automataInitialRounds;
	public int automataAfterRounds;
	public int automataInitialBirthChance;
	public int automataInitialDeathChance;
	public int automataAfterBirthChance;
	public int automataAfterDeathChance;
	public float automataInitialWallChance;

	public void CreateNewLevel(int seed) {
		if (this.boardHolder != null) {
			// There was an existing level so we destroy it
			Destroy(boardHolder);
			Destroy(accessoriesHolder);
		}
		this.boardHolder = new GameObject ("BoardHolder");
		this.accessoriesHolder = new GameObject ("AccessoriesHolder");

		this.level = CreateLevel (seed);
		FillOuterWalls ();
		AddAmmo ();
		Drawer.Instance.DrawTiles (level, boardHolder, accessoriesHolder);
	}

	public Level GetLevel() {
		return this.level;
	}

	public void AddZombieSpawningPoints(GameObject player) {
		CrazyCaveLevelManager.Instance.GetLevel ().AddZombieSpawningPoints (zombieSpawningPoints, Drawer.Instance.GetLevelPosition (player));
	}

	private Level CreateLevel(int seed) {
		return new AutomataLevel (levelXSize, levelYSize, automataInitialRounds, automataAfterRounds, automataInitialBirthChance, 
			automataInitialDeathChance, automataAfterBirthChance, automataAfterDeathChance, automataInitialWallChance, seed);
	}

	private void FillOuterWalls() {
		// TODO
	}

	private void AddAmmo() {
		GetLevel ().AddAmmo (bulletSpawningPoints);
	}
}
