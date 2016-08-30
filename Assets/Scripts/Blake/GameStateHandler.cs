using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameStateHandler : MonoBehaviour
{
    //Basic class to colate any relevant information that will needed to be saved or loaded.
    [HideInInspector]
    public float PlayerHealth = new float();
    [HideInInspector]
    public float PlayerSupply = new float();
    [HideInInspector]
    public Vector3 PlayerLocation = new Vector3();
    [HideInInspector]
    public Quaternion PlayerRotation = new Quaternion();

    //Things needing to be added: CurrentCoins, DiscoveredVistas, FogMap, Upgrades, etc..
    public void GetGameData()
    {
        GameObject h_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = h_PlayerRef.GetComponent<ControlScript>().playerHealth;
        PlayerSupply = h_PlayerRef.GetComponent<ControlScript>().supplyAmount;
        PlayerLocation = h_PlayerRef.transform.position;
        PlayerRotation = h_PlayerRef.transform.rotation;
    }

    public void SetGameData()
    {
        GameObject h_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        h_PlayerRef.GetComponent<ControlScript>().playerHealth = PlayerHealth;
        h_PlayerRef.GetComponent<ControlScript>().supplyAmount = PlayerSupply;
        h_PlayerRef.transform.position = PlayerLocation;
        h_PlayerRef.transform.rotation = PlayerRotation;
    }

}
