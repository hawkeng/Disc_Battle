using UnityEngine;
using System.Collections;

public class HitMechanics : MonoBehaviour {

	public float hitRadius = 1.8f;
	public float hitForce = 2500f;
	public float timeBetweenHits = 1f;

	private LayerMask hitLayer;
	private float timer;
	private PlayerMovement playerMov;
	private bool hitEnemy = false;

	void Start ()
	{
		playerMov = GetComponent<PlayerMovement> ();

		// Get the player layer so we only care about hitting the enemy
		hitLayer = 1 << LayerMask.NameToLayer ("Player");
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (Input.GetMouseButtonDown(0))
		{
			HandleHitMechanics ();
		}
	}

	void HandleHitMechanics ()
	{
		if (timer >= timeBetweenHits)
		{
			RaycastHit2D[] beenHit = Physics2D.CircleCastAll (transform.position, hitRadius, Vector2.zero, Mathf.Infinity, hitLayer);
			for (int i = 0, len = beenHit.Length; i < len; i++)
			{
				GameObject hitObj = beenHit[i].transform.gameObject;
				if (hitObj.name == "Enemy")
				{
					hitEnemy = true;

					PlayerMovement pm = hitObj.GetComponent<PlayerMovement> ();
					// Remove gem from enemy if has
					if (pm.hasGem)
					{
						GameObject gem = pm.collectedGem;
						pm.hasGem = false;
						gem.GetComponentInChildren<GemCollect> ().isCollected = false;
						gem.GetComponentInChildren<CircleCollider2D> ().enabled = true;
						pm.collectedGem = null;
						
						hitObj.GetComponent<EnemyMovement> ().LookForGem();
					}
					
					// Melee atack
					hitObj.rigidbody2D.velocity = Vector2.zero;
					hitObj.rigidbody2D.AddForce ((hitObj.transform.position - transform.position) * hitForce);
				}
			}
			// The player shoots when doesn't melee atacks the enemy
			if (!hitEnemy)
			{
				hitEnemy = false;
				Shoot ();
			}
			
			timer = 0f;
		}
	}

	/*protected bool HandleHitEnemy (string enemyObjName, GameObject enemyObj)
	{

	}*/
	
	void Shoot ()
	{
		//Debug.Log ("Shoot");
		GameObject projectile = Instantiate (PrefabManager.Instance.ProjectilePrefab, 
		                                     transform.position, 
		                                     transform.rotation) 
											 as GameObject;

		/*Vector2 projectVel = playerMov.facingCoordinates;
		projectile.GetComponent<Rigidbody2D> ().velocity = projectVel;
		Vector2 shootForce = Vector2.zero;
		shootForce.x = Mathf.Clamp (projectVel.x * 10, -1, 1);
		shootForce.y = Mathf.Clamp (projectVel.y * 10, -1, 1);
		projectile.GetComponent<Rigidbody2D> ().AddForce (shootForce * 2500f);*/

		Vector2 shotTarget = transform.position + playerMov.facingCoordinates * 100f;
		//Vector2 shootTarget = (transform.position + playerMov.facingCoordinates) * 100f;
		projectile.transform.LookAt (shotTarget);
		projectile.rigidbody2D.velocity = projectile.transform.forward * 25f;
		//projectile.transform.rotation = Quaternion.identity;
		//projectile.rigidbody2D.AddForce (shootTarget * 500f);

		Destroy (projectile, 2f);
	}
}
