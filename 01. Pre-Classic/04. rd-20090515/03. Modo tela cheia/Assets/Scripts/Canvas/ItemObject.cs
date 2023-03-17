using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable object/Item", fileName = "New Item")]
public class ItemObject : ScriptableObject {
    public Texture texture;
    public VoxelType voxelType;
}
