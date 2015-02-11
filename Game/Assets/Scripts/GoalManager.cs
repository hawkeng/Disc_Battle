using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {

	public string zoneName;
	public GameObject homePlayer;

	private ScoreManager scoreMan;
	private Animator anim;

	void Start () 
	{
		scoreMan = GetComponentInParent<ScoreManager> ();
		anim = GetComponentInParent<Animator> ();
	}

	void OnTriggerEnter2D (Collider2D coll) 
	{
		if (coll.gameObject == homePlayer)
		{
			if (homePlayer.GetComponent<PlayerMovement> ().hasGem)
			{
				anim.SetTrigger ("Goal");
				PlayerMovement pm = homePlayer.GetComponent<PlayerMovement> ();
				pm.hasGem = false;

				Destroy (pm.collectedGem);
				pm.collectedGem = null;

				StartCoroutine(scoreMan.ScoreAndReset(zoneName));
			}
		}
	}
}
