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
    float ZoomDistance = 0;

    Camera mapCamera;
    GameObject InputGameobject;
    float xVelocity;
    float zVelocity;

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

            transform.position += new Vector3(xVelocity * ScrollSpeed * Time.deltaTime, ZoomDistance, zVelocity * ScrollSpeed* Time.deltaTime);

       }


    }
}

