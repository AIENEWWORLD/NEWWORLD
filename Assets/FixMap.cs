using UnityEngine;
using System.Collections;

public class FixMap : MonoBehaviour
{
    public GameObject playerpos;
	// Use this for initialization
	void Start () {
        playerpos = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerpos.transform.position.x,transform.position.y, playerpos.transform.position.z);
	}
}
