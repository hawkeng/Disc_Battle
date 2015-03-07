using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	private int goalPlayer = 0;
	private int goalEnemi = 0;
	// Use this for initialization

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "GoalRight") {
			goalPlayer+=1;
				Debug.Log ("Player :" + goalPlayer);
		} else {
			goalEnemi+=1;
				Debug.Log ("Enemi :"+goalEnemi);
		}
	}
}
