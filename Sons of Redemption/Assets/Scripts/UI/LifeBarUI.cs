using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeBarUI : MonoBehaviour {

    // Use this for initialization
    public RectTransform rectTrans;
    public Vector2 sizeDelta;
    private float minWidth;

    void Awake()
    {
        minWidth = 0;
        sizeDelta = rectTrans.sizeDelta;

        //UpdateLifeBarUI (25);
    }

    public void UpdateLifeBarUI(float newLife)
    {
        float newWidth = (newLife / 100) * sizeDelta.x;

        if (newWidth > sizeDelta.x) newWidth = sizeDelta.x;
        else if (newWidth < minWidth) newWidth = minWidth;

        rectTrans.sizeDelta = new Vector2(newWidth, sizeDelta.y);

    }
}

