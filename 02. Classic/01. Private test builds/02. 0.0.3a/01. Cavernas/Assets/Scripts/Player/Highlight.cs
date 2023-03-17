using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highlight : MonoBehaviour {
    private Camera cam;
    private float rangeHit = 5.0f;
    private LayerMask groundMask;

    [SerializeField] private GameObject highlight;

    private Mesh voxelMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private List<Vector3> vertices = new List<Vector3>();
    private enum VoxelSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    private List<int> triangles = new List<int>();
    private int vertexIndex;

    private List<Vector2> uv = new List<Vector2>();
    [SerializeField] private Material material;

    private void Awake() {
        cam = GetComponentInChildren<Camera>();

        meshFilter = (MeshFilter)highlight.AddComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)highlight.AddComponent(typeof(MeshRenderer));
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
            Vector3 pointPos = hit.point - hit.normal / 2;
            Vector3 pointPos2 = hit.point + hit.normal / 2;
            
            highlight.transform.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );            

            HighlightGen(pointPos, pointPos2);

            highlight.SetActive(true);

            ColorUpdate();
        }
        else {
            highlight.SetActive(false);
        }
    }

    private void ColorUpdate() {
        Color colorA = material.color;
        colorA.a = 0.5f;

        Color colorB = material.color;
        colorB.a = 0.0f;

        float speed = 2.0f;

        meshRenderer.material = material;
        meshRenderer.material.color = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time * speed, 1));
    }

    private void HighlightGen(Vector3 pointPos, Vector3 pointPos2) {
        voxelMesh = new Mesh();
        voxelMesh.name = "Highlight";

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;

        VoxelGen(pointPos, pointPos2);
        
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
        meshRenderer.material = material;
    }

    private void VoxelGen(Vector3 pointPos, Vector3 pointPos2) {
        if(pointPos2.x > pointPos.x) {
            VerticesAdd(VoxelSide.RIGHT);
        }
        if(pointPos2.x < pointPos.x) {
            VerticesAdd(VoxelSide.LEFT);
        }
        if(pointPos2.y > pointPos.y) {
            VerticesAdd(VoxelSide.TOP);
        }
        if(pointPos2.y < pointPos.y) {
            VerticesAdd(VoxelSide.BOTTOM);
        }
        if(pointPos2.z > pointPos.z) {
            VerticesAdd(VoxelSide.FRONT);
        }
        if(pointPos2.z < pointPos.z) {
            VerticesAdd(VoxelSide.BACK);
        }
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
        Vector2 textureSizeInTiles = new Vector2(1, 1);
        
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
        UVAdd(new Vector2(0, 0));
    }
}
