using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    private static float frequecy = 0.01f;
    private static float amplitude = 50.0f;
    private static float heightOffset = 32.0f;
    private static int seed = 100;
    
    public static int Perlin(float x, float z) {        
        x *= frequecy;
        z *= frequecy;
        
        x += seed;
        z += seed;
        
        float y = Mathf.PerlinNoise(x, z);
        y *= amplitude;
        y += heightOffset;

        return (int)y;
    }

    public static float Perlin3D(float x, float y, float z) {
        x += seed;
        y += seed;
        z += seed;
        
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);

        float yx = Mathf.PerlinNoise(y, x);
        float yz = Mathf.PerlinNoise(y, z);

        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        float xyz = xy + xz + yx + yz + zx + zy;
        
        return xyz / 6.0f;
    }
}
