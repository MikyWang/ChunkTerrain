using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<Vector2> _uvs;
        private bool[,,] _voxelMap;

        [OnInspectorInit]
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();

        }

        [Button("生成地块")]
        private void Start()
        {

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

            if (x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth - 1)
                return false;

            return _voxelMap[x, y, z];
        }

        private void InitData()
        {
            _vertexIndex = 0;
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uvs = new List<Vector2>();
            _voxelMap = new bool[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
        }

        private void CreateMeshData()
        {
            for (var y = 0; y < VoxelData.ChunkHeight; y++)
            {
                for (var x = 0; x < VoxelData.ChunkWidth; x++)
                {
                    for (var z = 0; z < VoxelData.ChunkWidth; z++)
                    {
                        AddVoxelDataToChunk(new Vector3(x, y, z));
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
                        _voxelMap[x, y, z] = true;
                    }
                }
            }
        }

        private void AddVoxelDataToChunk(Vector3 offset)
        {
            for (var p = 0; p < 6; p++)
            {
                if (CheckVoxel(offset + VoxelData.FaceChecks[p])) continue;

                for (var i = 0; i < 6; i++)
                {
                    var triangleIndex = VoxelData.VoxelTris[p, i];
                    _vertices.Add(VoxelData.VexelVerts[triangleIndex] + offset);
                    _triangles.Add(_vertexIndex);

                    _uvs.Add(VoxelData.VoxelUVs[i]);

                    _vertexIndex++;
                }
            }
        }

        private void CreateMesh()
        {
            var mesh = new Mesh
            {
                vertices = _vertices.ToArray(),
                triangles = _triangles.ToArray(),
                uv = _uvs.ToArray()
            };

            mesh.RecalculateNormals();

            _meshFilter.mesh = mesh;
        }
    }
}
