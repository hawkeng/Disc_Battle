using UnityEngine;
using System.Collections;

public class GemCollect : MonoBehaviour {

	public float offset = 10f;

	private EnemyMovement enemyMov;
	private Transform carry;
	private Transform parentTransform;
	private bool collected = false;
	private Vector2 prevPos;
	private Animator anim;

	void Start ()
	{
		enemyMov = GameObject.Find ("Enemy").GetComponent<EnemyMovement> ();

		anim = GetComponent<Animator>();

		parentTransform = transform.parent.transform;
		//prevPos = parentTransform.position;
	}

	void LateUpdate ()
	{
		if (collected)
		{
			parentTransform.position = carry.position + (Vector3.up * (offset / 50));
		}
		/*else
		{
			myTransform.position = prevPos;
		}

		prevPos = myTransform.position;*/
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && !collected)
		{
			GameObject player =	coll.gameObject;

			PlayerMovement pm = player.GetComponent<PlayerMovement> ();
			pm.hasGem = true;
			pm.collectedGem = gameObject;
			pm.NotifGemCollect ();

			if (player.name == "Player")
			{
				enemyMov.PlayerGotGem ();
			}

			carry = player.transform;
			isCollected = true;
		}
	}

	public bool isCollected
	{
		get {return collected;}
		set 
		{
			collected = value;
			//anim.SetBool ("isCollected", collected);
		}
	}
}
