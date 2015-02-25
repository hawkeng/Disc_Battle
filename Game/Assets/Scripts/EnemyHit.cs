using UnityEngine;
using System.Collections;

public class EnemyHit : HitMechanics {

	public float distanceToShoot = 5f;

	private Transform player;
	private Transform myTransform;
	private float distanceToPlayer;

	protected override void Start () 
	{
		base.Start();

		playerMov = GetComponentInParent<PlayerMovement> ();

		myTransform = transform;
		player = GameObject.Find ("Player").transform;
	}

	void Update ()
	{
		timer += Time.deltaTime;
	}

	void FixedUpdate () 
	{
		distanceToPlayer = Vector3.Distance (myTransform.position, player.position);
		if (distanceToPlayer <= distanceToShoot && timer >= timeBetweenHits)
		{
			Shoot ();
			timer = 0f;
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (timer >= timeBetweenHits)
		{
			HandleHitEnemy (enemyName, coll.gameObject);

			timer = 0f;
		}
	}
}
