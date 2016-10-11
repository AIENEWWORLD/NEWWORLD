using UnityEngine;
using System.Collections;

public class TentPlacement : MonoBehaviour

{
    public GameObject Tent;
    private bool m_Inside = false;
    private bool m_TentPlaced = false;

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == ("Player"))
        {
          
            m_Inside = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
      
            m_Inside = false;
        }
    }

    void Start()
    {
        //if (m_TentPlaced == true)
        //{
        //    Instantiate(Tent, transform.position, transform.rotation);
        //}
    }

    void Update()
    {


        if (Input.GetButtonDown("PlaceTent") == true && m_Inside == true && m_TentPlaced == false)
        {
            m_TentPlaced = true;
            Vector3 TentLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(Tent, TentLocation, transform.rotation);
            gameObject.GetComponent<Renderer>().enabled = false;
            GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().allRespawnPoints.Add(gameObject);
        }
    }
}
