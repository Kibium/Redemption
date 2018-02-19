using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicityUI : MonoBehaviour
{
    public RectTransform rectTrans;
    public Vector2 sizeDelta;
    private float minWidth;

    void Awake()
    {
        minWidth = 0;
        sizeDelta = rectTrans.sizeDelta;

    }

    public void UpdateToxicityUI(float newToxicity)
    {
        float newWidth = (newToxicity / 100) * sizeDelta.x;

        if (newWidth > sizeDelta.x) newWidth = sizeDelta.x;
        else if (newWidth < minWidth) newWidth = minWidth;

        rectTrans.sizeDelta = new Vector2(newWidth, sizeDelta.y);

    }
}

