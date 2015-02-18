using UnityEngine;
using System.Collections;

public class EnemyHit : HitMechanics {

	void Start () 
	{
		canShoot = true;
	}

	void Update ()
	{
		timer += Time.deltaTime;
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
