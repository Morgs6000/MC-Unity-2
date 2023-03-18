using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVCoords {
    public static Vector2 UVs(VoxelType voxelType, VoxelSide side) {
        // STONE
        if(voxelType == VoxelType.stone) {
            return new Vector2(1, 0);
        }

        // GRASS BLOCK
        if(voxelType == VoxelType.grass_block) {
            if(side == VoxelSide.TOP) {
                return new Vector2(0, 0);
            }
            if(side == VoxelSide.BOTTOM) {
                return new Vector2(2, 0);
            }

            return new Vector2(3, 0);
        }

        // DIRT
        if(voxelType == VoxelType.dirt) {
            return new Vector2(2, 0);
        }

        // COBBLESTONE
        if(voxelType == VoxelType.cobblestone) {
            return new Vector2(0, 1);
        }

        // PLANKS
        if(voxelType == VoxelType.planks) {
            return new Vector2(4, 0);
        }
        
        // SAPILING
        if(voxelType == VoxelType.sapling) {
            return new Vector2(15, 0);
        }

        // BEDROCK
        if(voxelType == VoxelType.bedrock) {
            return new Vector2(1, 1);
        }

        // WATER
        if(voxelType == VoxelType.water) {
            return new Vector2(13, 0);
        }

        // LAVA
        if(voxelType == VoxelType.lava) {
            return new Vector2(14, 1);
        }

        return default;
    }
}
