using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaSalida : MonoBehaviour {

    public GameObject pressToOpen;
    public GameObject notKey;
    public GameObject thisObject;
    public bool isInsideDoor;
    Item llave;

    public GameObject TextNeedShotGun;
    public static bool llaveUsada;

    public GameObject textKillZombiesMan;
    void Start()
    {
        pressToOpen.gameObject.SetActive(false);
        notKey.gameObject.SetActive(false);
        textKillZombiesMan.gameObject.SetActive(false);
        TextNeedShotGun.gameObject.SetActive(false);
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
            if (GameManager.playerGetSalidaKey && GameManager.allZombiesKilled)
            {
                pressToOpen.gameObject.SetActive(true);
                isInsideDoor = true;
            }
            if (!GameManager.playerGetSalidaKey)
            {
                notKey.gameObject.SetActive(true);
            }
            if(GameManager.playerGetSalidaKey && !GameManager.playerGetPistola)
            {
                TextNeedShotGun.gameObject.SetActive(true);
            }
           
            if(GameManager.playerGetSalidaKey && GameManager.playerGetPistola && !GameManager.allZombiesKilled)
            {
                textKillZombiesMan.gameObject.SetActive(true);
            }
        }

    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.tag != "Player")
        {
            notKey.gameObject.SetActive(false);
            pressToOpen.gameObject.SetActive(false);
            TextNeedShotGun.gameObject.SetActive(false);
            textKillZombiesMan.gameObject.SetActive(false);
        }
    }
}
