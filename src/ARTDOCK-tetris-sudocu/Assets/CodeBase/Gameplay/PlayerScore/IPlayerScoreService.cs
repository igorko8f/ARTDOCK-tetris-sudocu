using System.Collections.Generic;
using R3;

namespace CodeBase.Gameplay.PlayerScore
{
    public interface IPlayerScoreService
    {
        public ReadOnlyReactiveProperty<long> Score { get; }
        int AmountPerCell { get; }


        long[] GetActualRecords();
        void AddScore(long value);
        void Reset();
        void UpdateNewRecords();
    }
}