using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public MovementFace[] movementFaces;
	public float maxSpeed = 10f;
	public bool hasGem = false;
	public GameObject collectedGem;

	protected SpriteRenderer spriteRend;
	protected string facingDirection;
	protected Dictionary<string, Sprite> movementDict;
	protected float moveH, moveV;

	// Is virtual in order to be overriden for children classes
	protected virtual void Start () 
	{
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
