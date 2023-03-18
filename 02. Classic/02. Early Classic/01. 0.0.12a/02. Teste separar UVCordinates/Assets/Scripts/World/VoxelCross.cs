using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCross : MonoBehaviour {
    private Mesh voxelMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private List<Vector3> vertices = new List<Vector3>();

    private List<int> triangles = new List<int>();
    private int vertexIndex;

    private List<Vector2> uv = new List<Vector2>();

    private VoxelType voxelType;

    //[SerializeField] private SelectedVoxel selectedBlock;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        //voxelType = selectedBlock.GetCurrentItem();
        voxelType = VoxelType.sapling;
    }

    private void Start() {
        voxelMesh = new Mesh();
        voxelMesh.name = "Voxel Cross";

        VoxelGen();

        MeshGen();
    }

    private void Update() {
        
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
        VerticesAdd(VoxelSide.FRONT_RIGHT);
        VerticesAdd(VoxelSide.BACK_LEFT);
        VerticesAdd(VoxelSide.BAKC_RIGHT);
        VerticesAdd(VoxelSide.FRONT_LEFT);
    }

    private void VerticesAdd(VoxelSide side) {
        switch(side) {
            case VoxelSide.FRONT_RIGHT: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));
                break;
            }
            case VoxelSide.BACK_LEFT: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));
                break;
            }
            case VoxelSide.BAKC_RIGHT: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));
                break;
            }
            case VoxelSide.FRONT_LEFT: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));
                break;
            }
        }

        TrianglesAdd();
        UVAdd(UVCoords.UVs(voxelType, side));
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
}
