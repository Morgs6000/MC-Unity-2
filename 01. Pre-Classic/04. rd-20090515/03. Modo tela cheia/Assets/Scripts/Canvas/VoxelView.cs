using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelView : MonoBehaviour {
    private Mesh voxelMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private List<Vector3> vertices = new List<Vector3>();
    private enum VoxelSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    private List<int> triangles = new List<int>();
    private int vertexIndex;

    private List<Vector2> uv = new List<Vector2>();

    private VoxelType voxelType;

    [SerializeField] private SelectedVoxel selectedBlock;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        
    }

    private void Update() {
        voxelType = selectedBlock.GetCurrentItem();

        VoxelViewGen();
    }

    private void VoxelViewGen() {
        voxelMesh = new Mesh();
        voxelMesh.name = "Voxel View";

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;

        VoxelGen();

        MeshGen();
    }

    private void MeshGen() {
        voxelMesh.vertices = vertices.ToArray();
        voxelMesh.triangles = triangles.ToArray();
        voxelMesh.uv = uv.ToArray();

        voxelMesh.RecalculateBounds();
        voxelMesh.RecalculateNormals();
        voxelMesh.Optimize();

        meshFilter.mesh = voxelMesh;
    }

    private void VoxelGen() {
        VerticesAdd(VoxelSide.RIGHT);
        VerticesAdd(VoxelSide.LEFT);
        VerticesAdd(VoxelSide.TOP);
        VerticesAdd(VoxelSide.BOTTOM);
        VerticesAdd(VoxelSide.FRONT);
        VerticesAdd(VoxelSide.BACK);
    }

    private void VerticesAdd(VoxelSide side) {
        switch(side) {
            case VoxelSide.RIGHT: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));
                break;
            }
            case VoxelSide.LEFT: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));
                break;
            }
            case VoxelSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));
                break;
            }
            case VoxelSide.BOTTOM: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                break;
            }
            case VoxelSide.FRONT: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));
                break;
            }
            case VoxelSide.BACK: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));
                break;
            }
        }

        TrianglesAdd();
        UVCoordinate(side);
    }

    private void TrianglesAdd() {
        // Primeiro Tiangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }

    private void UVAdd(Vector2 textureCoordinate) {
        Vector2 textureSizeInTiles = new Vector2(16, 16);
        
        float x = textureCoordinate.x;
        float y = textureCoordinate.y;

        float _x = 1.0f / textureSizeInTiles.x;
        float _y = 1.0f / textureSizeInTiles.y;

        y = (textureSizeInTiles.y - 1) - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }

    private void UVCoordinate(VoxelSide side) {
        // STONE
        if(voxelType == VoxelType.stone) {
            UVAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(voxelType == VoxelType.grass_block) {
            if(side == VoxelSide.TOP) {
                UVAdd(new Vector2(0, 0));
                return;
            }
            if(side == VoxelSide.BOTTOM) {
                UVAdd(new Vector2(2, 0));
                return;
            }

            UVAdd(new Vector2(3, 0));
        }

        // DIRT
        if(voxelType == VoxelType.dirt) {
            UVAdd(new Vector2(2, 0));
        }

        // COBBLESTONE
        if(voxelType == VoxelType.cobblestone) {
            UVAdd(new Vector2(0, 1));
        }

        // PLANKS
        if(voxelType == VoxelType.planks) {
            UVAdd(new Vector2(4, 0));
        }
    }
}
