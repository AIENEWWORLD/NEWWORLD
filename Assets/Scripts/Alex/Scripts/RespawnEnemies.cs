using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnEnemies : MonoBehaviour
{
    GameObject player;

    public float distAway = 20;

    public float dist;

    //[HideInInspector]
    public List<GameObject> EnemyList = new List<GameObject>();


    public List<Transform> EnemyposList = new List<Transform>();

    GameObject[] respawnGameObjects;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        
        //for (int i = 0; i < EnemyList.Count; i++) //this startup is run after the statsscript
        //{
        //    EnemyposList.Add(EnemyList[i].gameObject.transform);
        //}

        /*
         * to save:
         * setup a reference to the enemy "statsScript" on startup.
         * get the transform positions of the enemies.
         * in the stats script, when the enemy dies set its tag to "respawning"
         * if "respawning" set the gameobject to inactive, reset it's statsscript
         * check if the player is far enough away from it's original transform position
         * if so, set the inactive object back to active.
         */
	}
	
	void Update ()
    {
        if (EnemyList.Count != 0)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                dist = Vector3.Distance(EnemyList[i].transform.position, player.transform.position);
                //Debug.Log(dist);
                if (dist > distAway)
                {
                    EnemyList[i].SetActive(true);
                    EnemyList.Remove(EnemyList[i]);
                }
            }
        }
	}                                                                         
}
