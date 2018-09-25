using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a class to manage the Minimap of the game
public class MapControl : MonoBehaviour {

	// colors of the map: empty, wall, floor and player
    public Color emptyColor;
    public Color wallColor;
    public Color floorColor;
    public Color playerColor;

	// char variables to wall and to floor tiles
    private static char wall = '#';
    private static char floor = '.';

	// 2d arrays of tiles and lit area (fog of war)
    private char[][] tiles;
    private bool[][] lit;

	// array of adjacent tiles of the player
    private Vector2[] adjacentTiles = new Vector2[9]
        {new Vector2(0,0), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0), new Vector2(0,1), new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)  };

    private int playerX, playerZ, width, height;		// player's position x and z coordinates, minimap width and height
    private Texture2D myTexture;					// texture of the map
    private Transform myTransform;					// reference to transform of this game object
    private Transform playerTransform;			// reference to player's Transform component

	// Use this for initialization
    void Awake ()
    {
		// init references
        myTransform = this.transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

	// function to initialize the minimap
    public void InitMap (char[][] newTiles, int newWidht, int newHeight)
    {
		// define widht, height and tiles array
        width = newWidht;
        height = newHeight;
        tiles = newTiles;

		// define mytexture, and set this texture to the renderer component of this object
        myTexture = new Texture2D(width, height);
        myTexture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.mainTexture = myTexture;

        lit = new bool[width][];		// define lit array

		// iterate tiles array
        for (int i = 0; i < tiles.Length; i++)
        {
            lit[i] = new bool[height];		// define lit array
        }

        UpdateMap();		// call function UpdateMap in this script
    }

	// function to update the minimap
    public void UpdateMap ()
    {
		// define player's x and z coordinate values
        playerX = Mathf.RoundToInt(playerTransform.position.x);
        playerZ = Mathf.RoundToInt(playerTransform.position.z);

        LitMap(playerX, playerZ);		// call function LitMap from this script

		// iterate myTexture width
        for (int x = 0; x < myTexture.width; x++)
        {
			// iterate myTexture heihgt
            for (int y = 0; y < myTexture.height; y++)
            {
				// if the area in the minimap is lit
                if (lit[x][y])
                {
					// if the current tile is the player, set the playerColor to the tile
                    if (x == playerX && y == playerZ)
                    {
                        myTexture.SetPixel(x, y, playerColor);
                    }
					// if the current tile is wall, set the wallColor to the tile
                    else if (tiles[x][y] == wall)
                    {
                        myTexture.SetPixel(x, y, wallColor);
                    }
					// if the current tile is floor, set the floorColor to the tile
                    else if (tiles[x][y] == floor)
                    {
                        myTexture.SetPixel(x, y, floorColor);
                    }
					// otherwise set emptycolor to the current tile
                    else
                    {
                        myTexture.SetPixel(x, y, emptyColor);
                    }
                }
				// if the area is not lit, set emptycolor to the current tile
                else
                {
                    myTexture.SetPixel(x, y, emptyColor);
                }

                myTexture.Apply();			// apply a new texture to the minimap
            }
        }  
    }

	// function to lit adjacent tiles around the player
    private void LitMap (int newPlayerX, int newPlayerZ)
    {
		// iterate adjacentTiles array
        for (int i = 0; i < adjacentTiles.Length; i++)
        {
			// define the x and z values in the lit area
            int newX = newPlayerX + (int)adjacentTiles[i].x;
            int newZ = newPlayerZ + (int)adjacentTiles[i].y;
			
			// make sure x and z values do not exceed width or height of the minimap
            newX = Mathf.Clamp(newX, 0, width);
            newZ = Mathf.Clamp(newZ, 0, height);

			// if the current tile in the lit area is not lit, lit it
            if(lit[newX][newZ] == false)
            {
                lit[newX][newZ] = true;
            }
        }
    }
}
