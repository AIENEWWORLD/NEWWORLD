using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{
    public Material Health;
    public Material Supplies;
    public StatsScript PlayerStats;
    public GameObject FlipObject;

    public GameObject LookAtGameOject;
    public GameObject PlayerObject;
    public GameObject Pointer;

    public GameObject Screw;
    public GameObject ClosestSupplyTent; //set me to the default closest supply tent

    public float smoothRot = 5;
    bool flip = false;

    public Text Coins;
    public Text Landmarks;
    public Text Beasts;

    void Start()
    {
        Health.SetFloat("_Cutoff", 0);
        Supplies.SetFloat("_Cutoff", 0);
    }

    void Update()
    {
        Health.SetFloat("_Cutoff", 1-PlayerStats.health/PlayerStats.maxHealth);
        Supplies.SetFloat("_Cutoff", 1-PlayerStats.supplies/PlayerStats.maxSupply);
        if (Input.GetKeyDown(KeyCode.T))
        {
            flip = !flip;
        }
        Coins.text = PlayerStats.gold.ToString();
        Landmarks.text = 0.ToString(); //WAT
        Beasts.text = 0.ToString();
    }

    void LateUpdate()
    {

        Quaternion dir;
        if (flip == true)
        {
            dir = Quaternion.Euler(-180, PlayerObject.transform.rotation.eulerAngles.y, 0);
        }
        else
        {
            dir = Quaternion.Euler(0, PlayerObject.transform.rotation.eulerAngles.y, 0);
            if (Quaternion.Angle(FlipObject.transform.rotation, dir) < 3)
            {
                Pointer.transform.rotation = Quaternion.Lerp(Pointer.transform.rotation, Quaternion.LookRotation((new Vector3(LookAtGameOject.transform.position.x, Pointer.transform.position.y, LookAtGameOject.transform.position.z) - new Vector3(PlayerObject.transform.position.x, Pointer.transform.position.y, PlayerObject.transform.position.z))), 100 * Time.smoothDeltaTime);
                Screw.transform.rotation = Quaternion.Lerp(Screw.transform.rotation, Quaternion.LookRotation((new Vector3(ClosestSupplyTent.transform.position.x, Screw.transform.position.y, ClosestSupplyTent.transform.position.z) - new Vector3(PlayerObject.transform.position.x, Screw.transform.position.y, PlayerObject.transform.position.z))), 100 * Time.smoothDeltaTime);
            }
        }
        FlipObject.transform.rotation = Quaternion.Lerp(FlipObject.transform.rotation, dir, smoothRot * Time.deltaTime);
    }

    void OnApplicationQuit() //reset the material cutoff
    {
        Health.SetFloat("_Cutoff", 0);
        Supplies.SetFloat("_Cutoff", 0);
    }
}