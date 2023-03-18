using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelInteraction : MonoBehaviour {
    private Camera cam;
    private float rangeHit = 5.0f;
    private LayerMask groundMask;

    private SelectedVoxel selectedVoxel;

    [SerializeField] private GameObject voxelCross;

    private InterfaceManager interfaceManager;

    private bool destroyVoxels;

    private void Awake() {
        cam = GetComponentInChildren<Camera>();

        selectedVoxel = GameObject.Find("Selected Voxel").GetComponent<SelectedVoxel>();
        interfaceManager = GameObject.Find("Interface Manager").GetComponent<InterfaceManager>();
    }
    
    private void Start() {
        groundMask= LayerMask.GetMask("Ground");

        destroyVoxels = true;
    }

    private void Update() {
        bool isPaused = interfaceManager.IsPaused();

        if(!isPaused) {
            RaycastUpdate();

            if(Input.GetMouseButtonDown(1)) {
                destroyVoxels = !destroyVoxels;
            }
        }
    }

    private void RaycastUpdate() {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rangeHit, groundMask)) {
            if(destroyVoxels) {
                DestroyVoxel(hit);
            }
            else {
                AddVoxel(hit);
            }
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
        if(Input.GetMouseButtonDown(0)) {
            Vector3 pointPos = hit.point + hit.normal / 2;

            if(pointPos.y > World.WorldSizeInVoxels.y) {
                return;
            }

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
