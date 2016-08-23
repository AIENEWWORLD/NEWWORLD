using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ToBeSpawned;
    public int MaxNumberToBeSpawned;


    private float t_Time = 0.0f;
    private Vector3 StartPosition = new Vector3();

	void Start ()
    {
        StartPosition = gameObject.transform.position;
	}
	
	void Update ()
    {
	    
	}
}
