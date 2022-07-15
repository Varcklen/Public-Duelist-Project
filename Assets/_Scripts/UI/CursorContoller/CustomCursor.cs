using UnityEngine;
using System;

namespace Project.UI.Cursors
{
    [Serializable]
    public struct CustomCursor
    {
        [SerializeField] private Texture2D Texture;
        [SerializeField] private Texture2D TextureClicked;
        public CursorType CursorType;

        public Texture2D GetTexture(CursorTextureType cursorTextureType)
        {
            if (cursorTextureType == CursorTextureType.Clicked)
            {
                return TextureClicked;
            }
            return Texture;
        }
    }

    public enum CursorType : sbyte
    {
        None = -1,
        Default,
        Pointer,
        Lightning,
    }

    public enum CursorTextureType : byte
    {
        Default,
        Clicked
    }
}
