using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RespawnPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject PlayerObject;

    [HideInInspector]
    public GameObject nearestRespawnPoint;

    [HideInInspector]
    public List<GameObject> allRespawnPoints;

    public GameObject StartingPoint;

    //Invoke repeating
   public void FindNearestRespawn()
    {

        float distanceX;
        float distanceZ;
        float actualDistance = 0;
        float currentDistance = 0;
        foreach (GameObject itr in allRespawnPoints)
        {
            GameObject rRef = itr;
            distanceX = rRef.transform.position.x - PlayerObject.transform.position.x;
            distanceZ = rRef.transform.position.z - PlayerObject.transform.position.z;
            // abs transform for nearest
            currentDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
            distanceX = nearestRespawnPoint.transform.position.x - PlayerObject.transform.position.x;
            distanceZ = nearestRespawnPoint.transform.position.z - PlayerObject.transform.position.z;
            actualDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
            if (currentDistance < actualDistance)
            {
                actualDistance = currentDistance;
                nearestRespawnPoint = rRef;
            }
        }

        PlayerObject.transform.position = nearestRespawnPoint.transform.position;
    }

    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        allRespawnPoints.Add(StartingPoint);
        nearestRespawnPoint = StartingPoint;

        //  
    }

    void Update()
    {

    }


}
