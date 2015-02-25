using UnityEngine;
using System.Collections;

public class HitMechanics : MonoBehaviour {

	public float hitRadius = 1.8f;
	public float hitForce = 2500f;
	public float timeBetweenHits = 1f;
	public string enemyName = "Enemy";

	protected float timer;
	protected bool isHitting = false;
	protected PlayerMovement playerMov;

	private LayerMask hitLayer;

	public bool canShoot {get; set;}

	protected virtual void Start ()
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
			isHitting = true;
		}
	}

	void FixedUpdate ()
	{
		if (isHitting)
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
			for (int i = 0, len = beenHit.Length; i < len && !hitEnemy; i++)
			{
				hitObj = beenHit[i].gameObject;
				hitEnemy = HandleHitEnemy (enemyName, hitObj);
			}
			// The player shoots when doesn't melee atacks the enemy
			if (!hitEnemy && canShoot)
			{
				ShootAndNotify ();
			}

			isHitting = false;
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

	private void ShootAndNotify ()
	{
		Vector2 shotTarget;
		Shoot (out shotTarget);

		//RaycastHit2D[] objectsOnShotPath = Physics2D.LinecastAll (transform.position, shotTarget, hitLayer);
		// usar circlecast con radio considerable para detectar colisiones futuras y no sólo inmediatas
		RaycastHit2D[] objectsOnShotPath = Physics2D.CircleCastAll (transform.position, 1f, shotTarget, 100f, hitLayer);
		for (int i = 0, len = objectsOnShotPath.Length; i < len; i++)
		{
			GameObject hitObj = objectsOnShotPath[i].transform.gameObject;

			if (hitObj.name == "Enemy")
			{
				NotifyShooting(transform.position);
			}
		}
	}

	protected virtual void Shoot (out Vector2 shotTarget)
	{
		GameObject projectile = Instantiate (PrefabManager.Instance.ProjectilePrefab, 
		                                     transform.position, 
		                                     transform.rotation) 
											 as GameObject;

		Shot shotMan = projectile.GetComponentInChildren<Shot> ();
		shotMan.mechanics = this;

		shotTarget = transform.position + playerMov.facingCoordinates * 1000000f;

		projectile.transform.LookAt (shotTarget);
		projectile.rigidbody2D.velocity = projectile.transform.forward * shotMan.speed;
	}
	
	protected virtual void Shoot ()
	{
		GameObject projectile = Instantiate (PrefabManager.Instance.ProjectilePrefab, 
		                                     transform.position, 
		                                     transform.rotation) 
											 as GameObject;

		Shot shotMan = projectile.GetComponentInChildren<Shot> ();
		shotMan.mechanics = this;

		Vector2 shotTarget = transform.position + playerMov.facingCoordinates * 1000000f;

		projectile.transform.LookAt (shotTarget);
		projectile.rigidbody2D.velocity = projectile.transform.forward * shotMan.speed;
	}

	private void NotifyShooting (Vector2 hitOrigin)
	{
		GameObject.Find ("Enemy").GetComponent<EnemyMovement> ().TryDodge(hitOrigin);
	}
}
