using UnityEngine;
using System.Collections;

public class LevelInfinity : MonoBehaviour {
	

	void OnMouseDown(){
		Camera.main.audio.Stop ();
		audio.Play ();
		Invoke ("LoadLevelInfinity", audio.clip.length);
	}

	void LoadLevelInfinity(){
		Application.LoadLevel ("InifiniteStage01");
	}
}
