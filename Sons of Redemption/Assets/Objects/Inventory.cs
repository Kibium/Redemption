using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory instance;

#region Singleton

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Error! Ya existe!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int spaceInventory = 6;
    public List<Item> items = new List<Item>();
    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if(items.Count >= spaceInventory){
                Debug.Log("Demasiados objetos!");
                return false;
            }
            items.Add(item);
            if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        }

        return true;
    }
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
           
    }
  
}
