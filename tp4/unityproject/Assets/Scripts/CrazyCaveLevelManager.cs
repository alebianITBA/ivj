using UnityEngine;
using System.Collections.Generic;
using System;

public class CrazyCaveLevelManager : MonoBehaviourSingleton<CrazyCaveLevelManager> {
	[HideInInspector]
	//public Level level;
	Dictionary<Level.Direction, GameObject> boardHolders;
	Dictionary<Level.Direction, Level> levels;
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

	private int initialSeed = 0;
	private int seed;

	void Awake() {
		this.seed = initialSeed;
		boardHolders = new Dictionary<Level.Direction, GameObject>();
		levels = new Dictionary<Level.Direction, Level> ();
		boardHolders.Add (Level.Direction.Center, new GameObject ("Center BoardHolder"));
		boardHolders [Level.Direction.Center].transform.position = Level.GetPosition(Level.Direction.Center);
		levels.Add (Level.Direction.Center, CreateLevel (seed));

		boardHolders.Add(Level.Direction.NorthEast, new GameObject("NorthEast BoardHolder"));
		boardHolders.Add(Level.Direction.North, new GameObject("North BoardHolder"));
		boardHolders.Add(Level.Direction.NorthWest, new GameObject("NorthWest BoardHolder"));
		boardHolders.Add(Level.Direction.SouthEast, new GameObject("SouthEast BoardHolder"));
		boardHolders.Add(Level.Direction.South, new GameObject("South BoardHolder"));
		boardHolders.Add(Level.Direction.SouthWest, new GameObject("SouthWest BoardHolder"));
		boardHolders.Add(Level.Direction.East, new GameObject("East BoardHolder"));
		boardHolders.Add(Level.Direction.West, new GameObject("West BoardHolder"));

		this.accessoriesHolder = new GameObject ("AccessoriesHolder");

	}

	void Start() {
		foreach (Level.Direction dir in Enum.GetValues(typeof(Level.Direction))) {
			if (!levels.ContainsKey (dir)) {
				levels.Add (dir, CreateLevel (seed + Level.GetSeedOffset (dir)));
				boardHolders [dir].transform.position = Vector3.Scale (Level.GetPosition (dir), new Vector3 (levelXSize * Drawer.Instance.tileLength, levelYSize * Drawer.Instance.tileLength, 0));
			}
			AddAccessories (levels[dir]);
			Drawer.Instance.DrawTiles (levels[dir], boardHolders[dir], accessoriesHolder);
		}

	}
	public void CreateNewLevel(Level.Direction dir) {
		Dictionary<Level.Direction, GameObject> newBoards = new Dictionary<Level.Direction, GameObject>();
		foreach (Level.Direction d in boardHolders.Keys) {
			newBoards.Add (d, boardHolders [d]);
		}
		Dictionary<Level.Direction, Level> newLevels = new Dictionary<Level.Direction, Level>();
		foreach (Level.Direction d in levels.Keys) {
			newLevels.Add (d, levels [d]);
		}
		Vector3 pos = boardHolders[dir].transform.position;
		GameObject newHolder;
		seed += Level.GetSeedOffset (dir);
		GameObject old = boardHolders [Level.GetOpposite(dir)];
		boardHolders [Level.Direction.Center].name = newBoards [Level.GetOpposite (dir)].name;
		newBoards [Level.GetOpposite(dir)] = boardHolders [Level.Direction.Center];
		newLevels [Level.GetOpposite(dir)] = levels [Level.Direction.Center];
		Destroy (old);

		boardHolders [dir].name = newBoards [Level.Direction.Center].name;
		newBoards[Level.Direction.Center] = boardHolders[dir];
		newLevels [Level.Direction.Center] = levels [dir];
		newHolder = new GameObject (boardHolders[dir].name);
		newHolder.transform.position = pos + Vector3.Scale (Level.GetPosition (dir), new Vector3 (levelXSize * Drawer.Instance.tileLength, levelYSize * Drawer.Instance.tileLength, 0));
		newBoards[dir] =  newHolder;
		newLevels[dir] =  CreateLevel (seed + Level.GetSeedOffset(dir));
		AddAccessories (newLevels [dir]);
		newLevels[dir].AddZombieSpawningPoints(10,new LevelPosition(levelXSize/2, levelYSize/2));
		Drawer.Instance.DrawTiles (newLevels[dir], newBoards[dir],accessoriesHolder);

		foreach (Level.Direction neigh in Level.GetNeighbours(dir)) {
			old = boardHolders [Level.GetOpposite (dir, neigh)];
			boardHolders [Level.GetReplacement (dir, neigh)].name = newBoards [Level.GetOpposite (dir, neigh)].name;
			newBoards [Level.GetOpposite (dir, neigh)] = boardHolders [Level.GetReplacement (dir, neigh)];
			newLevels [Level.GetOpposite (dir, neigh)] = levels [Level.GetReplacement (dir, neigh)];
			Destroy (old);
			boardHolders [neigh].name = newBoards [Level.GetReplacement (dir, neigh)].name;
			newBoards [Level.GetReplacement (dir, neigh)] = boardHolders [neigh];
			newLevels [Level.GetReplacement (dir, neigh)] = levels [neigh];
			newHolder = new GameObject (boardHolders[neigh].name);
			newHolder.transform.position = pos + Vector3.Scale (Level.GetPosition (neigh), new Vector3 (levelXSize * Drawer.Instance.tileLength, levelYSize * Drawer.Instance.tileLength, 0));
			newBoards[neigh] =  newHolder;
			newLevels[neigh] =  CreateLevel (seed + Level.GetSeedOffset(neigh));
			AddAccessories (newLevels [neigh]);
			newLevels[neigh].AddZombieSpawningPoints(10,new LevelPosition(levelXSize/2, levelYSize/2));
			Drawer.Instance.DrawTiles (newLevels[neigh], newBoards[neigh], accessoriesHolder);
		
		}

		boardHolders = newBoards;
		levels = newLevels;


	} 

	public Level GetLevel() {
		return this.levels[Level.Direction.Center];
	}

	public GameObject GetHolder() {
		return boardHolders[Level.Direction.Center];
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

	private void AddAccessories(Level level) {
		level.AddAmmo (bulletSpawningPoints);
		level.AddHealthKit ();
		level.AddSpecialBox ();
	}
}
