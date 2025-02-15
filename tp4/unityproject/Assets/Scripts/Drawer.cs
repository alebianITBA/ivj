﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : MonoBehaviourSingleton<Drawer> {
	// COLORS
	public static Color YELLOW = new Color(1f, 1f, 0f);
	public static Color RED = new Color(1f, 0f, 0f);
	public static Color DARK_RED = new Color(0.4f, 0f, 0f);
	public static Color GREEN = new Color(0f, 1f, 0f);
	public static Color DARK_GREEN = new Color(0f, 0.4f, 0f);
	public static Color BLUE = new Color(0f, 0f, 1f);
	public static Color DARK_BLUE = new Color(0f, 0f, 0.4f);
	public static Color BLACK = new Color(0f, 0f, 0f);
	public static Color WHITE = new Color(1f, 1f, 1f);
	public static Color PURPLE = new Color(1f, 0f, 1f);
	private static Color MM_BORDER_COLOR = YELLOW;
	private static Color MM_WALL_COLOR = WHITE;
	private static Color MM_OUTER_WALL_COLOR = WHITE;
	private static Color MM_FLOOR_COLOR = BLACK;
	private static Color MM_PLAYER_COLOR = GREEN;
    private static Color MM_WARRIOR_COLOR = RED;
    private static Color MM_WARRIOR_SPAWN_COLOR = DARK_RED;
	private static Color MM_AMMO_SPAWN_COLOR = DARK_BLUE;
	private static Color MM_HEALTH_SPAWN_COLOR = DARK_GREEN;
	private static Color MM_SPECIAL_BOX_SPAWN_COLOR = PURPLE;
    private static Color TRANSPARENT_RED = new Color (1, 0, 0, 0);
    private static Color OPAQUE_RED = new Color (1, 0, 0, MAX_ALPHA);
    public static Color TRANSPARENT_WHITE = new Color (1, 1, 1, MAX_ALPHA);
	
    // Prefabs
	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;
	public GameObject playerPrefab;
	public GameObject spawnPrefab;
	public GameObject minimap;
	public GameObject ammoPrefab;
	public GameObject healthKitPrefab;
	public GameObject specialBoxPrefab;
	public GameObject actionText;

	private GameObject player;

	[HideInInspector]
	public float tileLength;
	private float halfTileLength;

	public Text scoreText;
	public Text lifeText;
	public Text bulletsText;
    public Text middleText;
    // DAMAGE
    public GameObject damagePanel;
    private static int DAMAGE_TTL = 500;
    private static int DELTA_LERP = 10;
    private static float MAX_ALPHA = 0.5f;
    private bool takingDamage = false;
    private System.DateTime damageStarted;
    private Color previousPauseColor;

	private List<TimeDestroyable> destroyables;

	override protected void Initialize() {
		this.destroyables = new List<TimeDestroyable> ();
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		halfTileLength = tileLength / 2f;
        damagePanel.GetComponent<Image> ().color = TRANSPARENT_RED;
	}

	void Update() {
        Character character = null;
        if (player != null) {
            character = player.GetComponent<Character> ();
        }

        if (character != null) {
			scoreText.text = "Score: " + character.score.ToString();
            lifeText.text = "Life: " + character.health.ToString();
            bulletsText.text = "Bullets: " + character.bullets.ToString() + "/" + GameLogic.MAX_AMMO.ToString();
		}
		for (int i = destroyables.Count - 1; i >= 0; i--) {
			if (destroyables [i].Destroyable ()) {
				Destroy (destroyables [i].obj);
				destroyables.RemoveAt (i);
			}
		}
        if (character != null) {
            if (character.health <= 0) {
                ShowMiddleText ("YOU LOST\nPRESS ESC TO CONTINUE", OPAQUE_RED);
            } else {
                // Damage panel
                if (takingDamage) {
                    if (damagePanel.GetComponent<Image> ().color.a < MAX_ALPHA) {
                        damagePanel.GetComponent<Image> ().color = Color.Lerp (damagePanel.GetComponent<Image> ().color, RED, DELTA_LERP * Time.deltaTime);
                    }

                    if ((System.DateTime.Now - damageStarted).TotalMilliseconds > DAMAGE_TTL) {
                        takingDamage = false;
                    }
                } else {
                    damagePanel.GetComponent<Image> ().color = Color.Lerp (damagePanel.GetComponent<Image> ().color, TRANSPARENT_RED, DELTA_LERP * Time.deltaTime);
                }
            }
        }
	}

	public void DrawTiles(Level level, GameObject tilesHolder, GameObject accessoriesHolder) {
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		for (int row = 0; row < level.GetMap().GetLength(0); row++) {
			for (int col = 0; col < level.GetMap().GetLength(1); col++) {
				GameObject tileInstance = null;
				Vector3 position = new Vector3 (row * tileLength, col * tileLength, 10);

				switch (level.GetMap() [row, col]) {
					case Level.Tile.Floor:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.PlayerSpawn:
						// TODO: Make a different floor sprite
						tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.ZombieSpawn:
						// TODO: Make a different floor sprite
						tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), tilesHolder.transform);
						CreateAccessory (spawnPrefab, tilesHolder.transform, position);
						break;
					case Level.Tile.Wall:
						tileInstance = NewObjectFromPrefab (RandomTile (wallPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.OuterWall:
						tileInstance = NewObjectFromPrefab (RandomTile (outerWallPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.AmmoSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject ammo = CreateAccessory (ammoPrefab, tilesHolder.transform, position);
						level.AddAmmo (ammo);
						break;
					case Level.Tile.HealthKitSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject kit = CreateAccessory (healthKitPrefab, tilesHolder.transform, position);
						level.AddHealthKit (kit);
						break;
					case Level.Tile.SpecialBoxSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject box = CreateAccessory (specialBoxPrefab, tilesHolder.transform, position);
						level.AddSpecialBox (box);
                        break;
				}

				if (tileInstance != null) {
					//tileInstance.transform.parent = holder.transform;
					tileInstance.transform.localPosition = position;

                    // To detect objects inside level
                    if (row == 0 && col == 0) {
                        level.renderedMinX = position.x;
                        level.renderedMinY = position.y;
                    } else if (row == level.GetMap().GetLength(0) - 1 && col == level.GetMap().GetLength(1) - 1) {
                        level.renderedMaxX = position.x + tileLength;
                        level.renderedMaxY = position.y + tileLength;
                    }
				}
			}
		}
	}

	public GameObject CreatePlayer(LevelPosition position) {
		player = NewObjectFromPrefab (playerPrefab, GetPosition(position));
		return player;
	}

	public void RepositionPlayer(GameObject player, Level.Direction direction, Level level, GameObject holder) {
		player.transform.parent = holder.transform;
	}

	public Vector3 GetPosition(LevelPosition position) {
		return new Vector3 ((position.x * tileLength) + halfTileLength, (position.y * tileLength) + halfTileLength, 0);
	}

	public float MaxVisibleX(Level level) {
		return level.GetMap ().GetLength (0) * tileLength;
	}

	public float MaxVisibleY(Level level) {
		return level.GetMap ().GetLength (1) * tileLength;
	}

	public LevelPosition GetLevelPosition(GameObject obj, Level level) {
        Vector3 objPosition = obj.transform.localPosition;

        if (objPosition.x >= level.renderedMinX && objPosition.x <= level.renderedMaxX)
        {
            if (objPosition.y >= level.renderedMinY && objPosition.y <= level.renderedMaxY)
            {
                float maxX = tileLength * level.GetMap ().GetLength (0);
                float maxY = tileLength * level.GetMap ().GetLength (1);
                float x = objPosition.x % maxX;
                float y = objPosition.y % maxY;

                if (x < 0) {
                    x = x + maxX;
                }
                if (y < 0) {
                    y = y + maxY;
                }

                return new LevelPosition (Mathf.FloorToInt (x / tileLength), Mathf.FloorToInt (y / tileLength));
            }
        }
        return new LevelPosition (-1, -1);
	}

	private GameObject NewObjectFromPrefab(GameObject prefab, Vector3 position) {
		return Instantiate (prefab, position, Quaternion.identity) as GameObject;
	}

	private GameObject NewObjectFromPrefab(GameObject prefab, Transform parent) {
		return Instantiate (prefab, parent) as GameObject;
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}

    // Parameter level should be the center level
	public void DrawMinimap(Level level, GameObject player) {
		Level.Tile[,] map = level.GetMap ();

		Texture2D texture = new Texture2D(map.GetLength(0) + 2, map.GetLength(1) + 2, TextureFormat.RGBA32, false);
		for (int y = 0; y < map.GetLength(1); y++) {
			for (int x = 0; x < map.GetLength(0); x++) {
				switch (map [x, y]) {
					case Level.Tile.Wall:
						texture.SetPixel (x + 1, y + 1, MM_WALL_COLOR);
						break;
					case Level.Tile.ZombieSpawn:
                        texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
					case Level.Tile.AmmoSpawn:
						texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
					case Level.Tile.HealthKitSpawn:
						texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
					case Level.Tile.SpecialBoxSpawn:
						texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
					default:
						texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
				}
			}
		}
		// Paint borders
		for (int x = 0; x < map.GetLength(0) + 2; x++) {
			texture.SetPixel(x, 0, MM_BORDER_COLOR);
			texture.SetPixel(x, map.GetLength(1) + 1, MM_BORDER_COLOR);
		}
		for (int y = 0; y < map.GetLength(1) + 2; y++) {
			texture.SetPixel(0, y, MM_BORDER_COLOR);
			texture.SetPixel(map.GetLength(0) + 1, y, MM_BORDER_COLOR);
		}

        // Paint player
		LevelPosition playerPosition = GetLevelPosition(player, level);
		texture.SetPixel(playerPosition.x + 1, playerPosition.y + 1, MM_PLAYER_COLOR);

        // Paint zombies
		foreach(Warrior w in CrazyCaveLevelManager.Instance.GetLevel().GetWarriors()) {
			if (w.IsAlive ()) {
	            LevelPosition wPosition = GetLevelPosition (w.gameObject, level);
	            // We are assuming that level was the center level
	            if (wPosition.x >= 0 && wPosition.x < level.GetMap().GetLength(0)) {
	                if (wPosition.y >= 0 && wPosition.y < level.GetMap().GetLength(1)) {
	                    texture.SetPixel(wPosition.x + 1, wPosition.y + 1, MM_WARRIOR_COLOR);
					}
                }
            }
        }

		// Paint accessories
		PrintAccessories(level.GetAmmos(), level, texture, MM_AMMO_SPAWN_COLOR);
		PrintAccessories(level.GetHealthKits(), level, texture, MM_HEALTH_SPAWN_COLOR);
		PrintAccessories(level.GetSpecialBoxes(), level, texture, MM_SPECIAL_BOX_SPAWN_COLOR);

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		minimap.GetComponent<GUITexture>().texture = texture;
	}

	private void PrintAccessories(List<GameObject> list, Level level, Texture2D texture, Color color) {
		foreach (GameObject elem in list) {
			if (elem == null) {
				list.Remove (elem);
			} else {
				LevelPosition elemPos = GetLevelPosition (elem, level);
				// We are assuming that level was the center level
				if (elemPos.x >= 0 && elemPos.x < level.GetMap ().GetLength (0)) {
					if (elemPos.y >= 0 && elemPos.y < level.GetMap ().GetLength (1)) {
						texture.SetPixel (elemPos.x + 1, elemPos.y + 1, color);
					}
				}
			}
		}
	}

	public void CreateActionText(string text, Color color, Vector3 position) {
		position.z = -10;
		GameObject newText = NewObjectFromPrefab (actionText, position);
        newText.AddComponent (typeof(FlyingText));
		newText.GetComponent<TextMesh> ().text = text;
		newText.GetComponent<TextMesh> ().color = color;
		destroyables.Add (new TimeDestroyable(newText, 500));
	}

	private GameObject CreateAccessory(GameObject prefab, Transform transform, Vector3 position) {
        GameObject obj = NewObjectFromPrefab (prefab, transform);
        obj.transform.localPosition = position;
        BulletManager.Instance.IgnoreColliders(obj.GetComponent<Collider2D>());
        WarriorManager.Instance.IgnoreColliders(obj.GetComponent<Collider2D>());
		return obj;
    }
		
    public void TookDamage() {
        takingDamage = true;
        damageStarted = System.DateTime.Now;
    }

    public void ShowMiddleText(string text, Color color) {
        damagePanel.GetComponent<Image> ().color = color;
        middleText.text = text;
        middleText.enabled = true;
    }

    public void PauseText() {
        previousPauseColor = damagePanel.GetComponent<Image> ().color;
        damagePanel.GetComponent<Image> ().color = TRANSPARENT_WHITE;
        middleText.text = "PAUSE\nPRESS ESC TO CONTINUE";
        middleText.enabled = true;
    }

    public void UnpauseText() {
        damagePanel.GetComponent<Image> ().color = previousPauseColor;
        middleText.enabled = false;
    }
}
