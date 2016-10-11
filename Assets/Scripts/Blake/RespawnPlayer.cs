using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour
{

    [HideInInspector]
    public GameObject nearestRespawnPoint;

    [HideInInspector]
    public GameObject[] allRespawnPoints;
    

    //Invoke repeating
    void FindNearestRespawn()
    {
        int CheckLength = allRespawnPoints.Length;
        float distanceX;
        float distanceZ;
        float actualDistance = 0;
        float currentDistance = 0;
        for (int itr = 0; itr <= CheckLength; itr++)
        {
            GameObject rRef = allRespawnPoints[itr];


            distanceX = rRef.transform.position.x - gameObject.transform.position.x;
            distanceZ = rRef.transform.position.z - gameObject.transform.position.z;
            // abs transform for nearest
            currentDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
            if (itr == 0)
            {
                nearestRespawnPoint = rRef;
                actualDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
                currentDistance = actualDistance;
            }

            if (currentDistance < actualDistance)
            {
                actualDistance = currentDistance;
                nearestRespawnPoint = rRef;
            }


        }
    }

    void Start()
    {

    }

    void Update()
    {

    }


}
