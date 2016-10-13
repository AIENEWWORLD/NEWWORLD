using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MapCamera;
    //public Camera FightCamera;
    public GameObject Player;


    public float ButtonToggleLimit;
    [HideInInspector]
    public bool MainCameraActive = true;
    private float m_lTime = 0;

    private Camera m_MapCameraStart = new Camera();
    private Camera m_MainCameraStart = new Camera();

    private Rect FullRect = new Rect(0, 0, 1, 1);
    private Rect MiniMapRect;

    private CheckinCombatScript CheckCombat;
    void Start()
    {
        CheckCombat = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>();
        MiniMapRect = new Rect(MapCamera.rect);
    }
    // Update is called once per frame
    void Update()
    {
        m_lTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //FightCamera.enabled = false;
        }


            if (m_lTime > ButtonToggleLimit && Input.GetButton("ToggleMap") == true && CheckCombat.Combatisenabled == false && CheckCombat.Optionsisenabled == false)
        {
            if (MainCameraActive == true)
            {
                MapCamera.enabled = true;
                MainCamera.enabled = false;
                //   MapCamera.fieldOfView = 140.0f;
                MainCameraActive = false;
                Player.GetComponent<ControlScript>().p_SeizeMovement = true;
            }
            else
            {
               // MapCamera.rect = MiniMapRect;
              //  MapCamera.fieldOfView = 45.0f;
                MainCameraActive = true;
                MapCamera.enabled = false;
                MainCamera.enabled = true;
             Player.GetComponent<ControlScript>().p_SeizeMovement = false;
            }
            m_lTime = 0.0f;
        }

    }
}
