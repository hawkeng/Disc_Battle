using UnityEngine;
using System.Collections;

public class BotonReturn : MonoBehaviour {

	public GameObject camaraLevel;
	public GameObject camaraInicio;

	void OnMouseDown(){
		audio.Play ();
		Invoke ("ReturnMenu", audio.clip.length);
	}

	void ReturnMenu(){
		camaraLevel.SetActive (false);
		camaraInicio.SetActive (true);
	}
}
