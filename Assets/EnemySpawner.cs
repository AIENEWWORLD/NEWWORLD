using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
//    public GameObject ToBeSpawned;
//    public int MaxNumberToBeSpawned;
//    public float RandomSpotOffset;
//    public float SpawnTimeLimit;
//    private float t_Time = 0.0f;
//    private Vector3 StartPosition = new Vector3();
//    //Current number spawned needs to be deincremented upon monster death/despawn
//    private int CurrentNumberSpawned;
//
//	void Start ()
//    {
//        StartPosition = gameObject.transform.position;
//	}
//
//    void Update()
//    {
//        t_Time = t_Time + Time.deltaTime;
//
//        if(t_Time > SpawnTimeLimit && MaxNumberToBeSpawned < CurrentNumberSpawned)
//        {
//            Vector3 RandomOffset = new Vector3(Random.Range(-RandomSpotOffset, RandomSpotOffset), 0.0f, Random.Range(-RandomSpotOffset, RandomSpotOffset));
//            CurrentNumberSpawned = CurrentNumberSpawned + 1;
//            Transform ph_Parent = gameObject.transform;
//            ph_Parent.transform.position += RandomOffset;
//            Instantiate(ToBeSpawned, ph_Parent);
//            
//            SpawnTimeLimit = Random.Range(SpawnTimeLimit / 2, SpawnTimeLimit * 2);
//            t_Time = 0.0f;
//        }
//
//
//    }
}
