using UnityEngine;
using System.Collections;

public class Level1vs1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown(){
		Camera.main.audio.Stop ();
		audio.Play ();
		Invoke ("LoadLevel", audio.clip.length);
	}

	void LoadLevel(){
		Application.LoadLevel ("Stage01");
	}
}
