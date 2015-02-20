using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public float speed = 25f;
	public HitMechanics mechanics {get; set;}

	void OnTriggerEnter2D (Collider2D coll)
	{
		bool isShootable = coll.gameObject.tag == "Shootable";
		bool hitEnemy = mechanics.HandleHitEnemy (mechanics.enemyName, coll.gameObject);

		if (hitEnemy || isShootable)
		{
			// mostrar animación de explosión de particulas
			Destroy (transform.parent.gameObject);
		}
	}
}
