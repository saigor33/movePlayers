using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class playerScript : MonoBehaviour {

    public GameObject player;
    private Rigidbody playerRB;
    public GameObject dirLight; //для получения размерности массива
    public float speedPlayer;
    private Vector3 finishPoz;

    private int finishIdPoz;
    private float widthPlate;
    public float areaCenterPercentPlate; //% от центра пликти, когда можно считать, что игрок дошёл до плитки

    //постоянные скорости игрока, без замедления
    private float speedX;
    private float speedZ;
    private List<int> patch = new List<int>();

    // Use this for initialization
    void Start() {
        //player = GetComponent<GameObject>();
        playerRB = player.GetComponent<Rigidbody>();
        //nextPosition();
    }

    // Update is called once per frame
    void Update() {
        Vector3 nextPoz;
        //если нет активных плиток, проеверяем их появление каждый кадр
        if (finishIdPoz == -1)
        {
            playerRB.velocity = Vector3.zero;
            getNextPosition();
            return;
        }
        if (dirLight.GetComponent<DrawPole>().needRecalculation)
        {
            patch.Clear();
            patch = searchPatch(); //patch[patch.Count - 1]

            if (patch.Count == 0)
            {
                return;
            }
            else
            {
                dirLight.GetComponent<DrawPole>().needRecalculation = false; //устанавливаем флаг, что перерасчёт пути выполенен
            }
        }
        try
        {
            nextPoz = dirLight.GetComponent<DrawPole>().gameField[patch[patch.Count - 1]].transform.position;
        }
        catch 
        {
            Debug.Log("нет следующей цели. Остановка");
            return;
        }
        Debug.Log("patch[patch.Count - 1]=" + patch[patch.Count - 1] + " nextPoz =" + nextPoz.ToString());
        speedX = nextPoz.x - transform.position.x;
        speedZ = nextPoz.z - transform.position.z;
        //Debug.Log("speedZ=" + speedZ.ToString());

        //если перснонаж находится правее или левее уровня клетки, то двигаем по оси Х
        if (transform.position.x <= nextPoz.x - widthPlate / areaCenterPercentPlate
            || transform.position.x >= nextPoz.x + widthPlate / areaCenterPercentPlate
            )
        {
            playerRB.velocity = new Vector3(speedX, 0, 0) * speedPlayer;
        }
        //если перснонаж находится ниже или выше уровня клетки, то двигаем по оси Z
       if (
            transform.position.z <= nextPoz.z - widthPlate / areaCenterPercentPlate
            || transform.position.z >= nextPoz.z + widthPlate / areaCenterPercentPlate
            )
        {
            Debug.Log("Добро playerRB.velocity="+ playerRB.velocity.ToString());
            playerRB.velocity = new Vector3(0, 0, speedZ) * speedPlayer; // z = finishPoz.z - transform.position.z
        }

       //если игрок дошёл до клетки, удаляем её из списка
        if (
            transform.position.x >= nextPoz.x - widthPlate / areaCenterPercentPlate
            && transform.position.x <= nextPoz.x + widthPlate / areaCenterPercentPlate
            && transform.position.z >= nextPoz.z - widthPlate / areaCenterPercentPlate
            && transform.position.z <= nextPoz.z + widthPlate / areaCenterPercentPlate
            )
        {
            patch.Remove(patch[patch.Count - 1]);
            string str = "";
            for (int i=0; i<patch.Count; i++)
            {
                str += " " + patch[i];
            }
            playerRB.velocity = Vector3.zero;
            Debug.Log("delete true: "+str);
        }

        //если персонаж находится на финешной плитки, а именно в areaCenterPercentPlate % от центра плитки, то считаем что персонаж дошёл до места
        if (
            transform.position.x >= finishPoz.x - widthPlate / areaCenterPercentPlate
            && transform.position.x <= finishPoz.x + widthPlate / areaCenterPercentPlate
            && transform.position.z >= finishPoz.z - widthPlate / areaCenterPercentPlate
            && transform.position.z <= finishPoz.z + widthPlate / areaCenterPercentPlate
            )
        {
            dirLight.GetComponent<DrawPole>().gameField[finishIdPoz].GetComponent<Renderer>().material.color =
               dirLight.GetComponent<DrawPole>().gameField[finishIdPoz].GetComponent<ListenerOnPlate>().statusActiv
               ? Color.green : Color.blue;

            getNextPosition();
        }
    }
    private void FixedUpdate()
    {
    }

    public void getNextPosition()
    {

        //int sizePole = dirLight.GetComponent<DrawPole>().gameField.Length;
        List<int> arr_gameFieldActive = dirLight.GetComponent<DrawPole>().gameFieldActive;
        // print("---");
        /*foreach (int i in arr_gameFieldActive)
        {
            print(i);
        }*/
        // print("---");


        if (arr_gameFieldActive.Count>0)
        {
            finishIdPoz = arr_gameFieldActive[Random.Range(0, arr_gameFieldActive.Count)];
            GameObject[] arr_gameField = dirLight.GetComponent<DrawPole>().gameField;
            if (arr_gameField[finishIdPoz].GetComponent<ListenerOnPlate>().statusBlock == true)
            {
                finishIdPoz = -1; //-1 значит, что в массиве нет активных плиток, которые выбраны и незаняты кубом. Игрок будет ожидать активную плитку
                return;
            }
            arr_gameField[finishIdPoz].GetComponent<Renderer>().material.color = Color.yellow;
            finishPoz = arr_gameField[finishIdPoz].GetComponent<Renderer>().transform.position;
            widthPlate = arr_gameField[finishIdPoz].GetComponent<Renderer>().transform.localScale.x;
            GameObject.Find("DirectionalLight").GetComponent<DrawPole>().needRecalculation = true; //ставим флаг, что требуется перерасчёт пути
        }
        else
        {
            finishIdPoz = -1; //-1 значит, что в массиве нет активных плиток, которые выбраны и незаняты кубом. Игрок будет ожидать активную плитку
        }
        /*try
        {
        }
        catch (System.Exception ex)
        {
            // Debug.Log("нет активных платок"+ ex);
        }*/
        //Debug.Log("arr_gameFieldActive.Count="+ arr_gameFieldActive.Count);
        /*Debug.Log("nextPosition="+ goIdPosition);
        Debug.Log("finishPoz=" + finishPoz);

        Debug.Log("finish playerScript-getNextPosition");*/

    }

    private List<int> searchPatch()
    {
        //получаем массив (текущее состояние поля игры), где отображены плитки по которым нельзя ходить
        GameObject[] gameField = dirLight.GetComponent<DrawPole>().gameField;
        int lenghtPoleLine = (int)System.Math.Sqrt(gameField.Length);
        bool status = true;
        int step = 2;
        int goodPlate = 0;
        int startIndex = -1;
        List<int> path = new List<int>();

        int[] arrStatusPlates = new int[lenghtPoleLine * lenghtPoleLine];
        string str="";

        for (int i = 0; i < gameField.Length; i++)
        {
            if (gameField[i].GetComponent<ListenerOnPlate>().statusBlock==false)
                arrStatusPlates[i] = 0;
            else
                arrStatusPlates[i] = -1;

            Transform plateTransform = gameField[i].GetComponent<Renderer>().transform;
            if (
                transform.position.x >= (plateTransform.position.x - widthPlate /2)
                && transform.position.x <= (plateTransform.position.x + widthPlate / 2)
                && transform.position.z >= (plateTransform.position.z - widthPlate / 2)
                && transform.position.z <= (plateTransform.position.z + widthPlate / 2)
                )
            {
                // Debug.Log
                startIndex = i;
            }
            str +=" "+ arrStatusPlates[i];
        }
        Debug.Log("startIndex=" + startIndex+ "; str="+ str);
        if(startIndex==-1)
        {
            return path;
        }


        //отсчёт пути начинается с step=2.  
        //(-1)- нельзя пройти или встать; 
        //(0) - можно пройти; 
        //(1) - зарезервированно для  "можно встать"

        arrStatusPlates[startIndex] = step;
        step++;


        int nextIndex = startIndex + 1;
        int countPlate = arrStatusPlates.Length;

        //проверяем соседнии клетки и устанавливаем номер шага step
        if (nextIndex < countPlate && nextIndex % lenghtPoleLine != 0)
        {
            if (arrStatusPlates[nextIndex] == goodPlate)
                arrStatusPlates[nextIndex] = step;
        }

        nextIndex = startIndex - 1;
        if (nextIndex >= 0 && (nextIndex % lenghtPoleLine != lenghtPoleLine - 1))
        {
            if (arrStatusPlates[nextIndex] == goodPlate)
                arrStatusPlates[nextIndex] = step;
        }

        nextIndex = startIndex - lenghtPoleLine;
        if (nextIndex >= 0) 
        {
            if (arrStatusPlates[nextIndex] == goodPlate)
                arrStatusPlates[nextIndex] = step;
        }

        nextIndex = startIndex + lenghtPoleLine;
        if (nextIndex < countPlate)
        {
            if (arrStatusPlates[nextIndex] == goodPlate)
                arrStatusPlates[nextIndex] = step;
        }



        while (status)
        {
            for (int i = 0; i < arrStatusPlates.Length; i++)
            {
                if (arrStatusPlates[i] == step)
                {
                    nextIndex = i + 1;
                    if (nextIndex < countPlate && nextIndex % lenghtPoleLine != 0)
                    {
                        if (arrStatusPlates[nextIndex] == goodPlate)
                            arrStatusPlates[nextIndex] = step + 1;
                    }

                    nextIndex = i - 1;
                    if (nextIndex >= 0 && (nextIndex % lenghtPoleLine != lenghtPoleLine - 1))
                    {
                        if (arrStatusPlates[nextIndex] == goodPlate)
                            arrStatusPlates[nextIndex] = step + 1;
                    }

                    nextIndex = i - lenghtPoleLine;
                    if (nextIndex >= 0)
                    {
                        if (arrStatusPlates[nextIndex] == goodPlate)
                            arrStatusPlates[nextIndex] = step + 1;
                    }

                    nextIndex = i + lenghtPoleLine;
                    if (nextIndex < countPlate)
                    {
                        if (arrStatusPlates[nextIndex] == goodPlate)
                            arrStatusPlates[nextIndex] = step + 1;
                    }
                }
            }

            step++;
            if (arrStatusPlates[finishIdPoz] > 1)
            {
                status = false;
            }
            if (step > countPlate)
            {
                status = false;
                Debug.Log("Нет подходящего пути");
                return path;
            }

        }
        string strt = "";
        for (int i=0; i< arrStatusPlates.Length; i++)
        {
            strt += " "+arrStatusPlates[i];
        }
        Debug.Log("Поиск пути= " + strt);


        return getPath(ref path, ref arrStatusPlates, ref lenghtPoleLine, ref countPlate);
    }

    private List<int> getPath(ref List<int> path, ref int[] arrStatusPlates, ref int  lenghtPoleLine, ref int countPlate )
    {
        int step = arrStatusPlates[finishIdPoz];
        path.Add(finishIdPoz);

        while (step > 3)
        {
            step--;
            int lastIndex = path[path.Count - 1];

            if ((lastIndex - 1 >= 0) && (lastIndex - 1 % lenghtPoleLine != lenghtPoleLine - 1))
            {
                if (arrStatusPlates[lastIndex - 1] == step)
                {
                    path.Add(lastIndex - 1);
                    continue;
                }
            }

            if ((lastIndex + 1) < countPlate && (lastIndex + 1 % lenghtPoleLine != 0))
            {
                if (arrStatusPlates[lastIndex + 1] == step)
                {
                    path.Add(lastIndex + 1);
                    continue;
                }
            }

            if (lastIndex - lenghtPoleLine > 0)
            {
                if (arrStatusPlates[lastIndex - lenghtPoleLine] == step)
                {
                    path.Add(lastIndex - lenghtPoleLine);
                    continue;
                }
            }

            if (lastIndex + lenghtPoleLine < countPlate)
            {
                if (arrStatusPlates[lastIndex + lenghtPoleLine] == step)
                {
                    path.Add(lastIndex + lenghtPoleLine);
                    continue;
                }
            }
        }

        string str = "";
        for (int i = 0; i < path.Count; i++)
        {
            str += " " + path[i];
        }
        Debug.Log("Patch: "+ str);

        return path;
    }
}
