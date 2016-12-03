using UnityEngine;
using System.Collections;

public class MainMapController : MonoBehaviour
{
    public float ScrollSpeed;
    [HideInInspector]
    public GameObject m_player;

    Vector3 StartPositionOfCamera;

    bool mapEnabled = false;
    float CameraYDistance = 0;
    public float ZoomDistance = 0;

    Camera mapCamera;
    GameObject InputGameobject;
    float xVelocity;
    float zVelocity;

    public float zoomMax;
    public float zoomMin;

    // Use this for initialization
    void Start()
    {
        StartPositionOfCamera = gameObject.transform.position;
        m_player = GameObject.FindGameObjectWithTag("Player");
        mapCamera = gameObject.GetComponent<Camera>();
        CameraYDistance = mapCamera.transform.position.y;
        ZoomDistance = CameraYDistance;
        InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes");
    }

    // Update is called once per frame
    void Update()
    {
        if (mapCamera.enabled == false)
        {
            transform.position = new Vector3(m_player.transform.position.x, CameraYDistance, m_player.transform.position.z);
        }
        else if (mapCamera.enabled == true)
        {
            ZoomDistance = Input.GetAxis("Mouse ScrollWheel");
            ZoomDistance = ZoomDistance * -100;


            xVelocity = InputGameobject.GetComponent<SavedInput>().horizontal;
            zVelocity = InputGameobject.GetComponent<SavedInput>().vertical;

            if (mapCamera.transform.position.y > zoomMax && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ZoomDistance = 0;
            }
            else if (mapCamera.transform.position.y < zoomMin && Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ZoomDistance = 0;
            }

            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

            transform.position += new Vector3(xVelocity * ScrollSpeed * Time.deltaTime, ZoomDistance, zVelocity * ScrollSpeed* Time.deltaTime);
           
       }


    }
}

