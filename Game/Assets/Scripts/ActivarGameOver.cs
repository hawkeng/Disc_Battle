using UnityEngine;
using System.Collections;

public class ActivarGameOver : MonoBehaviour {

	public GameObject camaraGameOver;

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "PersonajeHaMuerto");
	}
	
	void PersonajeHaMuerto(Notification notificacion){
		camaraGameOver.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
