using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkSpun.ChunkWorld.Models;
using MilkSpun.ChunkWorld.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MilkSpun.ChunkWorld.Main
{
    public class World : MonoBehaviour
    {
        public Material material;
        [SerializeField, InlineEditor, Space]
        private BlockConfig[] blockConfigs;
        private Chunk[,] _chunks;

        [Button("生成地形世界")]
        private void Start()
        {
            ClearChunks();
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            _chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];
            for (var x = 0; x < VoxelData.WorldSizeInChunks; x++)
            {
                for (var z = 0; z < VoxelData.WorldSizeInChunks; z++)
                {
                    CreateChunk(x, z);
                }
            }
        }

        public VoxelMapType GetVoxel(Vector3 pos)
        {
            if (!IsVoxelInWorld(pos)) return VoxelMapType.Air;

            return pos.y switch
            {
                VoxelData.ChunkHeight - 1 => VoxelMapType.Grass,
                < 1 => VoxelMapType.Stone,
                _ => VoxelMapType.Dirt
            };
        }

        private void CreateChunk(int x, int z)
        {
            _chunks[x, z] = new Chunk(this, new ChunkCoord(x, z));
        }

        public BlockConfig GetBlockTypeIndex(VoxelMapType voxelMapType)
        {
            return blockConfigs.First(b => b.voxelMapType == voxelMapType);
        }

        private void ClearChunks()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private bool IsChunkInWorld(ChunkCoord coord)
        {
            var x = coord.x;
            var z = coord.z;

            return x is > 0 and < VoxelData.WorldSizeInChunks &&
                   z is > 0 and < VoxelData.WorldSizeInChunks;
        }

        private static bool IsVoxelInWorld(Vector3 pos)
        {
            var x = pos.x;
            var y = pos.y;
            var z = pos.z;

            return x is >= 0 and < VoxelData.WorldSizeInVoxels &&
                   y is >= 0 and < VoxelData.ChunkHeight &&
                   z is >= 0 and < VoxelData.WorldSizeInVoxels;

        }

    }

}
