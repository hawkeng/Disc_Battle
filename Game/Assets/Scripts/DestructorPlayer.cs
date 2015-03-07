using UnityEngine;
using System.Collections;

public class DestructorPlayer : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D objeto){
		if(objeto.tag == "Player"){
			NotificationCenter.DefaultCenter().PostNotification(this, "PersonajeHaMuerto");
			//Debug.Break();
			GameObject personaje = GameObject.Find("Player");
			personaje.SetActive(false);

		}
	}
}
