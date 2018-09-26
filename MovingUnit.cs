using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a parent class to manage unit movement (player, enemies)
public abstract class MovingUnit : MonoBehaviour {

    public float moveTime = 0.3f;		// move time
    public float turnTime = 0.3f;		// turn time
    public float moveSpeed = 0.5f;		// unit move speed
    public LayerMask blockLayer;		// layer to check obstacles (walls etc)

    private float invMoveTime;			// inverse movement time
    private bool isMoving;				// is unit moving
    private bool canMove;				// is unit able to move

	// Use this for initialization
    protected virtual void Awake ()
    {
        invMoveTime = 1f / moveTime;		// calculate invMoveTime
    }

	// function to start moving
    protected virtual void StartMove (Vector3 newDir)
    {
		// if unit is not moving
        if (!isMoving)
        {
			// make a RaycastHit and call function Move from this script
            RaycastHit hit;
            Move(newDir, out hit);
        }
    }

	// function to start turning
    protected virtual void StartRotation (int newRot)
    {
				// if unit is not moving
        if (!isMoving)
        {
            StartCoroutine(Turning(newRot));		// start ienumerator Turning
        }
    }

	// function to check if movement is possible
    protected virtual void Move (Vector3 newDir, out RaycastHit hit)
    {
		// define start and end positions
        Vector3 start = transform.position;
        Vector3 end = newDir;

		// if a linecast hits something, cannot move (obstacle)
        if (Physics.Linecast(start, end, out hit, blockLayer))
        {
            canMove = false;
        }
		// otherwise movement is possible: call ienumerator Movement
        else
        {
            canMove = true;
            StartCoroutine(Movement(newDir));
        }
    }

	// function to do actual movement from grid to grid
    protected IEnumerator Movement (Vector3 end)
    {
		// unit is moving and define the distance from unit to end position
        isMoving = true;
        float remainDist = (transform.position - end).sqrMagnitude;

		// while remaining distance is greater than 0
        while (remainDist > float.Epsilon)
        {
			// define new position, move unit by time and check new distance from unit to end position
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, invMoveTime * Time.deltaTime);
            transform.position = newPosition;
            remainDist = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        isMoving = false;		// unit is no longer moving
    }

	// function to do actual turning from side to side in 90 degrees
    protected IEnumerator Turning (int newRot)
    {
		// unit is moving, define start and end rotation, define turn rate and time
        isMoving = true;
        int degrees = newRot * 90;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transform.rotation * Quaternion.Euler(0, degrees, 0);
        float rate = 1f / turnTime;
        float t = 1f;

		// while time is greater than 0
        while (t > float.Epsilon)
        {
			// reduce turning time and rotate unit by time
            t -= Time.deltaTime * rate;
            transform.rotation = Quaternion.Slerp(endRot, startRot, t);
            yield return new WaitForEndOfFrame();
        }

        isMoving = false;		// unit is no longer moving
    }

	// function to handle no moving situations
    protected abstract void CannotMove <T>(T component)
        where T: Component;

	// function to get isMoving value
    public bool IsMoving ()
    {
        return isMoving;
    }

	// function to get canMove value
    public bool CanMove ()
    {
        return canMove;
    }
}
