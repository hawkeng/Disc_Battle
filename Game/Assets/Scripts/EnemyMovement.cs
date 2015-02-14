using UnityEngine;
using System.Collections;

public class EnemyMovement : PlayerMovement {

	public Transform goalZone;
	public float minDistance = 0.85f;

	private Transform target;
	private Transform myTransform;
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
		timer += Time.deltaTime;

		// Keep track of target location
		myTransform.LookAt (target);

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

		if (isHitting)
		{
			if (timer >= timeBetweenHits)
			{
				RaycastHit2D[] beenHit = Physics2D.CircleCastAll (transform.position, hitRadius, Vector2.zero, Mathf.Infinity, hitLayer);
				for (int i = 0, len = beenHit.Length; i < len; i++)
				{
					GameObject hitObj = beenHit[i].transform.gameObject;
					if (hitObj.name == "Player")
					{
						hitObj.rigidbody2D.AddForce ((hitObj.transform.position - transform.position) * hitForce);
					}
				}
				timer = 0f;
			}
			isHitting = false;
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

	
	public bool playerIsTarget 
	{
		get {return targetIsPlayer;}
	}
}
