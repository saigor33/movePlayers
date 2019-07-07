using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenerOnPlate : MonoBehaviour {
    public GameObject cube;
    private GameObject cubeDraw;
    public bool statusActiv = true;
    public int id;
    public bool statusBlock = false;
    void Start () {
        GetComponent<Renderer>().material.color = Color.blue;
        statusActiv = false;
    }

    void Update () {


    }
    
    private void OnMouseOver()
    {
        //нажатие правой клавиши
        if (Input.GetMouseButtonDown(1))
        {
            statusActiv = !statusActiv;
            if (statusActiv)
            {
                GetComponent<Renderer>().material.color = Color.green;
                GameObject.Find("DirectionalLight").GetComponent<DrawPole>().gameFieldActive.Add(id);
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.blue;
                GameObject.Find("DirectionalLight").GetComponent<DrawPole>().gameFieldActive.Remove(id);
            }
            GameObject.Find("DirectionalLight").GetComponent<DrawPole>().needRecalculation = true ; //ставим флаг, что требуется перерасчёт пути

        }


        //нажатие левой клавиши
        if (Input.GetMouseButtonDown(0))
        {
            statusBlock = !statusBlock;
            if (statusBlock)
            {
                Vector3 posPlate = GetComponent<Renderer>().transform.position;
                cubeDraw = Instantiate(cube,new Vector3(posPlate.x, posPlate.y+cube.transform.localScale.y/2, posPlate.z),
                    Quaternion.identity) as GameObject;
                cubeDraw.GetComponent<DestroyCub>().cubeID = id;

            }
            else
            {
                 Destroy(cubeDraw);
            }
            GameObject.Find("DirectionalLight").GetComponent<DrawPole>().needRecalculation = true; //ставим флаг, что требуется перерасчёт пути
        }

    }
}
