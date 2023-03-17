using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelInteraction : MonoBehaviour {
    private Camera cam;
    private float rangeHit = 5.0f;
    private LayerMask groundMask;

    [SerializeField] private SelectedVoxel selectedVoxel;

    [SerializeField] private GameObject voxelCross;

    private void Awake() {
        cam = GetComponentInChildren<Camera>();
    }
    
    private void Start() {
        groundMask= LayerMask.GetMask("Ground");
    }

    private void Update() {
        RaycastUpdate();
    }

    private void RaycastUpdate() {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rangeHit, groundMask)) {
            DestroyVoxel(hit);
            AddVoxel(hit);
        }
    }

    private void DestroyVoxel(RaycastHit hit) {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 pointPos = hit.point - hit.normal / 2;

            Chunk c = Chunk.GetChunk(new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            ));

            c.SetVoxel(pointPos, VoxelType.air);
        }
    }

    private void AddVoxel(RaycastHit hit) {
        if(Input.GetMouseButtonDown(1)) {
            Vector3 pointPos = hit.point + hit.normal / 2;

            Chunk c = Chunk.GetChunk(new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            ));

            if(selectedVoxel.GetCurrentItem() != VoxelType.air) {
                if(selectedVoxel.GetCurrentItem() == VoxelType.sapling) {
                    Vector3 voxelOffset = new Vector3(
                        Mathf.FloorToInt(pointPos.x),
                        Mathf.FloorToInt(pointPos.y),
                        Mathf.FloorToInt(pointPos.z)
                    );
                    
                    Instantiate(voxelCross, voxelOffset, Quaternion.identity);
                }
                else {
                    c.SetVoxel(pointPos, selectedVoxel.GetCurrentItem());
                }                
            }
        }
    }
}
