using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkSpun.ChunkWorld.Models;
using MilkSpun.ChunkWorld.Utils;
using MilkSpun.ChunWorld.Extentions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MilkSpun.ChunkWorld.Main
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Chunk : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private int _vertexIndex;
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uv;
        private List<Vector2> _uv2;
        private VoxelMapType[,,] _voxelMap;
        private World _world;

        [OnInspectorInit]
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();

        }

        [Button("生成地块")]
        private void Start()
        {
            _world = GameObject.Find("World").GetComponent<World>();
            InitData();
            PopulateVoxelMap();
            CreateMeshData();
            CreateMesh();
        }

        private bool CheckVoxel(Vector3 pos)
        {
            var x = Mathf.FloorToInt(pos.x);
            var y = Mathf.FloorToInt(pos.y);
            var z = Mathf.FloorToInt(pos.z);

            if (x is < 0 or > VoxelData.ChunkWidth - 1 || y is < 0 or > VoxelData.ChunkHeight - 1 || z is < 0 or > VoxelData.ChunkWidth - 1)
                return false;

            return _world.blockTypes.First(b => b.voxelMapType == _voxelMap[x, y, z]).isSolid;

        }

        private void InitData()
        {
            _vertexIndex = 0;
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uv = new List<Vector2>();
            _uv2 = new List<Vector2>();
            _voxelMap = new VoxelMapType[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
        }

        private void CreateMeshData()
        {
            for (var y = 0; y < VoxelData.ChunkHeight; y++)
            {
                for (var x = 0; x < VoxelData.ChunkWidth; x++)
                {
                    for (var z = 0; z < VoxelData.ChunkWidth; z++)
                    {
                        AddVoxelDataToChunk(new Vector3Int(x, y, z));
                    }
                }
            }
        }

        private void PopulateVoxelMap()
        {
            for (var y = 0; y < VoxelData.ChunkHeight; y++)
            {
                for (var x = 0; x < VoxelData.ChunkWidth; x++)
                {
                    for (var z = 0; z < VoxelData.ChunkWidth; z++)
                    {
                        switch (y)
                        {
                            case VoxelData.ChunkHeight - 1:
                                _voxelMap[x, y, z] = VoxelMapType.Grass;
                                continue;
                            case < 1:
                                _voxelMap[x, y, z] = VoxelMapType.Rock;
                                continue;
                            default:
                                _voxelMap[x, y, z] = VoxelMapType.Dirt;
                                break;
                        }
                    }
                }
            }
        }

        private void AddVoxelDataToChunk(Vector3Int offset)
        {
            for (var p = 0; p < 6; p++)
            {
                if (CheckVoxel(offset + VoxelData.FaceChecks[p])) continue;

                var blockID = _voxelMap[offset.x, offset.y, offset.z];

                for (var i = 0; i < 4; i++)
                {
                    var triangleIndex = VoxelData.VoxelTris[p, i];
                    _vertices.Add(VoxelData.VoxelVerts[triangleIndex] + offset);
                }

                var faceType = (BlockFaceType)p;
                AddTexture(_world.GetBlockTypeIndex(blockID).GetTextureID(faceType));
                _triangles.Add(_vertexIndex);
                _triangles.Add(_vertexIndex + 1);
                _triangles.Add(_vertexIndex + 2);
                _triangles.Add(_vertexIndex + 2);
                _triangles.Add(_vertexIndex + 1);
                _triangles.Add(_vertexIndex + 3);
                _vertexIndex += 4;
            }
        }

        private void CreateMesh()
        {
            var mesh = new Mesh
            {
                vertices = _vertices.ToArray(),
                triangles = _triangles.ToArray(),
                uv = _uv.ToArray(),
                uv2 = _uv2.ToArray()
            };

            mesh.RecalculateNormals();

            _meshFilter.mesh = mesh;
        }

        private void AddTexture(int textureID)
        {
            var id = textureID % VoxelData.TextureAtlasSize;

            var row = id / VoxelData.TextureAtlasSizeInBlocks;
            var invertedRow = (VoxelData.TextureAtlasSizeInBlocks - 1) - row;
            var col = id % VoxelData.TextureAtlasSizeInBlocks;

            var x = col * VoxelData.NormalizedBlockTextureSize;
            var y = invertedRow * VoxelData.NormalizedBlockTextureSize;

            _uv.Add(new Vector2(x, y));
            _uv.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
            _uv.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
            _uv.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));


            var index = textureID / VoxelData.TextureAtlasSize;
            var v = index % 8;
            for (var i = 0; i < 4; i++)
            {
                _uv2.Add(new Vector2(v, v));
            }

        }
    }
}
