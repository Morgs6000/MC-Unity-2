using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    [HideInInspector] public ItemObject itemObject;
    
    private Image image;
    private RawImage rawImage;
    [HideInInspector] public VoxelType voxelType;

    public void InitialiseItem(ItemObject newItemObject) {
        itemObject = newItemObject;
        
        rawImage = GetComponent<RawImage>();
        rawImage.texture = newItemObject.texture;

        voxelType = newItemObject.voxelType;
    }
}
