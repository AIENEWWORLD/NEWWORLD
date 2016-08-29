using UnityEngine;
using System.Collections;

public class CreateInventory : MonoBehaviour
{

    public GameObject Items;
    int _x = -260; int _y = 180;

	// Use this for initialization
	void Start ()
    {

        //to-do load enemy coins which are preset on enemy at top of screen, load name + description on each coin then display the text.
        //load coin sprites, save selected coins into list of players coins, 

        //load in player inventory
	    for(int x = 0; x < 5; x++)
        {
            for(int y = 0; y < 10; y++)
            {
                GameObject item = Instantiate(Items);
                //item.transform.parent = gameObject.transform;
                item.transform.SetParent(gameObject.transform);
                item.transform.localScale = new Vector3(1, 1, 1);
                item.transform.localPosition = new Vector3(_x,_y, 0);
                _x += 60;
                if(y == 9)
                {
                    _x = -260;
                    _y -= 60;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
