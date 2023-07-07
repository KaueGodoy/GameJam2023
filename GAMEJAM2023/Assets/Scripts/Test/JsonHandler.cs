using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log("JsonHandler.Start");

        // get player data

        //PlayerDataTest playerDataTest = new PlayerDataTest();

        //playerDataTest.position = new Vector3(5, 0);
        //playerDataTest.health = 100;

        // save data to json

        //string json = JsonUtility.ToJson(playerDataTest);
        //Debug.Log(json);

        // create save file using json data

        //File.WriteAllText(Application.dataPath + "/saveFile.json", json);

        // load saved json file

        //string json = File.ReadAllText(Application.dataPath + "/saveFile.json");

        //// change data reading json file

        //PlayerDataTest loadedPlayerData = JsonUtility.FromJson<PlayerDataTest>(json);
        //Debug.Log("position: " + loadedPlayerData.position);
        //Debug.Log("health: " + loadedPlayerData.health);


    }

    private class PlayerDataTest
    {
        public Vector3 position;
        public float health;
    }
}
