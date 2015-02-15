using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	public GameObject GemPrefab;
	public GameObject ProjectilePrefab;

	private static PrefabManager instance;

	public static PrefabManager Instance 
	{
		get 
		{
			if (instance == null)
			{
				instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
			}

			return instance;
		}
	}
}
