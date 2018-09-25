using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to manage all tiles within TilesHolder parent gameobject
public class TilesHolder : MonoBehaviour {

    public int columns;				// amount of tiles in X axis
    public int rows;				// amount of tiles in Z axis
    public char[][] tiles;			// 2d array of tiles

    private static char empty = '_';		// empty tile
    private static char wall = '#';			// wall tile
    private static char floor = '.';		// floor tile

    private Transform myTransform;			// reference to this transform
    private MapControl mapCtrl;				// reference to MapControl script

	// Use this for initialization
    void Awake ()
    {
		// init references
        myTransform = this.transform;
        mapCtrl = GameObject.FindGameObjectWithTag("Map").GetComponent<MapControl>();

        InitTileMap();		// call function InitTileMap from this script
    }

	// Use this for initialization
	void Start () 
    {
		// call functions FillTileMap from this script and InitMap from MapControl script with values
        FillTileMap();
        mapCtrl.InitMap(tiles, columns, rows);
	}
	
	// function to setup tiles array
    private void InitTileMap ()
    {
		// setup tiles array size
        tiles = new char[columns][];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new char[rows];
        }
		
		// set up the content of tiles array (default type of tiles: empty)
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                tiles[i][j] = empty;
            }
        }
    }

	// function to fill the tiles array with floor and wall tiles
    private void FillTileMap ()
    {
		// iterate all child objects in this transform
        foreach (Transform child in myTransform)
        {
			// get the X and Z values of the position of the child object
            int posX = (int)child.localPosition.x;
            int posZ = (int)child.localPosition.z;

			// if the current child is wall (by it's name), the current tile is wall
            if(child.name.Contains("Wall"))
            {
                tiles[posX][posZ] = wall;
            }
			// if the current child is floor (by it's name), the current tile is floor
            else if (child.name.Contains("Floor"))
            {
                tiles[posX][posZ] = floor;
            }
        }
    }
}
