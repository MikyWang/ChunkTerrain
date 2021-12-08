using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Main
{
    public static class VoxelData
    {

        public const int ChunkWidth = 5;
        public const int ChunkHeight = 15;

        public const int TextureAtlasSizeInBlocks = 4;
        public static float NormalizedBlockTextureSize => 1f / TextureAtlasSizeInBlocks;

        public static readonly Vector3[] VoxelVerts =
        {
            new(0f, 0f, 0f),
            new(1f, 0f, 0f),
            new(1f, 1f, 0f),
            new(0f, 1f, 0f),
            new(0f, 0f, 1f),
            new(1f, 0f, 1f),
            new(1f, 1f, 1f),
            new(0f, 1f, 1f)
        };

        public static readonly Vector3[] FaceChecks =
        {
            new(0f, 0f, -1f),
            new(0f, 0f, 1f),
            new(0f, 1f, 0f),
            new(0f, -1f, 0f),
            new(-1f, 0f, 0f),
            new(1f, 0f, 0f),

        };

        public static readonly int[,] VoxelTris =
        {
            { 0, 3, 1, 2 }, //后面
            { 5, 6, 4, 7 }, //前面
            { 3, 7, 2, 6 }, //顶部
            { 1, 5, 0, 4 }, //底部
            { 4, 7, 0, 3 }, //左边
            { 1, 2, 5, 6 }  //右边
        };

        public static readonly Vector2[] VoxelUVs =
        {
            new(0f, 0f),
            new(0f, 1f),
            new(1f, 0f),
            new(1f, 1f)
        };
    }
}
