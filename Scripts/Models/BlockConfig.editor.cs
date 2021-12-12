#if UNITY_EDITOR

using System.Threading.Tasks;
using MilkSpun.Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Models
{
    public partial class BlockConfig
    {

        [ShowInInspector, ReadOnly]
        [PreviewField(145f, ObjectFieldAlignment.Right)]
        public Texture2D[] TexturesPreview { get; private set; }

        [OnInspectorInit]
        private async Task ShowTexture()
        {
            if (!isSolid) return;

            await Task.Delay(1);

            TexturesPreview = new Texture2D[2];
            const string path = "Assets/Milkspun/ChunkTerrain/Textures/Tex2dArray0.asset";
            var array = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);

            TexturesPreview[0] =
                TextureBuilder.GetPartTextureFromTexture2DArray(array, page, 16, topFaceTexture);
            TexturesPreview[1] =
                TextureBuilder.GetPartTextureFromTexture2DArray(array, page, 16, leftFaceTexture);
        }

    }
}
#endif
