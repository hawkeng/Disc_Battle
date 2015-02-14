using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public MovementFace[] movementFaces;
	public float maxSpeed = 10f;
	public float hitRadius = 2.19f;
	public float hitForce = 5000f;
	public float timeBetweenHits = 1f;

	protected SpriteRenderer spriteRend;
	protected string facingDirection;
	protected Dictionary<string, Sprite> movementDict;
	protected float moveH, moveV;
	protected LayerMask hitLayer;
	protected bool isHitting = false;
	protected GameObject gemObject;
	protected bool carryingGem = false;
	protected float timer;

	// Is virtual in order to be overriden for children classes
	protected virtual void Start () 
	{
		// Get the player layer so we only care about hit the enemy
		hitLayer = 1 << LayerMask.NameToLayer ("Player");
		spriteRend = GetComponent<SpriteRenderer> ();

		MovementFace[] mf = movementFaces;
		movementDict = new Dictionary<string, Sprite>();
		for (int i = 0, len = mf.Length; i < len; i++) 
		{
			movementDict.Add(mf[i].name, mf[i].sprite);
		}
	}
	
	void Update () 
	{
		timer += Time.deltaTime;
		moveH = Input.GetAxis ("Horizontal");
		moveV = Input.GetAxis ("Vertical");

		if (Input.GetButton("Fire1"))
		{
			isHitting = true;
		}
	}

	protected virtual void FixedUpdate ()
	{
		rigidbody2D.velocity = new Vector3 (moveH * maxSpeed, moveV * maxSpeed, 0);

		facingDirection = "";
		if (moveV > 0) 
		{
			facingDirection = "Up";
		}
		else if (moveV < 0)
		{
			facingDirection = "Down";
		}

		if (moveH > 0)
		{
			facingDirection += "Right";
		}
		else if (moveH < 0)
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
					if (hitObj.name == "Enemy")
					{
						hitObj.rigidbody2D.AddForce ((hitObj.transform.position - transform.position) * hitForce);
					}
				}
				timer = 0f;
			}
			isHitting = false;
		}
	}

	public GameObject collectedGem 
	{
		get {return gemObject;}
		set {gemObject = value;}
	}

	public bool hasGem
	{
		get {return carryingGem;}
		set {carryingGem = value;}
	}

	public virtual void NotifGemCollect () {}

	public virtual void HandleGoal () {}

	// Keep the different movement sprites organized
	// Using a name for each sprite to describe
	// The sprite illustration
	[System.Serializable]
	public class MovementFace {
		public string name;
		public Sprite sprite;
	}
}
