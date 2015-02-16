using UnityEngine;
using System.Collections;

public class GemCollect : MonoBehaviour {

	public float offset = 10f;

	private EnemyMovement enemyMov;
	private Transform carry;
	private Transform parentTransform;
	private bool collected = false;

	void Start ()
	{
		enemyMov = GameObject.Find ("Enemy").GetComponent<EnemyMovement> ();

		parentTransform = transform.parent.transform;
	}

	void LateUpdate ()
	{
		if (collected)
		{
			parentTransform.position = carry.position + (Vector3.up * (offset / 50));
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && !collected)
		{
			GameObject player =	coll.gameObject;

			PlayerMovement pm = player.GetComponent<PlayerMovement> ();
			pm.hasGem = true;
			pm.collectedGem = transform.parent.gameObject;
			pm.NotifGemCollect ();

			if (player.name == "Player")
			{
				enemyMov.PlayerGotGem ();
			}

			carry = player.transform;
			isCollected = true;
			collider2D.enabled = false;
		}
	}

	public bool isCollected
	{
		get {return collected;}
		set {collected = value;}
	}
}
