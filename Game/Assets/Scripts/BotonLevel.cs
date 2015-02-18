using UnityEngine;
using System.Collections;

public class BotonLevel : MonoBehaviour {

	public GameObject camaraLevel;
	public GameObject camaraInicio;

	void OnMouseDown(){
		audio.Play ();
		Invoke ("ActivationLevel", audio.clip.length);
	}

	void ActivationLevel(){
		camaraLevel.SetActive (true);
		camaraInicio.SetActive (false);
	}
}