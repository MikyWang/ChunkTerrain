using System.Collections;
using System.Collections.Generic;
using MilkSpun.ChunkWorld.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BlockConfig", menuName = "MilkSpun/创建Block")]
public class BlockConfig : ScriptableObject
{
    [Title("基本设置")]
    [Tooltip("方块类型")]
    public VoxelMapType voxelMapType;
    [Tooltip("是否为固体.")]
    public bool isSolid;
    [Title("纹理ID")]
    [Tooltip("后面纹理")]
    public int backFaceTexture;
    [Tooltip("前面纹理")]
    public int frontFaceTexture;
    [Tooltip("顶部纹理")]
    public int topFaceTexture;
    [Tooltip("底部纹理")]
    public int bottomFaceTexture;
    [Tooltip("左面纹理")]
    public int leftFaceTexture;
    [Tooltip("右面纹理")]
    public int rightFaceTexture;
}
