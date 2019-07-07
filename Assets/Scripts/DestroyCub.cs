using UnityEngine;
using System.Collections;

public class DestroyCub : MonoBehaviour {


    public int cubeID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseDown()
    {
        if (GameObject.Find("DirectionalLight").GetComponent<DrawPole>().getLog) Debug.Log("Destroy cube=" + cubeID);
        GameObject.Find("plate" + cubeID).GetComponent<ListenerOnPlate>().statusBlock = false;
        Destroy(this.gameObject);
        GameObject.Find("DirectionalLight").GetComponent<DrawPole>().needRecalculation = true; //ставим флаг, что требуется перерасчёт пути

    }
}
