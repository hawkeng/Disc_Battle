using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	private GoalManager[] goalZones;

	void Start ()
	{
		goalZones = GetComponentsInChildren<GoalManager> ();
	}

	public IEnumerator ScoreAndReset (string zoneName)
	{
		GameObject player;
		// Reset players' position to their respective goal zones
		// and temporary disable their movement and shoot ability
		for (int i = 0, len = goalZones.Length; i < len; i++)
		{
			player = goalZones[i].homePlayer;

			player.GetComponent<PlayerMovement> ().enabled = false;
			//player.GetCom

			player.rigidbody2D.velocity = Vector2.zero;
			player.transform.position = goalZones[i].transform.position;
		}

		yield return new WaitForSeconds (2);

		Instantiate (PrefabManager.Instance.GemPrefab, (Vector3.up * 0.3f), transform.rotation);

		PlayerMovement pm;
		for (int i = 0, len = goalZones.Length; i < len; i++)
		{
			pm = goalZones[i].homePlayer.GetComponent<PlayerMovement> ();
			pm.enabled = true;
			pm.LookForGem();
		}
	}
}
