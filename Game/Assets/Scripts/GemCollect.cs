using UnityEngine;
using System.Collections;

public class GemCollect : MonoBehaviour {

	public float offset = 10f;

	private Transform carry;
	private bool collected = false;

	void LateUpdate ()
	{
		if (collected)
		{
			transform.localPosition = carry.localPosition + (Vector3.up * (offset / 50));
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && !collected)
		{
			GameObject player =	coll.gameObject;

			PlayerMovement pm = player.GetComponent<PlayerMovement>();
			pm.hasGem = true;
			pm.collectedGem = this.gameObject;
			pm.NotifGemCollect();

			carry = player.transform;
			collected = true;
		}
	}
}
