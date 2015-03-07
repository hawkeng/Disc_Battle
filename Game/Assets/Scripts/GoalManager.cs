using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {

	public GameObject homePlayer;
	public Vector2 startPoint;
	public TextMesh maracador;
	public GameObject camaraWin;
	public GameObject camaraLose;
	private ScoreManager scoreMan;
	private Animator anim;
	private int goles = 0;

	void Start () 
	{
		//scoreMan = GameObject.Find("GameManager").GetComponent<ScoreManager> ();
		scoreMan = GetComponentInParent<ScoreManager> ();
		//anim = GetComponentInChildren<Animator> ();
		anim = GetComponentInParent<Animator> ();
	}

	void OnTriggerStay2D (Collider2D coll) 
	{
		if (coll.gameObject == homePlayer)
		{
			if (homePlayer.GetComponent<PlayerMovement> ().hasGem)
			{

				if(homePlayer.name == "Player"){
					if(goles <5){
					goles+=1;
					maracador.text = goles.ToString();
					}else{
						camaraWin.SetActive(true);

					}
				}else if(homePlayer.name == "Enemy"){
					if(goles <5){
						goles+=1;
						maracador.text = goles.ToString();
					}else{
						camaraLose.SetActive(true);
					}
				}




				anim.SetTrigger ("Goal");
				PlayerMovement pm = homePlayer.GetComponent<PlayerMovement> ();
				pm.hasGem = false;

				Destroy (pm.collectedGem);
				pm.collectedGem = null;

				StartCoroutine(scoreMan.ScoreAndReset());
			}
		}
	}
}
