using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Models
{
    [System.Serializable]
    public struct ChunkCoord
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

        public static bool operator ==(ChunkCoord a, ChunkCoord b)
        {
            return a.x == b.x && a.z == b.z;
        }
        public static bool operator !=(ChunkCoord a, ChunkCoord b)
        {
            return !(a == b);
        }
    }
}
