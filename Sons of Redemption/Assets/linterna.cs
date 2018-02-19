using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linterna : MonoBehaviour
{
    public bool lantern = false;
    public GameObject linternaObj;
    public bool pressLantern = false;

    public static bool isLanternExisting;
    private void Start()
    {
        linternaObj.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isLanternExisting)
        {
            if (!pressLantern)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    lantern = true;
                    pressLantern = true;
                }
            }

            else if (pressLantern)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {

                    lantern = false;
                    pressLantern = false;
                }
            }

            if (lantern)
            {
                linternaObj.gameObject.SetActive(true);
            }
            else
            {
                linternaObj.gameObject.SetActive(false);
            }
        }
    }
	
}
