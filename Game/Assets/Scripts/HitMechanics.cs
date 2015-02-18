using UnityEngine;
using System.Collections;

public class HitMechanics : MonoBehaviour {

	public float hitRadius = 1.8f;
	public float hitForce = 2500f;
	public float timeBetweenHits = 1f;
	public string enemyName = "Enemy";

	protected float timer;

	private LayerMask hitLayer;
	private PlayerMovement playerMov;

	public bool canShoot {get; set;}

	void Start ()
	{
		canShoot = true;

		playerMov = GetComponent<PlayerMovement> ();

		// Get the player layer so we only care about hitting the enemy
		hitLayer = 1 << LayerMask.NameToLayer ("Player");
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (Input.GetMouseButtonDown(0))
		{
			Hit ();
		}
	}

	void Hit ()
	{
		if (timer >= timeBetweenHits)
		{
			bool hitEnemy = false;
			Collider2D[] beenHit = Physics2D.OverlapCircleAll (transform.position, hitRadius, hitLayer);
			GameObject hitObj;
			for (int i = 0, len = beenHit.Length; i < len; i++)
			{
				hitObj = beenHit[i].gameObject;
				hitEnemy = HandleHitEnemy (enemyName, hitObj);
				if (hitEnemy)
				{
					break;
				}
			}
			// The player shoots when doesn't melee atacks the enemy
			if (!hitEnemy && canShoot)
			{
				Shoot ();
			}

			timer = 0f;
		}
	}

	public bool HandleHitEnemy (string enemyName, GameObject enemyObj)
	{
		bool hitEnemy = false;
		if (enemyName == enemyObj.name)
		{
			hitEnemy = true;

			PlayerMovement pm = enemyObj.GetComponent<PlayerMovement> ();
			// Remove gem from enemy if has
			if (pm.hasGem)
			{
				GameObject gem = pm.collectedGem;
				pm.hasGem = false;
				gem.GetComponentInChildren<GemCollect> ().isCollected = false;
				gem.GetComponentInChildren<CircleCollider2D> ().enabled = true;
				pm.collectedGem = null;
				
				if (gameObject.name == "Enemy")
				{
					GetComponentInParent<EnemyMovement> ().LookForGem();
				}
				else if (enemyObj.name == "Enemy")
				{
					enemyObj.GetComponent<EnemyMovement> ().LookForGem ();
				}
			}
			
			// Melee atack
			enemyObj.rigidbody2D.velocity = Vector2.zero;
			enemyObj.rigidbody2D.AddForce ((enemyObj.transform.position - transform.position) * hitForce);
		}

		return hitEnemy;
	}
	
	void Shoot ()
	{
		//Debug.Log ("Shoot");
		GameObject projectile = Instantiate (PrefabManager.Instance.ProjectilePrefab, 
		                                     transform.position, 
		                                     transform.rotation) 
											 as GameObject;

		projectile.GetComponentInChildren<Shot> ().mechanics = this;

		/*Vector2 projectVel = playerMov.facingCoordinates;
		projectile.GetComponent<Rigidbody2D> ().velocity = projectVel;
		Vector2 shootForce = Vector2.zero;
		shootForce.x = Mathf.Clamp (projectVel.x * 10, -1, 1);
		shootForce.y = Mathf.Clamp (projectVel.y * 10, -1, 1);
		projectile.GetComponent<Rigidbody2D> ().AddForce (shootForce * 2500f);*/

		Vector2 shotTarget = transform.position + playerMov.facingCoordinates * 1000000f;
		//Vector2 shootTarget = (transform.position + playerMov.facingCoordinates) * 100f;

		//Debug.Log (shotTarget);

		projectile.transform.LookAt (shotTarget);
		projectile.rigidbody2D.velocity = projectile.transform.forward * 25f;
		//projectile.transform.rotation = Quaternion.identity;
		//projectile.rigidbody2D.AddForce (shootTarget * 500f);
	}
}
