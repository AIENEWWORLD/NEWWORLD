using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonsPressed : MonoBehaviour
{
    GameObject fCam;
    public Button FlipButton;
    public Button InventoryButton;
    public Button FleeButton;

    public bool CanFlee = false;
    public bool CanAttack = false;
    [HideInInspector]
    public bool CanInventory = false;
    public bool clicktocontinue;
	// Use this for initialization
	void Start ()
    {
        fCam = GameObject.FindGameObjectWithTag("FightCamera");

    }

    public void setButtons(bool attack, bool flee, bool inventory, bool clicktoCont)
    {
        CanFlee = flee;
        CanAttack = attack;
        CanInventory = inventory;
        clicktocontinue = clicktoCont;
        Debug.Log("s");
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
        //if (CanFlee == true && clicktocontinue == false)
        //{
        //    if (GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>() != null)
        //    {
        //        GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>().currentFrame++;
        //    }
        //    if (fCam.GetComponent<SetupFight>().enemyStats.guyType != StatsScript.enumType.boss)//remember to add all the boss names
        //    {
        //        if (fCam.GetComponent<SetupFight>().playerAttacks == true)
        //        {
        //            if (getRandom(fCam.GetComponent<SetupFight>().playerFleeRate, 0, 100))
        //            {
        //                //have the enemy attack you without your coins working
        //                endcombat();
        //                fCam.GetComponent<Camera>().enabled = false;
        //
        //                Transitions T = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<Transitions>();
        //                T.TransCam = Camera.main;
        //                T.trans = true;
        //
        //                gameObject.GetComponent<SetupFight>().playerinCombat = false;
        //            }
        //            else
        //            {
        //                if (fCam != null)
        //                {
        //                    if (fCam.GetComponent<SetupFight>().playerinCombat == true)
        //                    {
        //                        fCam.GetComponent<SetupFight>().playerAttacks = false;
        //                    }
        //                    if (fCam.GetComponent<SetupFight>().playerinCombat == true)
        //                    {
        //                        fCam.GetComponent<SetupFight>().calcFight = true;
        //                        SetInteractable(false);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public void endcombat()
    {
         Destroy(fCam.GetComponent<SetupFight>().enemySprite);
         if (fCam != null)
         {
             //fCam.GetComponent<Camera>().enabled = false;
             fCam.GetComponent<SetupFight>().onExitCombat();
             //Debug.Log("clearing");
         }
         else
         {
             Debug.Log("FightCamera = null");
         }
        
        gameObject.GetComponent<EnemyDropCoins>().dead = false;
    }

    bool getRandom(int chance, int rangeMin, int rangeMax)//returns true if random number is higher
    {
        return (Random.Range(rangeMin, rangeMax) > rangeMax - chance);
    }

    public void onClickAttack()
    {
        //Debug.Log("1");
        if (fCam != null)
        {
            if (CanAttack == true && clicktocontinue == false && fCam.GetComponent<SetupFight>().enemyStats != null)
            {
                if (GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>() != null)
                {
                    GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>().currentFrame++;
                }
                //Debug.Log("2");
                fCam.GetComponent<SetupFight>().calcFight = true;
                SetInteractable(false);
            }
        }
    }

    public void SetInteractable(bool a)
    {
        FlipButton.interactable = a;
        InventoryButton.interactable = a;
        FleeButton.interactable = a;
        
    }
}