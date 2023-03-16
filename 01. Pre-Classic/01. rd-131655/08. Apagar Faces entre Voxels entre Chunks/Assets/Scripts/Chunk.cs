using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Mesh voxelMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private List<Vector3> vertices = new List<Vector3>();
    private enum VoxelSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    private List<int> triangles = new List<int>();
    private int vertexIndex;

    private List<Vector2> uv = new List<Vector2>();

    public static Vector3 ChunkSizeInVoxels = new Vector3(16, 64, 16);

    private VoxelType[,,] voxelMap = new VoxelType[(int)ChunkSizeInVoxels.x, (int)ChunkSizeInVoxels.y, (int)ChunkSizeInVoxels.z];

    private VoxelType voxelType;

    private static List<Chunk> chunkMap = new List<Chunk>();

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        chunkMap.Add(this);

        VoxelMapGen();
    }

    private void Update() {
        
    }

    public VoxelType GetVoxel(Vector3 worldPos) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        if(
            x < 0 || x >= ChunkSizeInVoxels.x ||
            y < 0 || y >= ChunkSizeInVoxels.y ||
            z < 0 || z >= ChunkSizeInVoxels.z
        ) {
            Debug.LogError("Coordinates out of range");

            return default(VoxelType);
        }

        return voxelMap[x, y, z];
    }

    public static Chunk GetChunk(Vector3 pos) {        
        for(int i = 0; i < chunkMap.Count; i++) {            
            Vector3 chunkPos = chunkMap[i].transform.position;

            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + ChunkSizeInVoxels.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + ChunkSizeInVoxels.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + ChunkSizeInVoxels.z
            ) {
                continue;
            }

            return chunkMap[i];
        }

        return null;
    }

    private void VoxelMapGen() {
        for(int x = 0; x < ChunkSizeInVoxels.x; x++) {
            for(int y = 0; y < ChunkSizeInVoxels.y; y++) {
                for(int z = 0; z < ChunkSizeInVoxels.z; z++) {
                    VoxelLayers(new Vector3(x, y, z));
                }
            }
        }

        ChunkGen();
    }

    private void VoxelLayers(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        if(y < 32) {
            voxelMap[x, y, z] = VoxelType.stone;
        }
        if(y == 32) {
            voxelMap[x, y, z] = VoxelType.grass_block;
        }
    }

    public void ChunkGen() {
        voxelMesh = new Mesh();
        voxelMesh.name = "Chunk";

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;

        for(int x = 0; x < ChunkSizeInVoxels.x; x++) {
            for(int y = 0; y < ChunkSizeInVoxels.y; y++) {
                for(int z = 0; z < ChunkSizeInVoxels.z; z++) {
                    if(voxelMap[x, y, z] != VoxelType.air) {
                        VoxelGen(new Vector3(x, y, z));
                    }                    
                }
            }
        }

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

    private void VoxelGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        voxelType = voxelMap[x, y, z];

        if(!HasSolidNeighbor(new Vector3(1, 0, 0) + offset)) {
            VerticesAdd(VoxelSide.RIGHT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(-1, 0, 0) + offset)) {
            VerticesAdd(VoxelSide.LEFT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 1, 0) + offset)) {
            VerticesAdd(VoxelSide.TOP, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, -1, 0) + offset)) {
            VerticesAdd(VoxelSide.BOTTOM, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, 1) + offset)) {
            VerticesAdd(VoxelSide.FRONT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, -1) + offset)) {
            VerticesAdd(VoxelSide.BACK, offset);
        }
    }

    private bool HasSolidNeighbor(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        //*
        if(
            x < 0 || x >= ChunkSizeInVoxels.x ||
            y < 0 || y >= ChunkSizeInVoxels.y ||
            z < 0 || z >= ChunkSizeInVoxels.z
        ) {
            return false;
        }
        //*/

        /*
        Vector3 WorldSize = new Vector3(
            World.WorldSizeInChunks.x / 2,
            World.WorldSizeInChunks.y,            
            World.WorldSizeInChunks.z / 2            
        );

        if(
            x < 0 || x >= ChunkSizeInVoxels.x ||
            y < 0 || y >= ChunkSizeInVoxels.y ||
            z < 0 || z >= ChunkSizeInVoxels.z
        ) {
            if(
                x < -WorldSize.x || x >= WorldSize.x ||
                y < -WorldSize.y || y >= WorldSize.y ||
                z < -WorldSize.z || z >= WorldSize.z
            ) {
                return false;
            }
        }
        //*/

        /*
        if(
            x < 0 || x >= ChunkSizeInVoxels.x ||
            y < 0 || y >= ChunkSizeInVoxels.y ||
            z < 0 || z >= ChunkSizeInVoxels.z
        ) {
            for(int i = 0; i < chunkMap.Count; i++) {            
                Vector3 chunkPos = chunkMap[i].transform.position;

                if(
                    x < chunkPos.x || x > (chunkPos.x + ChunkSizeInVoxels.x - 1) || 
                    y < chunkPos.y || y > (chunkPos.y + ChunkSizeInVoxels.y - 1) || 
                    z < chunkPos.z || z > (chunkPos.z + ChunkSizeInVoxels.z - 1)
                ) {
                    return false;
                }
            }
        }
        */

        if(voxelMap[x, y, z] == VoxelType.air) {
            return false;
        }

        /*
        if(GetVoxel(new Vector3(World.WorldSizeInVoxels.x, World.WorldSizeInVoxels.y, World.WorldSizeInVoxels.z)) == VoxelType.air) {
            return false;
        }
        */
        
        /*
        for(int i = 0; i < chunkMap.Count; i++) {            
            Vector3 chunkPos = chunkMap[i].transform.position;

            if(
                x < chunkPos.x || x > (chunkPos.x + ChunkSizeInVoxels.x - 1) || 
                y < chunkPos.y || y > (chunkPos.y + ChunkSizeInVoxels.y - 1) || 
                z < chunkPos.z || z > (chunkPos.z + ChunkSizeInVoxels.z - 1)
            ) {
                return false;
            }
        }
        //*/

        return true;
    }

    private void VerticesAdd(VoxelSide side, Vector3 offset) {
        switch(side) {
            case VoxelSide.RIGHT: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);
                break;
            }
            case VoxelSide.LEFT: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);
                break;
            }
            case VoxelSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                break;
            }
            case VoxelSide.BOTTOM: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);
                break;
            }
            case VoxelSide.FRONT: {
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);
                break;
            }
            case VoxelSide.BACK: {
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);
                break;
            }
        }

        TrianglesAdd();
        UVCoordinate();
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

    private void UVCoordinate() {
        // STONE
        if(voxelType == VoxelType.stone) {
            UVAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(voxelType == VoxelType.grass_block) {
            UVAdd(new Vector2(0, 0));
        }        
    }
}