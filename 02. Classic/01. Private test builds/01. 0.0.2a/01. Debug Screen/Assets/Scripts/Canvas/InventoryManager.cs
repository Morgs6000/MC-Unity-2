using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private ItemObject[] startItems;

    public GameObject toolbar;

    public List<Slot> slots = new List<Slot>();

    public GameObject itemPrefab;

    private bool skipNextSlot;

    private void Awake() {
        slots.AddRange(toolbar.GetComponentsInChildren<Slot>());
    }

    private void Start() {
        for(int i = 0; i < startItems.Length; i++) {
            if(startItems[i] == null) {
                // Se um elemento do array "startItems" for vazio, deixe um slot vazio e pele para o proximo slot.
                
                //Slot slot = slots[i];
                //i++;

                /*
                for(int j = 0; j < slots.Count; j++) {
                    Slot slot = slots[j];
                    j++;
                }
                */

                continue;
            }
            else {
                AddItem(startItems[i]);
            }
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
