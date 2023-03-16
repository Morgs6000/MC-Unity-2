using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    //private static Vector3 WorldSizeInVoxels = new Vector3(256, 64, 256);
    private static Vector3 WorldSizeInVoxels = new Vector3(32, 64, 32);

    public static Vector3 WorldSizeInChunks = new Vector3(
        WorldSizeInVoxels.x / Chunk.ChunkSizeInVoxels.x,
        WorldSizeInVoxels.y / Chunk.ChunkSizeInVoxels.y,
        WorldSizeInVoxels.z / Chunk.ChunkSizeInVoxels.z
    );

    [SerializeField] private GameObject chunkPrefab;

    private static List<Chunk> chunkMap = new List<Chunk>();
    
    private void Start() {
        StartCoroutine(WorldGen());
    }

    private void Update() {
        
    }

    private IEnumerator WorldGen() {
        Vector3 WorldSize = new Vector3(
            WorldSizeInChunks.x / 2,
            WorldSizeInChunks.y,            
            WorldSizeInChunks.z / 2            
        );

        for(int x = -(int)WorldSize.x; x < WorldSize.x; x++) {
            for(int z = -(int)WorldSize.z; z < WorldSize.z; z++) {

                for(int y = 0; y < WorldSize.y; y++) {
                    InstantiateChunk(new Vector3(x, y, z));
                }

                yield return null;
            }
        }
    }

    private void InstantiateChunk(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;
        
        Vector3 chunkOffset = new Vector3(
            x * Chunk.ChunkSizeInVoxels.x,
            y * Chunk.ChunkSizeInVoxels.y,
            z * Chunk.ChunkSizeInVoxels.z
        );

        GameObject chunk = Instantiate(chunkPrefab);
        chunk.transform.position = chunkOffset;
        chunk.transform.SetParent(transform);
        chunk.name = "Chunk: " + x + ", " + z;

        /*
        Chunk c = chunk.GetComponent<Chunk>();
        c = Chunk.GetChunk(new Vector3(x, y, z));
        c.ChunkGen();
        */
    }
}