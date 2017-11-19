using UnityEngine;

public class CrazyCaveLevelManager : MonoBehaviourSingleton<CrazyCaveLevelManager> {
	[HideInInspector]
	public Level level;
	GameObject boardHolder = null;

	public int levelXSize;
	public int levelYSize;
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
		}
		this.boardHolder = new GameObject ("BoardHolder");

		this.level = CreateLevel (seed);
		FillOuterWalls ();
		Drawer.Instance.DrawTiles (level, boardHolder);
	}

	public Level GetLevel() {
		return this.level;
	}

	private Level CreateLevel(int seed) {
		return new AutomataLevel (levelXSize, levelYSize, automataInitialRounds, automataAfterRounds, automataInitialBirthChance, 
			automataInitialDeathChance, automataAfterBirthChance, automataAfterDeathChance, automataInitialWallChance, seed);
	}

	private void FillOuterWalls() {
		// TODO
	}
}
