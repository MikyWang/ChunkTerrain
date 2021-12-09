using System.Collections;
using System.Collections.Generic;
using MilkSpun.ChunkWorld.Models;
using MilkSpun.ChunkWorld.Utils;
using UnityEngine;

namespace MilkSpun.ChunWorld.Extentions
{
    public static class ChunkExtentions
    {
        public static int GetTextureID(this BlockConfig blockConfig, BlockFaceType blockFaceType)
        {
            return blockFaceType switch
            {
                BlockFaceType.Back => blockConfig.backFaceTexture +
                                      blockConfig.page * VoxelData.TextureAtlasSize,
                BlockFaceType.Front => blockConfig.frontFaceTexture +
                                       blockConfig.page * VoxelData.TextureAtlasSize,
                BlockFaceType.Top => blockConfig.topFaceTexture +
                                     blockConfig.page * VoxelData.TextureAtlasSize,
                BlockFaceType.Bottom => blockConfig.bottomFaceTexture +
                                        blockConfig.page * VoxelData.TextureAtlasSize,
                BlockFaceType.Left => blockConfig.leftFaceTexture +
                                      blockConfig.page * VoxelData.TextureAtlasSize,
                BlockFaceType.Right => blockConfig.rightFaceTexture +
                                       blockConfig.page * VoxelData.TextureAtlasSize,
                _ => 0
            };
        }

        public static ChunkCoord GetChunkCoordFromPosition(this Vector3 pos)
        {
            var x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
            var z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);

            return new ChunkCoord(x, z);
        }
    }
}
