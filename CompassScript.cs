using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a class to manage compass in the game GUI
public class CompassScript : MonoBehaviour {

    public Text compassText;			// reference to the Text component (to show cardinal directions: N, W, E, s)
	// all cardinal directions: north, west, south, east
    private static char north = 'N';
    private static char west = 'W';
    private static char south = 'S';
    private static char east = 'E';

    private Transform playerTransform;		// reference to player's transform component
	
	// Use this for initialization (before Start)
    void Awake ()
    {
		// init references
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Use this for initialization
	void Start () 
    {
        SetCompassText();		// call function SetCompassText in this script
	}

	// function to start updating the compass (called from PlayerMove script)
    public void UpdateCompass (float waitTime)
    {
		// set up "-" marker to the compassText and call function SetCompassText with delay "waitTime"
        compassText.text = "-";
        Invoke("SetCompassText", waitTime);
    }
	
	// function to set the heading to the compass
    private void SetCompassText ()
    {
		// define the current rotation of the player and define variable  compassChar
        int playerRotation = Mathf.RoundToInt(playerTransform.eulerAngles.y);
        char compassChar = '-';

		// switch case to define the rotation in Y-axis (0, 90, 180 or 270) and appropriate cardinal heading (north, west, south or east)
        switch (playerRotation)
        {
            case 0:
                compassChar = north;
                break;
            case 90:
                compassChar = east;
                break;
            case 180:
                compassChar = south;
                break;
            case 270:
                compassChar = west;
                break;
            default:
                compassChar = '.';
                break;
        }
                
        compassText.text = compassChar.ToString();		// set up the text to compassText component
    }
}
