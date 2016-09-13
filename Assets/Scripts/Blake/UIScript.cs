using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	public Text exhaustion;
	public Text ammo;
	public Text health;
	public ControlScript m_player;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		exhaustion.text = "Supply: " + (int)m_player.supplyAmount;
	
		health.text = "Health: " + (int)m_player.playerHealth;

	}
}
