using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float smoothing = 5f;
	public Transform target;

	private Vector3 offset;

	void Start () 
	{
		offset = transform.position - target.position;
	}
	
	void Update () 
	{
		Vector3 nextPosition = target.position + offset;
		transform.position = Vector3.Lerp 
			(transform.position, nextPosition, smoothing * Time.deltaTime);
	}
}
