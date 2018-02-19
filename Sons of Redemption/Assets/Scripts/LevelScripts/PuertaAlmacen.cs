using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaAlmacen : MonoBehaviour {

    public GameObject pressToOpen;
    public GameObject notKey;
    public GameObject thisObject;
    public bool isInsideDoor;
    Item llave;
    public static bool llaveUsada;
    void Start()
    {
        pressToOpen.gameObject.SetActive(false);
        notKey.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isInsideDoor)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                llaveUsada = true;
                Destroy(pressToOpen);
                Destroy(notKey);
                Destroy(thisObject);
                Destroy(llave);
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (GameManager.playerGetAlmacenKey)
            {
                pressToOpen.gameObject.SetActive(true);
                isInsideDoor = true;
            }
            else
            {
                notKey.gameObject.SetActive(true);
            }
        }
        
    }
    void OnTriggerExit(Collider coll)
    {
        if(coll.tag != "Player")
        {
            notKey.gameObject.SetActive(false);
            pressToOpen.gameObject.SetActive(false);
        }
    }
}
