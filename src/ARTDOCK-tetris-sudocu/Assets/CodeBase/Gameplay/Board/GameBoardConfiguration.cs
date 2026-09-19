using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;

namespace CodeBase.Gameplay.Board
{
    [CreateAssetMenu(fileName = "GameBoardConfiguration", menuName = "Gameplay/GameBoard/Configuration")]
    public class GameBoardConfiguration : ScriptableObject, IResource
    {
        public int Board_With = 8;
        public int Board_Height = 8;
    }
}