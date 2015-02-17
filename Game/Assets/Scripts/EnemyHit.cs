using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour {

	public float hitForce = 5000f;
	public float timeBetweenHits = 1f;
	
	private float moveH, moveV;
	private float timer;

	void Update ()
	{
		timer += Time.deltaTime;
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (timer >= timeBetweenHits)
		{
			GameObject hitObj = coll.gameObject;
			//bool playerIsTarget = GetComponentInParent<EnemyMovement> ().playerIsTarget;
			if (hitObj.name == "Player")
			{
				PlayerMovement pm = hitObj.GetComponent<PlayerMovement> ();
				if (pm.hasGem)
				{
					GameObject gem = pm.collectedGem;
					pm.hasGem = false;
					gem.GetComponentInChildren<GemCollect> ().isCollected = false;
					gem.GetComponentInChildren<CircleCollider2D> ().enabled = true;
					pm.collectedGem = null;

					transform.parent.GetComponent<EnemyMovement> ().LookForGem();
				}

				hitObj.rigidbody2D.velocity = Vector2.zero;
				hitObj.rigidbody2D.AddForce ((hitObj.transform.position - transform.position) * hitForce);
			}
			timer = 0f;
		}
	}
}
