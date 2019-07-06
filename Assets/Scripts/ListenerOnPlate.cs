using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenerOnPlate : MonoBehaviour {
    public GameObject cube;
    private GameObject cubeDraw;
    public bool statusActiv = true;
    public int id;
    public bool statusBlock = false;
    //private  DrawPole drawPole; //для получения массива активных плиток

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.color = Color.blue;
        //cube.GetComponent<Renderer>().material.color = Color.black;
        // drawPole =   GameObject.Find("DirectionalLight").GetComponent<DrawPole>();
        //GameObject.Find("DirectionalLight").GetComponent<DrawPole>().gameFieldActive.Add(id);
        statusActiv = false;
    }

    // Update is called once per frame
    void Update () {


    }
    
    private void OnMouseOver()
    {
        //нажатие правой клавиши
        if (Input.GetMouseButtonDown(1))
        {
            //print(" start:clicl(" + statusActiv + ") =" + id);
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

            // print("fin: clicl("+ statusActiv + ") =" + id);
            // print( GameObject.Find("DirectionalLight").GetComponent<DrawPole>().testPole());
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
                //cubeDraw.GetComponent<PhysicMaterial>().bounciness = 0;
                cubeDraw.GetComponent<DestroyCub>().cubeID = id;

            }
            else
            {
                 Destroy(cubeDraw);
                //cube.SetActive(false);
            }
            GameObject.Find("DirectionalLight").GetComponent<DrawPole>().needRecalculation = true; //ставим флаг, что требуется перерасчёт пути
        }

    }

    private void OnMouseDown()
    {
       /* if (Input.GetKey(KeyCode.Mouse0))
        {
            statusActiv = !statusActiv;
            if (statusActiv)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }*/
    }

}
