using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Main
{
    public static class VoxelData
    {

        public static readonly int ChunkWidth = 1;
        public static readonly int ChunkHeight = 1;

        public static readonly Vector3[] VexelVerts =
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
            { 0, 3, 1, 1, 3, 2 }, //后面
            { 5, 6, 4, 4, 6, 7 }, //前面
            { 3, 7, 2, 2, 7, 6 }, //顶部
            { 1, 5, 0, 0, 5, 4 }, //底部
            { 4, 7, 0, 0, 7, 3 }, //左边
            { 1, 2, 5, 5, 2, 6 }  //右边
        };

        public static readonly Vector2[] VoxelUVs =
        {
            new(0f, 0f),
            new(0f, 1f),
            new(1f, 0f),
            new(1f, 0f),
            new(0f, 1f),
            new(1f, 1f)
        };
    }
}
