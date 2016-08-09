using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MapCamera;
    public float ButtonToggleLimit;
    [HideInInspector]
    public bool MainCameraActive = true;
    private float m_lTime = 0;


    // Update is called once per frame
    void Update()
    {
        m_lTime += Time.deltaTime;

        if(m_lTime > ButtonToggleLimit && Input.GetButton("ToggleMap") == true)
        {
            if (MainCameraActive == true)
            {
                MainCamera.enabled = false;
                MapCamera.enabled = true;
                MainCameraActive = false;
            }
            else
            {
                MainCamera.enabled = true;
                MapCamera.enabled = false;
                MainCameraActive = true;
            }
            m_lTime = 0.0f;
        }

    }
}
