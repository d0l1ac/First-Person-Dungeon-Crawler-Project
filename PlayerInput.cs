using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to manage all player keyboard input
public class PlayerInput : MonoBehaviour {

    private PlayerMove playerMove;		// reference to PlayerMove script

	// Use this for initialization
	void Awake () 
    {
		// init references
        playerMove = GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		// if player is not moving (return value of IsMoving is false)
        if (!playerMove.IsMoving())
        {
            GetPlayerInput();		// call function GetPlayerInput from this script
        }
	}

	// function to manage player keyboard input
    private void GetPlayerInput()
    {
		// variables of horizontal moving, vertical moving and turning
        int horizontal = 0;
        int vertical = 0;
        int turning = 0;

		// cache movement and turning values based on input axis
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        turning = (int)(Input.GetAxisRaw("Turning"));

		// limit movement and turning values so that only one value is something than zero
        if (horizontal != 0)
        {
            vertical = 0;
            turning = 0;

            playerMove.MovePlayer(horizontal, 0, 0);		// call function MovePlayer from this script
        }
        else if (vertical != 0)
        {
            horizontal = 0;
            turning = 0;

            playerMove.MovePlayer(0, vertical, 0);			// call function MovePlayer from this script
        }
        else if (turning != 0)
        {
            horizontal = 0;
            vertical = 0;

            playerMove.MovePlayer(0, 0, turning);			// call function MovePlayer from this script
        }
    }
}
