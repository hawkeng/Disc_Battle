using UnityEngine;
using System.Collections;

public class HitMechanics : MonoBehaviour {

	public float hitRadius = 1.8f;
	public float hitForce = 2500f;
	public float timeBetweenHits = 1f;

	private bool isHitting = false;
	private LayerMask hitLayer;
	private float timer;

	void Start ()
	{
		// Get the player layer so we only care about hit the enemy
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
		HandleHitMechanics ();
	}

	void HandleHitMechanics ()
	{
		if (isHitting)
		{
			Debug.Log ("isHitting");
			if (timer >= timeBetweenHits)
			{
				RaycastHit2D[] beenHit = Physics2D.CircleCastAll (transform.position, hitRadius, Vector2.zero, Mathf.Infinity, hitLayer);
				for (int i = 0, len = beenHit.Length; i < len; i++)
				{
					GameObject hitObj = beenHit[i].transform.gameObject;
					if (hitObj.name == "Enemy")
					{
						PlayerMovement pm = hitObj.GetComponent<PlayerMovement> ();
						if (pm.hasGem)
						{
							GameObject gem = pm.collectedGem;
							pm.hasGem = false;
							gem.GetComponentInChildren<GemCollect> ().isCollected = false;
							pm.collectedGem = null;
							
							hitObj.GetComponent<EnemyMovement> ().LookForGem();
						}

						// Melee atack
						//hitObj.rigidbody2D.velocity = Vector2.zero;
						Debug.Log ("Adding Force");
						hitObj.rigidbody2D.AddForce ((hitObj.transform.position - transform.position) * hitForce);
					}
					else
					{
						Shoot ();
					}
				}
				timer = 0f;
			}
			isHitting = false;
		}
	}

	void Shoot ()
	{

	}
}
