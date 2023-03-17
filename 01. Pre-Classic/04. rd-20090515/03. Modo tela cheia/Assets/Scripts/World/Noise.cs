using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    private static float frequecy = 0.01f;
    private static float amplitude = 50.0f;
    private static float heightOffset = 32.0f;
    private static int seed;
    
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
}
