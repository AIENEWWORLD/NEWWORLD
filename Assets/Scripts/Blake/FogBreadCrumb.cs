using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FogOfWarUnit))]
public class FogBreadCrumb : MonoBehaviour
{
    //Requires that this is pointing to main player object
    //Breadcrumbs used as placeholder to control maintained sight after player death
    public GameObject MainPlayer;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Breadcrumb")
        {
            //Add cleanups
            print("interaction");
        }
    }
	void Start ()
    {
        //Insures that the breadcrumb radius matchy matches with player radius at time of breadcrumb drop
       gameObject.GetComponent<FogOfWarUnit>().radius = MainPlayer.GetComponent<ControlScript>().visionRadius;

	}
	
}
