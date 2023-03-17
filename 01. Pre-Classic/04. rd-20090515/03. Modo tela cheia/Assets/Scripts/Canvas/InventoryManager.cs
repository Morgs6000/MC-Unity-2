using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private ItemObject[] startItems;

    public GameObject toolbar;

    public List<Slot> slots = new List<Slot>();

    public GameObject itemPrefab;

    private void Awake() {
        slots.AddRange(toolbar.GetComponentsInChildren<Slot>());
    }

    private void Start() {
        foreach(var itemObject in startItems) {
            AddItem(itemObject);
        }
    }

    public bool AddItem(ItemObject itemObject) {
        for(int i = 0; i < slots.Count; i++) {
            Slot slot = slots[i];
            Item itemInSlot = slot.GetComponentInChildren<Item>();

            if(itemInSlot == null) {
                SpawnNewItem(itemObject, slot);

                return true;
            }
        }
        
        return false;
    }

    public void SpawnNewItem(ItemObject itemObject, Slot slot) {
        GameObject newItemObject = Instantiate(itemPrefab, slot.transform);

        Item item = newItemObject.GetComponent<Item>();
        item.InitialiseItem(itemObject);
    }
}
