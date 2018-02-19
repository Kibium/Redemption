using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    public GameObject panel;
    public bool haSidoPresionado = false;
    // Use this for initialization
    void Start()
    {
        panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       if(!haSidoPresionado)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                panel.gameObject.SetActive(true);
                haSidoPresionado = true;
                ThirdPersonOrbitCamBasic.instance.horizontalAimingSpeed = 0;
                ThirdPersonOrbitCamBasic.instance.verticalAimingSpeed = 0;
               
}
        }
       else if(haSidoPresionado)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                panel.gameObject.SetActive(false);
                haSidoPresionado = false;
                ThirdPersonOrbitCamBasic.instance.horizontalAimingSpeed = 6;
                ThirdPersonOrbitCamBasic.instance.verticalAimingSpeed = 6;
            }
        }
    }
}
