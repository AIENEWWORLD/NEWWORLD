using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveLoad : MonoBehaviour
{
    GameStateHandler gs_Ref;

    void Save()
    {
        gs_Ref = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<GameStateHandler>();
        gs_Ref.GetGameData();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.gd");
        bf.Serialize(file, this.gs_Ref);
        file.Close();
    }


    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.gd"))
        {
            gs_Ref = new GameStateHandler();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.gd", FileMode.Open);
            this.gs_Ref = (GameStateHandler)bf.Deserialize(file);
            file.Close();

            GameStateHandler GameStateReference = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<GameStateHandler>();

            GameStateReference = gs_Ref;
            GameStateReference.SetGameData();

        }
    }

}
