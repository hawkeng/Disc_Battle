using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	public float velocidad = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update (){
		renderer.material.mainTextureOffset = new Vector2 (Time.time * velocidad, 0);

	}
}