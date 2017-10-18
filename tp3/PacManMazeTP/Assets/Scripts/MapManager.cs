// Mono Framework
using System.Collections.Generic;
using System.IO;

// Unity Framework
using UnityEngine;

public class MapManager : MonoBehaviorSingleton<MapManager> 
{
    public Sprite[] mapTiles;
	public GameObject playerPrefab;

    public char tile0Char = 'a';
	public char dotChar = 'J';
	public char playerStartChar = '1';

    public char defChar = ' ';

    public int cols = 24;
    public int rows = 31;

    public int tileWidth = 16;
    public int tileHeight = 16;

    int[,] mapData;

    TileInfo[,] mapSprites;

	int ofsx;
	int ofsy;

	// Unity Start Method
	void Start()
    {
		ofsx = ((cols * tileWidth) / 2) - (tileWidth / 2);
		ofsy = (rows * tileHeight) / 2 - (tileHeight / 2);

        LoadLevel(1);
        CreateMap();
	}

    void LoadLevel(int lvl)
    {
        TextAsset ta = Resources.Load<TextAsset>(string.Format("level{0}", lvl));
        StringReader sr = new StringReader(ta.text);

        mapData = new int[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
				// El mapa especifica más filas de las que tiene el archivo, se
				// completa con defChar
				for (int c = 0; c < cols; c++)
				{
					mapData[r, c] = defChar;
				}
            }
            else
            {
                for (int c = 0; c < cols; c++)
                {
                    if (c < line.Length)
                    {
						if (line[c] == playerStartChar)
						{
							PlacePlayer(GetWorldPos(r, c));

							// Dejo limpio en la estructura la celda donde ubico al jugador
							mapData[r, c] = defChar;
						}
						else
							mapData[r, c] = line[c];

                    }
                    else
                    {
                        // La línea leída no tiene tantos caracteres como
                        // especifica la estructura del mapa que debe tener,
                        // se completa con defChar
                        mapData[r, c] = defChar;
                    }
                }
            }
        }
    }

    void CreateMap()
    {
        if (mapData != null)
        {
            if (mapSprites == null) mapSprites = new TileInfo[rows, cols];

            GameObject tile;
            SpriteRenderer sprRnd;
            TileInfo ti;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
					if (mapData[r, c] != defChar && mapData[r, c] != playerStartChar)
                    {
                        if (mapSprites[r, c] == null)
                        {
                            tile = new GameObject(string.Format("tile {0} - {1}", r, c)); 
                            sprRnd = tile.AddComponent<SpriteRenderer>();
                            ti = tile.AddComponent<TileInfo>();
                            mapSprites[r, c] = ti;
                        }
                        else
                        {
                            tile = mapSprites[r, c].gameObject;
                            sprRnd = tile.GetComponent<SpriteRenderer>();
                            ti = tile.GetComponent<TileInfo>();
                        }

						sprRnd.sprite = mapTiles[mapData[r, c] - tile0Char];
						tile.transform.position = new Vector3(c * tileWidth - ofsx, ofsy - r * tileHeight, 0f);
						tile.transform.SetParent(gameObject.transform);

						ti.tileNum = mapData[r, c];
						ti.sprRnd = sprRnd;
                    }
                }
            }
        }
    }

	void PlacePlayer(Vector3 pos)
	{
		GameObject player = UnityEngine.Object.Instantiate(playerPrefab);
		player.transform.position = pos;
		player.name = "Player";
	}

	// Retorna la posición de mundo de la celda específicada
	// (el cero local de la celda es su centro)
	public Vector3 GetWorldPos(int row, int col)
	{
		return new Vector3(col * tileWidth - ofsx, ofsy - row * tileHeight, 0f);
	}

	// Retorna verdero si la posición especificada se encuentra dentro del mapa
	// y el row/col de dicha posición en los parámetros de salida
	public bool GetRowCol(Vector3 pos, out int row, out int col)
	{
		row = (int) Mathf.Floor((pos.y - ofsy - (tileHeight / 2)) / -tileHeight);
		col = (int) Mathf.Floor((pos.x + ofsx + (tileWidth / 2)) / tileWidth);
		return (0 <= row && row < rows && 0 <= col && col < cols);
	}

	// Retorna verdadero si la celda se encuentra vacía o con un dot
	public bool IsWalkable(int row, int col)
	{
		return (mapData[row, col] == defChar || mapData[row, col] == dotChar);
	}

	// Retorna verdadero si la celda se encuentra vacía o con un dot
	public bool IsWalkable(Vector3 pos)
	{
		int r, c;
		if (GetRowCol(pos, out r, out c))
			return IsWalkable(r, c);
		else
			return false;
	}

	public Vector3 ClampWorldPos(Vector3 pos)
	{
		int r, c;
		if (GetRowCol(pos, out r, out c))
		{
			return GetWorldPos(r, c);
		}
		else
			return Vector3.zero;
	}

	// Remueve el dot de la posición específicada
	public bool EatDot(int row, int col)
	{
		if (mapData[row, col] == dotChar)
		{
			mapData[row, col] = defChar;
			mapSprites[row, col].sprRnd.sprite = null;
			return true;
		}
		else
			return false;
	}

}
