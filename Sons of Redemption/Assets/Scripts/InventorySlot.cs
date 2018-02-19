using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {

    Item item;
    public Image icon;
    public Button removeButton;
    public GameObject removePanel;
    public Button useItem;
    public bool isActivated;
    private void Start()
    {
        removePanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (item == null) removeButton.interactable = false;
       
            if (PuertaAlmacen.llaveUsada)
            {
                if (item.name == "clau magatzem")
                {
                    Inventory.instance.Remove(item);
                PuertaAlmacen.llaveUsada = false;
                }
            }
      
        if (PuertaSalida.llaveUsada)
        {
            if(item.name == "clau sortida")
            {
                Inventory.instance.Remove(item);
                PuertaSalida.llaveUsada = false;
            }
        }
    }
    public void AddItem(Item newItem)
    {
       
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void clearSlot()
    {

        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }
    public void showRemovePanel()
    {
        if (item != null)
        {
            removePanel.gameObject.SetActive(true);
        }
    }
    public void backToPanel()
    {
        removePanel.gameObject.SetActive(false);
    }
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        removePanel.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }


}
