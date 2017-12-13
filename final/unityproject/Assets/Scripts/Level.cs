using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {
	public enum Tile { Wall, Floor, Home, BaseTower, MinionSpawn, Tower };

	private Tile[,] map;

	public Level(TextAsset map) {
		CreateMap (map);
	}

	public Tile[,] GetMap() {
		return this.map;
	}

	private void CreateMap(TextAsset textFile) {
		string text = textFile.text;
		string[] splitted = text.Split (new string[]{ System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
		this.map = new Tile[splitted.Length, splitted[0].Length];

		for (int y = 0; y < splitted.Length - 1; y++) {
			for (int x = 0; x < splitted[0].Length - 1; x++) {
				char c = splitted [y] [x];
				Tile t;
				switch (c) {
				case '#':
					t = Tile.Wall;
					break;
				case '.':
					t = Tile.Floor;
					break;
				case 'B':
					t = Tile.BaseTower;
					break;
				case 'H':
					t = Tile.Home;
					break;
				case 'M':
					t = Tile.MinionSpawn;
					break;
				default:
					t = Tile.Floor;
					break;
				}
				map [y, x] = t;
			}
		}
	}
}
