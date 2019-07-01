using UnityEngine;
using System.Collections;

public class ListenerOnPlate : MonoBehaviour {
    public GameObject cube;
    private GameObject cubeDraw;
    public bool statusActiv = true;
    public int id;
    public bool statusBlock = false;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.green;
        //cube.GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
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
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
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
                //cube.SetActive(false);
            }
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
