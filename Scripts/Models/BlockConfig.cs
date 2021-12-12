using System.Collections;
using System.Collections.Generic;
using MilkSpun.ChunkWorld.Models;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

namespace MilkSpun.ChunkWorld.Models
{
    [CreateAssetMenu(fileName = "Config", menuName = "MilkSpun/创建Block")]
    public partial class BlockConfig : ScriptableObject
    {
        [Title("基本设置")] [Tooltip("方块类型")] public VoxelMapType voxelMapType;
        [Tooltip("是否为固体.")] public bool isSolid;
        [Title("纹理ID")]
        [Tooltip("使用第几页纹理")]
        [Range(0, 8)]
        public int page;
        [Tooltip("后面纹理"), OnValueChanged("ShowTexture")]
        public int backFaceTexture;
        [Tooltip("前面纹理"), OnValueChanged("ShowTexture")]
        public int frontFaceTexture;
        [Tooltip("顶部纹理"), OnValueChanged("ShowTexture")]
        public int topFaceTexture;
        [Tooltip("底部纹理"), OnValueChanged("ShowTexture")]
        public int bottomFaceTexture;
        [Tooltip("左面纹理"), OnValueChanged("ShowTexture")]
        public int leftFaceTexture;
        [Tooltip("右面纹理"), OnValueChanged("ShowTexture")]
        public int rightFaceTexture;
    }

    public enum BlockFaceType : byte
    {
        Back,
        Front,
        Top,
        Bottom,
        Left,
        Right
    }

    public enum VoxelMapType : byte
    {
        Air,
        LightGrass,
        Grass,
        Dirt,
        Stone,
        BedRock,
        Sand
    }

}
