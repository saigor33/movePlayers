using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPole : MonoBehaviour {

    public GameObject plate;
    public GameObject[] gameField;
    public  List<int> gameFieldActive;
    public bool needRecalculation = true;

    // Use this for initialization
    void Start() {
        int count_obj = Random.Range(5, 6);
        Debug.Log("Размер стороны(count_obj) =" + count_obj);

        gameField = new GameObject[count_obj * count_obj];
        float posX = plate.transform.localScale.x + plate.transform.localScale.x / 50;
        float posZ = 0;
        for (int i = 0; i < gameField.Length; i++)
        {
            if (i % count_obj == 0)
            {
                posZ += plate.transform.localScale.z + plate.transform.localScale.z / 50;
            }
            gameField[i] = Instantiate(plate, new Vector3(posX * (i % count_obj), plate.transform.position.y, posZ),
                Quaternion.identity) as GameObject;
            gameField[i].GetComponent<ListenerOnPlate>().id = i;
           // gameField[i].GetComponent<PhysicMaterial>().bounciness = 0;
            gameField[i].name = "plate" + i.ToString();
        }
        GameObject.Find("player").GetComponent<playerScript>().getNextPosition();
        Debug.Log("finish DrawPole");
    }
    // Update is called once per frame
    void Update() {

    }

    public string testPole()
    {
        string str="";
        foreach (int i in gameFieldActive)
        {
            str += i + " ";
        }
        return str;
    }
    /*public   List<int>  getArr_gameFieldActive
    {
        get
        {
            return   gameFieldActive;
        }
    }
    public void addItemGameFieldActive(int idPlate)
    {
        gameFieldActive.Add(idPlate);
    }

    public void removeItemGameFieldActive(int idPlate)
    {
        gameFieldActive.Remove(idPlate);
    }*/

}
