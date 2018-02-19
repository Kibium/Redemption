
using UnityEngine;

public class ItemPickUp : MonoBehaviour {
    public Collider player;
    bool isColliding = false;
    bool isTouching = false;
    public Texture2D text = null;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Item item;
    public GameObject textGetObject;

    
    public bool getKeyAlmacen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isColliding = true;
          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
        {
            isColliding = false;
         
        }
    }
    // Use this for initialization
    void Start()
    {
        textGetObject.gameObject.SetActive(false);
    }
 
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        if (isColliding)
        {
            textGetObject.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.G))
                {
                textGetObject.gameObject.SetActive(false);
                    bool getObject = Inventory.instance.Add(item);
                    Debug.Log("He cogido" + item.name);
                    if (item.name == "linterna")
                    {
                        linterna.isLanternExisting = true;
                    }
                    if(item.name == "clau magatzem")
                    {
                    GameManager.playerGetAlmacenKey = true;
                    }
                    if (item.name == "clau sortida")
                    {
                        GameManager.playerGetSalidaKey = true;
                    
                    }
                    if (item.name == "revolver")
                        {
                        GameManager.playerGetPistola = true;
 
                        }
                if (getObject)
                    {
                        Destroy(this.gameObject);
                    }
                }

            //TEXTO-> PRESIONA B PARA COGER!

        }
        else
        {
            textGetObject.gameObject.SetActive(false);
            //TEXT = FALSE
        }
      
    }
}
