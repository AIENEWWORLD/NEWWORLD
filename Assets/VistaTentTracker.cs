using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VistaTentTracker : MonoBehaviour

{
    public GameObject StartingTent;

    [HideInInspector]
    public GameObject PlayerObject;
    [HideInInspector]
    public GameObject NearestTent;
    [HideInInspector]
    public List<GameObject> allTents;
    [HideInInspector]
    //  public List<GameObject> allVistas;

   public void FindNearestTent()
    {
        float distanceX;
        float distanceZ;
        float actualDistance = 0;
        float currentDistance = 0;
        foreach (GameObject itr in allTents)
        {
          
            GameObject rRef = itr;
            distanceX = rRef.transform.position.x - PlayerObject.transform.position.x;
            distanceZ = rRef.transform.position.z - PlayerObject.transform.position.z;
            // abs transform for nearest
            currentDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
            distanceX = NearestTent.transform.position.x - PlayerObject.transform.position.x;
            distanceZ = NearestTent.transform.position.z - PlayerObject.transform.position.z;
            actualDistance = Mathf.Abs(distanceX) + Mathf.Abs(distanceZ);
            if (currentDistance <= actualDistance)
            {
                actualDistance = currentDistance;
                NearestTent = itr;
                GameObject Compass = GameObject.FindGameObjectWithTag("Compass");
                Compass.GetComponent<CompassScript>().ClosestSupplyTent = NearestTent;

            }
        }
    }

    void Start ()
    {
        allTents = new List<GameObject>();
        allTents.Add(StartingTent);
        NearestTent = StartingTent;
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        
     

        //allVistas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Vista"));

        InvokeRepeating("FindNearestTent", 2.0f, 1.0f);
    }
	

}
