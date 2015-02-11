using UnityEngine;
using System.Collections;

public class EnemyMovement : PlayerMovement {

	public Transform goalZone;

	private Transform target;
	private Transform myTransform;
	private float minDistance;
	private Vector3 prevPos;
	private Vector3 posDifference;

	void Awake ()
	{
		myTransform = transform;
	}

	protected override void Start ()
	{
		base.Start();
		target = GameObject.FindGameObjectWithTag ("Scorable").transform;
		//target = GameObject.Find ("Player").transform;
		prevPos = myTransform.position;
	}

	void Update () 
	{
		// We need to emulate the Input.GetAxis behaviour in our enemy
		// So we catch the difference between the current position and
		// the last position we were. We take the x and y difference
		// and normalize it to be -1, 0 or 1 with Clamp
		posDifference = (myTransform.position - prevPos) / Time.deltaTime;
		moveH = Mathf.Clamp (posDifference.x, -1, 1);
		moveV = Mathf.Clamp (posDifference.y, -1, 1);
		prevPos = myTransform.position;

		// Keep track of target location
		myTransform.LookAt (target);

		// Move the current position to the target position
		// Also restrict z axis movement
		myTransform.position = myTransform.position + myTransform.forward * maxSpeed * Time.deltaTime;

		// If the enemy starts to disappear comment the code above and uncomment this:
		/*Vector3 tmpPos = myTransform.position + myTransform.forward * maxSpeed * Time.deltaTime;
		tmpPos.z = 0;
		myTransform.position = tmpPos;*/

		// Set rotation to 0,0,0
		myTransform.rotation = Quaternion.identity;
	}

	protected override void FixedUpdate ()
	{	
		//Debug.Log ("H: " + moveH + " --- V: " + moveV);

		facingDirection = "";
		if (moveV == 1) 
		{
			facingDirection = "Up";
		}
		else if (moveV == -1)
		{
			facingDirection = "Down";
		}
		
		if (moveH == 1)
		{
			facingDirection += "Right";
		}
		else if (moveH == -1)
		{
			facingDirection += "Left";
		}
		
		if (facingDirection != "")
		{
			spriteRend.sprite = movementDict[facingDirection];
		}
	}

	public override void NotifGemCollect ()
	{
		target = goalZone;
	}

	public override void HandleGoal ()
	{
		target = GameObject.FindGameObjectWithTag ("Scorable").transform;
	}
}
