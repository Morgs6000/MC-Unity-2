using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedVoxel : MonoBehaviour {
    [SerializeField] private Slot[] slots;

    private int slotIndex = 0;

    private void Awake() {
        slots = GetComponentsInChildren<Slot>();
    }
    
    private void Start() {
        // desativa todos os GameObjects dos slots, exceto o primeiro
        for (int i = 1; i < slots.Length; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

    private void Update() {
        KeyInputs();
        ScrollInput();

        SlotIndexUpdate();
    }

    private void KeyInputs() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            slotIndex = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            slotIndex = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            slotIndex = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            slotIndex = 3;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)) {
            slotIndex = 4;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)) {
            slotIndex = 5;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)) {
            slotIndex = 6;
        }
        /*
        if(Input.GetKeyDown(KeyCode.Alpha8)) {
            slotIndex = 7;
        }
        if(Input.GetKeyDown(KeyCode.Alpha9)) {
            slotIndex = 8;
        }
        */
    }

    private void ScrollInput() {
        if(Input.GetAxis("Mouse ScrollWheel") < 0.0f) {
            slotIndex++;
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
            slotIndex--;
        }

        if(slotIndex > slots.Length - 1) {
            slotIndex = 0;
        }
        if(slotIndex < 0) {
            slotIndex = slots.Length - 1;
        }
    }

    private void SlotIndexUpdate() {
        // adicione outros casos para cada tecla numérica
        slotIndex = Mathf.Clamp(slotIndex, 0, slots.Length - 1);

        // desativa o GameObject do slot anterior
        if(slotIndex > 0) {
            slots[slotIndex - 1].gameObject.SetActive(false);
        }

        // ativa o GameObject do slot atual
        if(slotIndex < slots.Length) {
            slots[slotIndex].gameObject.SetActive(true);
        }
    }

    public VoxelType GetCurrentItem() {
        // Obter a referência ao slot atual
        Slot currentSlot = slots[slotIndex].GetComponent<Slot>();

        if(currentSlot.transform.childCount == 0) {
            return VoxelType.air;
        }
        else {
            // Obter a referência ao GameObject filho "ItemObject" do slot atual
            GameObject currentItem = currentSlot.transform.GetChild(0).gameObject;

            // Obter a referência ao script "Item" do item atual
            Item item = currentItem.GetComponent<Item>();

            // Acessar a variável "voxelType" do item atual por meio do script "Item"
            VoxelType voxelType = item.voxelType;

            return voxelType;
        }
    }
}
