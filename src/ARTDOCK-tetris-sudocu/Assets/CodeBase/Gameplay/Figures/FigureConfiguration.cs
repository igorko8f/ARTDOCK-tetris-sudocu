using System;
using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;

namespace CodeBase.Gameplay.Figures
{
    [CreateAssetMenu(fileName = "Figure", menuName = "Gameplay/Figure/Empty")]
    public class FigureConfiguration : ScriptableObject, ISerializationCallbackReceiver, IResource
    {
        public const int Size = 5;
        public bool[,] Matrix => _matrix;
        
        [NonSerialized]
        private bool[,] _matrix = new bool[Size, Size];

        [SerializeField]
        private bool[] _serializedMatrix = new bool[Size * Size];
        
        public void OnBeforeSerialize()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    _serializedMatrix[y * Size + x] = _matrix[x, y];
                }
            }
        }

        public void OnAfterDeserialize()
        {
            _matrix = new bool[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    _matrix[x, y] = _serializedMatrix[y * Size + x];
                }
            }
        }
    }
}