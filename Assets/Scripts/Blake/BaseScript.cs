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
			ControlScript thisPlayer = collision.gameObject.GetComponentInParent<ControlScript> ();
			thisPlayer.supplyAmount = thisPlayer.maxSupply;
			thisPlayer.currentAmmo = thisPlayer.maxAmmo;
			thisPlayer.playerHealth = thisPlayer.maxHealth;
		}
	}
}
