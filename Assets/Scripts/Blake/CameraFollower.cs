using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour
{
    //[HideInInspector]
    //public GameObject PlayerObject;
    //public float StepSpeed;
    //public Vector3 CameraOffSet;


    //[HideInInspector]
    //public Transform CameraTransform;
    //[HideInInspector]
    ////  public Quaternion CameraStartRotation = new Quaternion();

    //private Vector3 m_CameraTargetPosition = new Vector3();


    //// Use this for initialization
    //void Start()
    //{
    //    PlayerObject = GameObject.FindGameObjectWithTag("Player");         
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Vector3 CurrentCamPos = transform.position;
    //    m_CameraTargetPosition = Vector3.Lerp(CurrentCamPos, PlayerObject.transform.position, StepSpeed * Time.deltaTime);
    //    Vector3 FinalTransform = new Vector3(m_CameraTargetPosition.x, CameraOffSet.y, m_CameraTargetPosition.z);
    //    transform.position = FinalTransform;
     
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
         
            transform.RotateAround(GameObject.FindGameObjectWithTag("Player").transform.position, Vector3.up, 90);
        }
    }
}
