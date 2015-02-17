using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public MovementFace[] movementFaces;
	public float maxSpeed = 10f;

	protected SpriteRenderer spriteRend;
	protected string facingDirection;
	protected Dictionary<string, Sprite> movementDict;
	protected float moveH, moveV;
	protected Vector3 _facingCoordinates;

	public GameObject collectedGem {get; set;}
	public bool hasGem {get; set;}

	public Vector3 facingCoordinates 
	{
		get {return _facingCoordinates;}
		protected set {_facingCoordinates = value;}
	}

	// Is virtual in order to be overriden for children classes
	protected virtual void Start () 
	{
		hasGem = false;

		// Player always starts facing RIGHT
		facingCoordinates = new Vector2 (1, 0);
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
		moveH = Input.GetAxis ("Horizontal");
		moveV = Input.GetAxis ("Vertical");

		// Keep track of the last facing coordinates that are 
		// different from 0
		if (moveH != 0 || moveV != 0)
		{
			_facingCoordinates.x = Mathf.Clamp (moveH, -1, 1);
			_facingCoordinates.y = Mathf.Clamp (moveV, -1, 1);
		}
		//if (moveH != 0) {lastFacing.x = Mathf.Clamp (moveH, -1, 1);}
		//if (moveV != 0) {lastFacing.y = Mathf.Clamp (moveV, -1, 1);}
	}

	protected virtual void FixedUpdate ()
	{
		rigidbody2D.velocity = new Vector3 (moveH * maxSpeed, moveV * maxSpeed, 0);

		HandleFacingDirection();
	}

	void HandleFacingDirection ()
	{
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
	}

	public virtual void NotifGemCollect () {}

	public virtual void LookForGem () {}

	// Keep the different movement sprites organized
	// Using a name for each sprite to describe
	// The sprite illustration
	[System.Serializable]
	public class MovementFace {
		public string name;
		public Sprite sprite;
	}
}
