using UnityEngine;
using System.Collections;

public class EnemyMovement : PlayerMovement {

	public Transform goalZone;
	public float minDistance = 0.85f;

	private Transform target;
	private Transform myTransform;
	//private Vector3 prevPos;
	private Vector2 nextPos;
	private bool targetIsPlayer = false;

	protected override void Start ()
	{
		base.Start();
		myTransform = transform;
		target = GameObject.FindGameObjectWithTag ("Scorable").transform;
		//prevPos = myTransform.position;
	}

	void Update () 
	{
		// We need to emulate the Input.GetAxis behaviour in our enemy
		// So we catch the difference between the current position and
		// the last position we were. We take the x and y difference
		// and normalize it to be -1, 0 or 1 with Clamp
		/*Vector2 posDifference = (myTransform.position - prevPos) / Time.deltaTime;
		moveH = Mathf.Clamp (posDifference.x, -1, 1);
		moveV = Mathf.Clamp (posDifference.y, -1, 1);
		prevPos = myTransform.position;*/

		// Keep track of target location
		myTransform.LookAt (target);

		// NOTE: Uncomment this block if the enemy movement fails
		//       and comment the 'nextPos' line
		/*float distanceToPlayer = (target.position - myTransform.position).magnitude;
		if (!targetIsPlayer || distanceToPlayer >= minDistance)
		{
			// Move the current position to the target position
			myTransform.position += myTransform.forward * maxSpeed * Time.deltaTime;

		}*/

		// If the enemy starts to disappear when moving comment the block above and uncomment this:
		/*Vector3 tmpPos = myTransform.position + myTransform.forward * maxSpeed * Time.deltaTime;
		tmpPos.z = 0;
		myTransform.position = tmpPos;*/

		nextPos = myTransform.forward * maxSpeed;

		// Set rotation to 0,0,0,0
		// So the enemy's shape doesn't distort on moving
		myTransform.rotation = Quaternion.identity;
	}

	protected override void FixedUpdate ()
	{	
		rigidbody2D.velocity = nextPos;

		moveH = Mathf.Clamp (rigidbody2D.velocity.x, -1, 1);
		moveV = Mathf.Clamp (rigidbody2D.velocity.y, -1, 1);

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

	public void PlayerGotGem () 
	{
		targetIsPlayer = true;
		target = GameObject.Find ("Player").transform;
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
