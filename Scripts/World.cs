using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MilkSpun.ChunkWorld.Models;
using MilkSpun.ChunkWorld.Utils;
using MilkSpun.ChunWorld.Extentions;
using MilkSpun.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MilkSpun.ChunkWorld.Main
{
    public class World : MonoBehaviour, IWorld
    {
        public Material material;
        [SerializeField] private Transform cameraRig;
        [SerializeField] private int seed;
        [SerializeField, InlineEditor] private Biome biome;
        [SerializeField, InlineEditor, Space] private BlockConfig[] blockConfigs;
        private Chunk[,] _chunks;
        private List<ChunkCoord> _activeChunks;
        private ChunkCoord _playerLastChunkCoord;
        private ChunkCoord _playerChunkCoord;
        private Vector3 _spawnPosition;

        public int NoiseResolution => VoxelData.ChunkWidth;

        [Button("生成地形世界")]
        private void Start()
        {
            Random.InitState(seed);
            InitChunks();
            GenerateWorld();
        }

        private void Update()
        {
            _playerChunkCoord = cameraRig.position.GetChunkCoordFromPosition();
            if (_playerLastChunkCoord != _playerChunkCoord)
            {
                CheckViewDistance();
                _playerLastChunkCoord = _playerChunkCoord;
            }
        }

        private void GenerateWorld()
        {
            var min = Mathf.Max(VoxelData.WorldSizeInChunks / 2 - VoxelData.ViewDistanceInChunks,
                0);
            var max = Mathf.Max(VoxelData.WorldSizeInChunks / 2 + VoxelData.ViewDistanceInChunks,
                0);
            _chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];
            for (var x = min; x < max; x++)
            {
                for (var z = min; z < max; z++)
                {
                    CreateChunk(x, z);
                }
            }
            cameraRig.position = _spawnPosition;
            _playerLastChunkCoord = cameraRig.position.GetChunkCoordFromPosition();
        }

        //TODO:构建复杂地形逻辑
        public VoxelMapType GetVoxel(Vector3 pos)
        {
            //默认行为
            if (!IsVoxelInWorld(pos)) return VoxelMapType.Air;

            var y = Mathf.FloorToInt(pos.y);
            if (y == 0) return VoxelMapType.BedRock;

            // 基础地形
            var noise =
                NoiseGenerator.Get2DPerlinNoise(new Vector2(pos.x, pos.z), 0, biome.scale,
                    NoiseResolution) *
                biome.terrainHeight;
            var terrainHeight = Mathf.FloorToInt(noise) + biome.solidGroundHeight;
            if (y > terrainHeight)
            {
                return VoxelMapType.Air;
            }

            var voxelType = VoxelMapType.Air;

            if (y == terrainHeight)
                voxelType = VoxelMapType.Grass;
            if (y < terrainHeight)
            {
                voxelType = y > terrainHeight - 5 ? VoxelMapType.Dirt : VoxelMapType.Stone;
            }

            //生物多样性地形

            if (voxelType == VoxelMapType.Stone)
            {
                foreach (var lode in biome.lodes)
                {
                    if (y < lode.minHeight || y > lode.maxHeight) continue;

                    if (NoiseGenerator.Get3DPerlinNoise(pos, lode.offset, lode.scale,
                            lode.threshold, 1))
                    {
                        voxelType = lode.voxelMapType;
                    }
                }
            }

            return voxelType;

        }

        private void CreateChunk(int x, int z)
        {
            var coord = new ChunkCoord(x, z);
            _chunks[x, z] = new Chunk(this, coord);
            _activeChunks.Add(coord);
        }

        public BlockConfig GetBlockTypeIndex(VoxelMapType voxelMapType)
        {
            return blockConfigs.First(b => b.voxelMapType == voxelMapType);
        }

        private void InitChunks()
        {
            _spawnPosition = new Vector3(VoxelData.WorldSizeInVoxels / 2f, cameraRig.position.y,
                VoxelData.WorldSizeInVoxels / 2f);
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            _chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];
            _activeChunks = new List<ChunkCoord>();
        }

        private void CheckViewDistance()
        {
            var position = cameraRig.position;
            var coord = position.GetChunkCoordFromPosition();

            var previousActiveChunks = new List<ChunkCoord>(_activeChunks);

            for (var x = coord.x - VoxelData.ViewDistanceInChunks;
                 x < coord.x + VoxelData.ViewDistanceInChunks;
                 x++)
            {
                for (var z = coord.z - VoxelData.ViewDistanceInChunks;
                     z < coord.z + VoxelData.ViewDistanceInChunks;
                     z++)
                {
                    var newCoord = new ChunkCoord(x, z);
                    if (IsChunkInWorld(new ChunkCoord(x, z)))
                    {
                        if (_chunks[x, z] is null)
                        {
                            CreateChunk(x, z);
                            _activeChunks.Add(newCoord);
                        }
                        if (!_chunks[x, z].IsActive)
                        {
                            _chunks[x, z].IsActive = true;
                        }
                    }
                    previousActiveChunks.RemoveAll(c => c == newCoord);
                }
            }
            previousActiveChunks.ForEach(c => _chunks[c.x, c.z].IsActive = false);
        }

        private static bool IsChunkInWorld(ChunkCoord coord)
        {
            var x = coord.x;
            var z = coord.z;

            return x is >= 0 and < VoxelData.WorldSizeInChunks &&
                   z is >= 0 and < VoxelData.WorldSizeInChunks;
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

        public bool CheckPositionInWorld(Vector3 position)
        {
            var xCheck = Mathf.FloorToInt(position.x);
            var yCheck = Mathf.FloorToInt(position.y);
            var zCheck = Mathf.FloorToInt(position.z);

            var xChunk = xCheck / VoxelData.ChunkWidth;
            var zChunk = zCheck / VoxelData.ChunkWidth;
            xCheck -= (xChunk * VoxelData.ChunkWidth);
            zCheck -= (zChunk * VoxelData.ChunkWidth);

            return _chunks[xChunk, zChunk].CheckVoxelInWorld(xCheck, yCheck, zCheck);
        }

    }

}
