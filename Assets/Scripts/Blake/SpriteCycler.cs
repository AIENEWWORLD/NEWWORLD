using UnityEngine;
using System.Collections;

public class SpriteCycler : MonoBehaviour
{

    public Material LeftSprite;
    public Material RightSprite;

    Renderer ThisSprite;
      public GameObject PlayerObject;
    bool FacingDir;
    // Use this for initialization
    void Start()
    {
     
        ThisSprite = gameObject.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.position = PlayerObject.transform.position;

        if (PlayerObject.GetComponent<ControlScript>().FacingDirection == true)
        {
            ThisSprite.material = LeftSprite;
        }
        else if (PlayerObject.GetComponent<ControlScript>().FacingDirection == false)
        {
            ThisSprite.material = RightSprite;
        }
    }
}
