using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Models
{
    [System.Serializable]
    public class ChunkCoord
    {
        public int x;
        public int z;

        public ChunkCoord(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"Chunk({x},{z})";
        }
    }
}
