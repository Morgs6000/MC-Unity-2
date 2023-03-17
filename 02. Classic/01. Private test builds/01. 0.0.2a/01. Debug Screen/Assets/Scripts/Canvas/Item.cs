using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    /*[HideInInspector] */public ItemObject itemObject;

    private RawImage rawImage;
    private Image image;

    [HideInInspector] public VoxelType voxelType;

    private void Awake() {
        rawImage = GetComponentInChildren<RawImage>();
        rawImage.gameObject.SetActive(false);

        image = GetComponentInChildren<Image>();
        image.gameObject.SetActive(false);
    }

    public void InitialiseItem(ItemObject newItemObject) {
        itemObject = newItemObject;

        if(newItemObject.texture != null) {
            rawImage.texture = newItemObject.texture;
            rawImage.gameObject.SetActive(true);
        }
        if(newItemObject.sprite != null) {
            image.sprite = newItemObject.sprite;
            image.gameObject.SetActive(true);
        }
        
        voxelType = newItemObject.voxelType;
    }
}
