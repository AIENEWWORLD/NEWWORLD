using UnityEngine;
using System.Collections;

public class BaseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            print("Yes");
			StatsScript thisPlayer = collision.gameObject.GetComponentInParent<ControlScript> ().statsScript;
            thisPlayer.supplies = thisPlayer.maxSupply;
            thisPlayer.health = thisPlayer.maxHealth;

        }
	}
}
