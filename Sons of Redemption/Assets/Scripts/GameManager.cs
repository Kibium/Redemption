using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject pistola;
    public static bool playerGetPistola;
    public GameObject textToSeeInventory;
    public GameObject textToActivateLantern;
    public GameObject flechaGame;

    public GameObject flechaLlave;
    public static bool playerGetAlmacenKey;
    public static bool playerGetSalidaKey;
    public GameObject textToShot;

    public GameObject zombie;
    public GameObject zombie2;
    public GameObject zombie3;
    public GameObject textKillZombies;
    public GameObject textSalir;

    public static bool allZombiesKilled;
    public bool spawned;

    public GameObject balasUI;
	// Use this for initialization
    void Awake()
    {
    
    }
	void Start () {
        balasUI.gameObject.SetActive(false);
        spawned = false;
        playerGetAlmacenKey = false;
        playerGetSalidaKey = false;
        allZombiesKilled = false;
        linterna.isLanternExisting = false;
        pistola.gameObject.SetActive(false);
        flechaGame.gameObject.SetActive(true);
        flechaLlave.gameObject.SetActive(false);
        textToShot.gameObject.SetActive(false);
        textToSeeInventory.gameObject.SetActive(false);
        textToActivateLantern.gameObject.SetActive(false);
        textKillZombies.gameObject.SetActive(false);
        playerGetPistola = true;

        zombie.gameObject.SetActive(false);
        zombie2.gameObject.SetActive(false);
        zombie3.gameObject.SetActive(false);
        textSalir.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (playerGetPistola && textToShot.gameObject != null)
        {
            pistola.gameObject.SetActive(true);
            balasUI.gameObject.SetActive(true);
            textToShot.gameObject.SetActive(true);
        }

        if (linterna.isLanternExisting)
        {
            if (textToSeeInventory != null)
            {
                textToSeeInventory.gameObject.SetActive(true);
                Destroy(flechaGame);
            }
        }
        if (playerGetAlmacenKey)
        {
            Destroy(flechaLlave);
        }

        if (playerGetPistola && playerGetSalidaKey && textKillZombies.gameObject != null)
        {
            textKillZombies.gameObject.SetActive(true);
            zombie.gameObject.SetActive(true);
            zombie2.gameObject.SetActive(true);
            zombie3.gameObject.SetActive(true);
            spawned = true;
        }
        if (spawned)
        {
            if(zombie.gameObject == null && zombie2.gameObject == null && zombie3.gameObject == null)
            {
                textSalir.gameObject.SetActive(true);
                allZombiesKilled = true;
            }
        }
	}

    public void closeTextInventory()
    {
        if (textToSeeInventory != null && textToActivateLantern != null)
        {
            Destroy(textToSeeInventory);
            textToActivateLantern.gameObject.SetActive(true);

        }
    }
    public void closelanternText() {
        if (textToActivateLantern != null)
        {
            Destroy(textToActivateLantern);
            flechaLlave.gameObject.SetActive(true);
        }
    }
    public void CloseShotText()
    {
        Destroy(textToShot);
    }
    public void closeKillZombiesText()
    {
        Destroy(textKillZombies);
    }
  
}
