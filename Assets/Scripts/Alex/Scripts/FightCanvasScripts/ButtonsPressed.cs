using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonsPressed : MonoBehaviour
{
    GameObject fCam;
    public Button FlipButton;
    public Button InventoryButton;
    public Button FleeButton;
	// Use this for initialization
	void Start ()
    {
        fCam = GameObject.FindGameObjectWithTag("FightCamera");

    }

    // Update is called once per frame
    void Update()
    {
        if (fCam != null)
        {

        }
    }
    public void onClickFlee()
    {
        if (fCam.GetComponent<SetupFight>().enemyStats.guyType != StatsScript.enumType.boss)//remember to add all the boss names
        {
            if (fCam.GetComponent<SetupFight>().playerAttacks == true)
            {
                if (getRandom(fCam.GetComponent<SetupFight>().playerFleeRate, 0, 100))
                {
                    //have the enemy attack you without your coins working
                    endcombat();
                    fCam.GetComponent<Camera>().enabled = false;
                    gameObject.GetComponent<SetupFight>().playerinCombat = false;
                }
                else
                {
                    if (fCam != null)
                    {
                        if (fCam.GetComponent<SetupFight>().playerinCombat == true)
                        {
                            fCam.GetComponent<SetupFight>().playerAttacks = false;
                        }
                        if (fCam.GetComponent<SetupFight>().playerinCombat == true)
                        {
                            fCam.GetComponent<SetupFight>().calcFight = true;
                            SetInteractable(false);
                        }
                    }
                }
            }
        }

    }

    public void endcombat()
    {
        if (GameObject.FindGameObjectWithTag("CanvasEnemy") != null)
        {
            destroysprite();
            if (fCam != null)
            {
                //fCam.GetComponent<Camera>().enabled = false;
                fCam.GetComponent<SetupFight>().onExitCombat();
            }
            else
            {
                Debug.Log("FightCamera = null");
            }
        }
        gameObject.GetComponent<EnemyDropCoins>().dead = false;
    }

    bool getRandom(int chance, int rangeMin, int rangeMax)//returns true if random number is higher
    {
        return (Random.Range(rangeMin, rangeMax) > rangeMax - chance);
    }

    public void onClickAttack()
    {
        if (fCam != null)
        {
            fCam.GetComponent<SetupFight>().calcFight = true;
            SetInteractable(false);
        }
    }

    public void SetInteractable(bool a)
    {
        FlipButton.interactable = a;
        InventoryButton.interactable = a;
        FleeButton.interactable = a;
    }

    public void destroysprite()
    {
        if (GameObject.FindGameObjectWithTag("CanvasEnemy") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("CanvasEnemy"));
        }
        else
        {
            Debug.Log("CanvasEnemy = null");
        }
    }
}
