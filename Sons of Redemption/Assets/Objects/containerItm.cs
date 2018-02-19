using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class containerItm : MonoBehaviour {
    public bool existingItem;
    private bool isTouching;
    public GameObject objectToInstantiate;
    public Canvas canvas;
    public static bool playerIsHere = false;
    // Use this for initialization

    void OnMouseEnter()
    {
        isTouching = true;
    }
    void OnMouseExit()
    {

        isTouching = false;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(existingItem)
        {
            if(isTouching)
            {
                if (Input.GetMouseButton(0))
                {
                    if (objectToInstantiate != null)
                    {
                        Instantiate(objectToInstantiate, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity, canvas.transform);
                        existingItem = false;
                    }
                }
            }
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIsHere = true;
        }
      
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
        {
            playerIsHere = false;
        }
    }

}
