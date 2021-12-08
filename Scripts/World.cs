using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkSpun.ChunkWorld.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Main
{
    public class World : MonoBehaviour
    {
        public Material material;
        [InlineEditor, Space]
        public BlockConfig[] blockTypes;


        public BlockConfig GetBlockTypeIndex(VoxelMapType voxelMapType)
        {
            return blockTypes.First(b => b.voxelMapType == voxelMapType);
        }

    }

}
