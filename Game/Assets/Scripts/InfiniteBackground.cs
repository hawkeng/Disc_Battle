using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class InfiniteBackground : MonoBehaviour {

	private Camera cam;
	private float spriteWidth;
	private bool hasNextSegment = false;
	private Transform nextSegment;
	private float nextSegmentOffset;
	private Vector3 nextSegmentPos;
	private Transform currentSegment;

	void Awake ()
	{
		cam = Camera.main;
	}
		
	void Start () 
	{
		currentSegment = transform;

		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		spriteWidth = sr.bounds.size.x;
	
		// Set half the size of what the camera can see horizontally
		// 'orthographicSize' represents half the size of what the camera
		// can see vertically
		// In order to know the horizontal half size we apply a rule of 3:
		// Screen.heigth          --->   Screen.width
		// cam.orthographicSize   --->   ?
		nextSegmentOffset = cam.orthographicSize * Screen.width / Screen.height;
	}
	
	void Update () 
	{
		if (!hasNextSegment) 
		{
			// First we get the Background Sprite ending x position
			// Then we sustract the half of what the camera can see horizontally
			// This will tell us when to place the next segment
			float nextSegmentFlag = (currentSegment.position.x + spriteWidth/2) - nextSegmentOffset;

			// If we are in or passed the next segment flag and the next segment
			// has not been placed yet then place the next segment
			if (cam.transform.position.x >= nextSegmentFlag)
			{
				nextSegmentPos = currentSegment.position + (Vector3.right * spriteWidth);

				if (nextSegment == null)
				{
					//Debug.Log ("Instantiate()");
					nextSegment = Instantiate (transform, nextSegmentPos, transform.rotation) as Transform;
					nextSegment.GetComponent<InfiniteBackground>().enabled = false;
				}
				else 
				{
					//Debug.Log ("Position change: " + nextSegmentPos);
					nextSegment.position = nextSegmentPos;
				}
				//Debug.Log ("hasNextSegment = true");
				hasNextSegment = true;
			}
		}
		else 
		{
			// If we are currently over the last segment we need to tell when to move the past segment
			// forward. If the left edge of the current segment is in place with the camera left edge
			// then set the flag 'hasNextSegment' to false
			float lastSegmentPos = (currentSegment.position.x - spriteWidth/2) + nextSegmentOffset;
			if (cam.transform.position.x >= lastSegmentPos)
			{
				//Debug.Log ("hasNextSegment = false");
				hasNextSegment = false;
				Transform tmp = currentSegment;
				currentSegment = nextSegment;
				nextSegment = tmp;
			}
		}
	}
}
