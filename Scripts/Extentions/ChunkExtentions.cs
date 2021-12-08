using System.Collections;
using System.Collections.Generic;
using MilkSpun.ChunkWorld.Models;
using UnityEngine;

namespace MilkSpun.ChunWorld.Extentions
{
    public static class ChunkExtentions {
        
        public static int GetTextureID(this BlockConfig blockConfig,BlockFaceType blockFaceType)
        {
            return blockFaceType switch
            {
                BlockFaceType.Back => blockConfig.backFaceTexture,
                BlockFaceType.Front => blockConfig.frontFaceTexture,
                BlockFaceType.Top => blockConfig.topFaceTexture,
                BlockFaceType.Bottom => blockConfig.bottomFaceTexture,
                BlockFaceType.Left => blockConfig.leftFaceTexture,
                BlockFaceType.Right => blockConfig.rightFaceTexture,
                _ => 0
            };

        }
        
    }
}
