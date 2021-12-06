using System.Collections;
using System.Collections.Generic;
using MilkSpun.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Main
{
    public class TextureManager : MonoBehaviour
    {
        [SerializeField]
        private List<Texture2D> textures;
        [SerializeField]
        private MeshRenderer cubeRender;


        [Button("生成2DArray")]
        private void GenerateArray()
        {
            var array = new Texture2DArray(textures[0].width, textures[0].height, textures.Count, textures[0].format, true)
            {
                wrapMode = TextureWrapMode.Clamp
            };

            for (var i = 0; i < textures.Count; i++)
            {
                for (var m = 0; m < textures[i].mipmapCount; m++)
                {
                    Graphics.CopyTexture(textures[i], 0, m, array, i, m);
                }
            }
            
            TextureBuilder.SaveTexture2DArrayToAsset(array, "TerrainArray");

        }

    }
}
