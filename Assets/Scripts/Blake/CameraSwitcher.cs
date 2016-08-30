using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MapCamera;
    public Camera FightCamera;

    public float ButtonToggleLimit;
    [HideInInspector]
    public bool MainCameraActive = true;
    private float m_lTime = 0;

    private Camera m_MapCameraStart = new Camera();
    private Camera m_MainCameraStart = new Camera();

    private Rect FullRect = new Rect(0, 0, 1, 1);
    private Rect MiniMapRect;
    void Start()
    {
      
        MiniMapRect = new Rect(MapCamera.rect);
    }
    // Update is called once per frame
    void Update()
    {
        m_lTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FightCamera.enabled = false;
        }


            if (m_lTime > ButtonToggleLimit && Input.GetButton("ToggleMap") == true)
        {
            if (MainCameraActive == true)
            {
                MapCamera.rect = FullRect;
                MapCamera.fieldOfView = 140.0f;
                MainCameraActive = false;
            }
            else
            {
                MapCamera.rect = MiniMapRect;
                MapCamera.fieldOfView = 45.0f;
                MainCameraActive = true;
            }
            m_lTime = 0.0f;
        }

    }
}
