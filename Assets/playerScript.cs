using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

    public GameObject player;
    private Rigidbody playerRB;
    public GameObject dirLight; //для получения размерности массива
    public float speedPlayer;
    private Vector3 finishPoz;

    private int goIdPosition;
    private float widthPlate;
    public float areaCenterPercentPlate; //% от центра пликти, когда можно считать, что игрок дошёл до плитки

	// Use this for initialization
	void Start () {
        //player = GetComponent<GameObject>();
        playerRB = player.GetComponent<Rigidbody>();
        //nextPosition();
    }

    // Update is called once per frame
    void Update () {

        //если перснонаж находится правее или левее уровня клетки, то двигаем по оси Х
        if (transform.position.x <= finishPoz.x - widthPlate / areaCenterPercentPlate
            || transform.position.x >= finishPoz.x + widthPlate / areaCenterPercentPlate
            )
        {
            playerRB.velocity = new Vector3(finishPoz.x - transform.position.x, 0, 0) * speedPlayer;
        }
        //если перснонаж находится ниже или выше уровня клетки, то двигаем по оси Z
        if (
            transform.position.z <= finishPoz.z - widthPlate / areaCenterPercentPlate
            || transform.position.z >= finishPoz.z + widthPlate / areaCenterPercentPlate
            )
        {
            playerRB.velocity = new Vector3(0, 0, finishPoz.z - transform.position.z) * speedPlayer;
        }

        //если персонаж находится на плитки, а именно в areaCenterPercentPlate % от центра плитки, то считаем что персонаж дошёл до места
        if (
            transform.position.x >= finishPoz.x- widthPlate/ areaCenterPercentPlate 
            && transform.position.x <= finishPoz.x + widthPlate / areaCenterPercentPlate
            && transform.position.z >= finishPoz.z - widthPlate /areaCenterPercentPlate 
            && transform.position.z <= finishPoz.z + widthPlate / areaCenterPercentPlate
            )
        {
            dirLight.GetComponent<DrawPole>().pole[goIdPosition].GetComponent<Renderer>().material.color = Color.white;
            getNextPosition();
        }
    }
    private void FixedUpdate()
    {
    }

    public void getNextPosition()
    {
        int sizePole = dirLight.GetComponent<DrawPole>().pole.Length;
        goIdPosition = Random.Range(0, sizePole-1);
        dirLight.GetComponent<DrawPole>().pole[goIdPosition].GetComponent<Renderer>().material.color = Color.yellow;
        finishPoz = dirLight.GetComponent<DrawPole>().pole[goIdPosition].GetComponent<Renderer>().transform.position;
        widthPlate = dirLight.GetComponent<DrawPole>().pole[goIdPosition].GetComponent<Renderer>().transform.localScale.x;


        Debug.Log("nextPosition="+ goIdPosition);
        Debug.Log("finishPoz=" + finishPoz);

        Debug.Log("finish playerScript-getNextPosition");

    }
}
