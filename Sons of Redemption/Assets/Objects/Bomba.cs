using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour {
    public Collider player;
    bool isColliding = false;
    bool isTouching = false;
    Ray ray;
    RaycastHit hit;
    public Texture2D text;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    bool toInventari = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isColliding = true;
        }
    }
    void OnMouseEnter()
    {
        isTouching = true;
}
    void OnMouseExit()
    {

        isTouching = false;
    }
// Use this for initialization
void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        if (isColliding)
        {
            if (isTouching)
            {
                Cursor.visible = false;
                Cursor.SetCursor(text, hotSpot, cursorMode);
                if (Input.GetMouseButton(0))
                {
                    toInventari = true;
                    Cursor.SetCursor(null, hotSpot, cursorMode);
                    Cursor.visible = true;
                    Destroy(this.gameObject);
                }
            }
       
        else
        {
                Cursor.SetCursor(null, hotSpot, cursorMode);
            Cursor.visible = true;
            }
        }
    }
}
