using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPole : MonoBehaviour {

    public bool getLog = false;
    public GameObject plate;
    public GameObject[] gameField;
    public  List<int> gameFieldActive;
    public bool needRecalculation = true;
    public bool statusUserClickOnStop = false;

    public GameObject playerPrefab;
    public GameObject[] arrPlayers;

    // Use this for initialization
    void Start() {
        int count_obj = Random.Range(5, 10);
        if(getLog)Debug.Log("Размер стороны(count_obj) =" + count_obj);

        gameField = new GameObject[count_obj * count_obj];
        float posX = plate.transform.localScale.x+ plate.transform.localScale.x / 50; //
        float posZ = 0;

        float sizePoleX = count_obj* posX;
        float sizePoleZ = count_obj*(plate.transform.localScale.z + plate.transform.localScale.z/50);
        for (int i = 0; i < gameField.Length; i++)
        {
            if (i % count_obj == 0)
            {
                posZ += plate.transform.localScale.z+ plate.transform.localScale.z / 50; //
            }
            gameField[i] = Instantiate(plate, new Vector3(posX * (i % count_obj), plate.transform.position.y, posZ),
                Quaternion.identity) as GameObject;
            gameField[i].GetComponent<ListenerOnPlate>().id = i;
            gameField[i].name = "plate" + i.ToString();
        }

        GameObject.FindWithTag("MainCamera").transform.position = new Vector3(sizePoleX / 2f - sizePoleX / count_obj, sizePoleX*1.5f, sizePoleZ / 2f);
        int count_player = Random.Range(1, 5);
        arrPlayers = new GameObject[count_player];
        for (int i=0; i< count_player; i++)
        {
            arrPlayers[i] = Instantiate(playerPrefab, new Vector3(gameField[i].transform.position.x, playerPrefab.transform.position.y, gameField[i].transform.position.z), 
                Quaternion.identity) as GameObject;
           /// arrPlayers[i].GetComponent<playerScript>().getNextPosition();
        }
    }
    void Update() {

    }

    public void closeGame()
    {
        Application.Quit();
    }

    public void pauseGame()
    {
        statusUserClickOnStop = !statusUserClickOnStop;
        // if(getLog)Debug.Log("statusUserClickOnStop=" + statusUserClickOnStop.ToString());
        if (statusUserClickOnStop == false)
        {
            for (int i = 0; i < arrPlayers.Length; i++)
            {
                arrPlayers[i].GetComponent<playerScript>().getNextPosition();
                arrPlayers[i].name = "player" + i;
            }

        }
    }

}
