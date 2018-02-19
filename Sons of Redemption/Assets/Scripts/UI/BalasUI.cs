using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BalasUI : MonoBehaviour
{
    public Text text;
    public Image[] imgs;
    public int balasUI;
    // Use this for initialization
    void Start()
    {

    }
    public void almacenBalas(int balas)
    {
        text.text = "/"+balas.ToString("");
    }
    // Update is called once per frame
    void Update()
    {
        
        if (GameObject.Find("Pistola") != null)
        {
            balasUI = GameObject.Find("Pistola").GetComponent<sho>().municion;
        }

        if (balasUI == 6)
        {
            imgs[6].gameObject.SetActive(true);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(false);
        }
        if (balasUI == 5)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(true);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(false);
        }
        if (balasUI == 4)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(true);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(false);
        }
        if (balasUI == 3)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(true);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(false);
        }
        if (balasUI ==2)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(true);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(false);
        }
        if (balasUI == 1)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(true);
            imgs[0].gameObject.SetActive(false);

        }
        if (balasUI == 0)
        {
            imgs[6].gameObject.SetActive(false);
            imgs[5].gameObject.SetActive(false);
            imgs[4].gameObject.SetActive(false);
            imgs[3].gameObject.SetActive(false);
            imgs[2].gameObject.SetActive(false);
            imgs[1].gameObject.SetActive(false);
            imgs[0].gameObject.SetActive(true);

        }
    }
}
