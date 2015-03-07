using UnityEngine;
using System.Collections;

public class EnemyMovement : PlayerMovement {

	public Transform goalZone;
	public float minDistance = 0.85f;

	private Transform target;
	private Transform myTransform;
	private Transform player;
	private PlayerMovement playerMov;
	private Vector2 nextPos;
	private Vector2 dodgePos;
	private Vector2[] dodgePositions;
	//private Vector3 spriteSize;
	private bool dodging = false;
	private float dodgeTimer;
	private float maxTimeDodging = 1f;

	public bool targetIsPlayer {get; private set;}

	protected override void Start ()
	{
		base.Start();

		//spriteSize = spriteRend.bounds.size;

		targetIsPlayer = false;

		// Enemy starts facing LEFT
		facingCoordinates = new Vector2 (-1, 0);

		myTransform = transform;

		target = GameObject.FindGameObjectWithTag ("Scorable").transform;

		player = GameObject.Find ("Player").transform;
		playerMov = player.GetComponent<PlayerMovement> ();

		setDodgePositions ();
	}

	void setDodgePositions ()
	{
		dodgePositions = new Vector2[8];
		int count = 0;
		for (int i = -1, max1 = 1; i <= max1; i++)
		{
			for (int j = -1, max2 = 1; j <= max2; j++)
			{
				if (i != 0 || j != 0) 
				{
					dodgePositions[count] = new Vector2 (i, j);
					count++;
				}
			}
		}
	}

	void Update () 
	{
		if (targetIsPlayer && !playerMov.hasGem)
		{
			targetIsPlayer = false;
			LookForGem ();
		}

		if (dodging)
		{
			Dodge();
		}
		else
		{
			// Keep track of target location
			myTransform.LookAt (target);
		}

		nextPos = myTransform.forward * maxSpeed;

		// Set rotation to 0,0,0,0
		// So the enemy's shape doesn't distort on moving
		myTransform.rotation = Quaternion.identity;
	}

	void Dodge ()
	{
		// We keep track of the time since the enemy started to dodge
		// to prevent the enemy dodging last forever
		dodgeTimer += Time.deltaTime;
		Vector2 curPos = myTransform.position;
		if (AlmostInPlace (curPos, dodgePos, 0.1f) || dodgeTimer >= maxTimeDodging)
		{
			dodging = false;
			dodgeTimer = 0;
		}
		else
		{
			myTransform.LookAt (dodgePos);
		}
	}

	bool AlmostInPlace (Vector2 a, Vector2 b, float epsilon)
	{
		bool almostInX = Mathf.Abs(a.x - b.x) < epsilon;
		bool almostInY = Mathf.Abs(a.y - b.y) < epsilon;

		return almostInX && almostInY;
	}

	void FixedUpdate ()
	{	
		rigidbody2D.velocity = nextPos;

		moveH = rigidbody2D.velocity.x;
		moveV = rigidbody2D.velocity.y;

		UpdateFacingCoords ();

		HandleFacing ();
	}

	protected override void HandleFacing ()
	{
		facingDirection = "";
		if (_facingCoordinates.y == 1) 
		{
			facingDirection = "Up";
		}
		else if (_facingCoordinates.y == -1)
		{
			facingDirection = "Down";
		}
		
		if (_facingCoordinates.x == 1)
		{
			facingDirection += "Right";
		}
		else if (_facingCoordinates.x == -1)
		{
			facingDirection += "Left";
		}
		
		if (facingDirection != "")
		{
			spriteRend.sprite = movementDict[facingDirection];
		}
	}

	public void TryDodge (Vector2 hitOrigin)
	{
		Vector2 curPos = myTransform.position;
		Vector2 hitDistance = hitOrigin - curPos;
		// The direction from where the shot is coming
		Vector2 hitDir = Vector2.zero;
		// Convert any number in 1 if greater than 0, -1 if lower than 0, otherwise 0
		hitDir.x = hitDistance.x < -0.3 ? -1: hitDistance.x > 0.3 ? 1:0;
		hitDir.y = hitDistance.y < -0.3 ? -1: hitDistance.y > 0.3 ? 1:0;

		//float rayLength = spriteSize.x;
		// The dodge position is choosen by a random because we don't want
		// the enemy trying to dodge always in the same direction
		int random;
		Vector2 dodgeDir;
		for (int i = 1, tries = 8; i < tries && !dodging; i++)
		{
			random = Random.Range (0, 8);
			dodgeDir = dodgePositions[random];
			if (hitDir != dodgeDir && hitDir != (dodgeDir * -1))
			{
				//dodgePos = curPos + (dodgeDir * rayLength);
				dodgePos = curPos + dodgeDir;
				RaycastHit2D[] blockingObjs = Physics2D.LinecastAll(curPos, dodgePos);
				dodging = blockingObjs.Length == 0;
				for (int j = 0, len = blockingObjs.Length; j < len && !dodging; j++)
				{
					dodging = true;
					if (blockingObjs[j].transform.name != "Enemy")
					{
						dodging = blockingObjs[j].transform.tag == "Scorable";
					}
				}
			}
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

	public override void LookForGem ()
	{
		target = GameObject.FindGameObjectWithTag ("Scorable").transform;
	}
}
