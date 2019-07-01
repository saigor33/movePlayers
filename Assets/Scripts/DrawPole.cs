using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPole : MonoBehaviour {

    public GameObject plate;
    public GameObject[] pole;

	// Use this for initialization
	void Start () {
        // cube = Instantiate()
        //cube.GetComponent<Renderer>().material.color = Color.blue;

        int count_obj = Random.Range(5, 10);
        Debug.Log("Размер стороны(count_obj) ="+ count_obj);

        pole = new GameObject[count_obj * count_obj];
        float posX = plate.transform.localScale.x + plate.transform.localScale.x / 50;
       // cube.
        float posZ = 0; // cube.transform.localScale.z + cube.transform.localScale.z / 50;
        float pozY = 0;
        for (int i=0; i< pole.Length; i++)
        {
            if (i % count_obj == 0)
            {
                posZ += plate.transform.localScale.z + plate.transform.localScale.z / 50;
            }
            //pole[i]=Instantiate()
            pole[i] = Instantiate(plate, new Vector3( posX*(i% count_obj), plate.transform.position.y, posZ),
                Quaternion.identity) as GameObject;
            //pole[i].GetComponent<Renderer>().material.color = Color.red;
            //cube.
            pole[i].GetComponent<ListenerOnPlate>().id = i;
            pole[i].name = "plate" + i.ToString();
        }
        GameObject.Find("player").GetComponent<playerScript>().getNextPosition();
        Debug.Log("finish DrawPole");
    }
	// Update is called once per frame
	void Update () {
	 
	}
}
