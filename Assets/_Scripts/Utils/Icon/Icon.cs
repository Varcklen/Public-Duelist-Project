using UnityEngine;
using System;

namespace Project.Utils.IconNS
{
    [Serializable]
    public struct Icon
    {
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
        [SerializeField] private Vector2 _deviation;
        public Vector2 Deviation => _deviation;

        public Icon(Sprite sprite, Vector2 deviation)
        {
            _sprite = sprite;
            _deviation = deviation;
        }
    }
}
