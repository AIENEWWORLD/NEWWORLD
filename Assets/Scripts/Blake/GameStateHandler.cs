using UnityEngine;
using System.Collections;

public class GameStateHandler : MonoBehaviour
{
    
    public float PlayerStartHealth;
    public float PlayerStartAPS;
    public float PlayerStartMoveSpeed;

    [HideInInspector]
    public float PlayerHealth;
    [HideInInspector]
    public float PlayerAPS;
    [HideInInspector]
    public float PlayerMoveSpeed;
    [HideInInspector]
    public float PercentageMapExplored;


    void Start()
    {
        PlayerHealth = PlayerStartHealth;
        PlayerAPS = PlayerStartAPS;
        PlayerMoveSpeed = PlayerStartMoveSpeed;
        PercentageMapExplored = 0.0f;
    }

   public void SetGameData()
    {
        GetComponent<ControlScript>().playerHealth = PlayerHealth;
    }

    public void GetGameData()
    {
        PlayerHealth = GetComponent<ControlScript>().playerHealth;
    }


}
