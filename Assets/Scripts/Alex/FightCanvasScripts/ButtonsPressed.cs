using UnityEngine;
using System.Collections;

public class ButtonsPressed : MonoBehaviour
{
    GameObject fCam;
	// Use this for initialization
	void Start ()
    {
        fCam = GameObject.FindGameObjectWithTag("FightCamera");

    }
	
	// Update is called once per frame
	void Update ()
    {
	
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
                }
                else
                {
                    fCam.GetComponent<SetupFight>().playerAttacks = false;
                    if (fCam != null)
                    {
                        fCam.GetComponent<SetupFight>().calcFight = true;
                    }
                }
            }
        }

    }

    public void endcombat()
    {
        destroysprite();
        if (fCam != null)
        {
            fCam.GetComponent<Camera>().enabled = false;
            fCam.GetComponent<SetupFight>().onExitCombat();
        }
        else
        {
            Debug.Log("FightCamera = null");
        }
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
        }
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
