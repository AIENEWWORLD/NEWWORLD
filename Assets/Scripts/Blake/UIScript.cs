using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	public Text supply;

	public Text health;
	public GameObject m_player;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        supply.text = "Supply: " + (int)m_player.GetComponent<StatsScript>().supplies;
	
		health.text = "Health: " + (int)m_player.GetComponent<StatsScript>().health;

    }
}
