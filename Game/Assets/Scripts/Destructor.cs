using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D objeto){
		if(objeto.tag == "Player"){
			Debug.Break();
		}else{
			Destroy(objeto.gameObject);
		}
	}


}
