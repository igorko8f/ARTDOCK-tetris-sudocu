using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Board;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.SaveLoad;
using R3;

namespace CodeBase.Gameplay.PlayerScore
{
    public class PlayerScoreService: IPlayerScoreService, IDisposable
    {
        public ReadOnlyReactiveProperty<long> Score => _score;
        public int AmountPerCell { get; private set; } = 0;

        private readonly ISaveService _saveService;
        private readonly ReactiveProperty<long> _score;

        public PlayerScoreService(IProjectResourcesProvider resourcesProvider,
            ISaveService saveService)
        {
            _saveService = saveService;
            
            AmountPerCell = resourcesProvider
                .LoadResource<GameBoardConfiguration>()
                .ScoreAmountPerCell;
            
            _score = new ReactiveProperty<long>(0);
        }

        public long[] GetActualRecords()
        {
            return _saveService.Data.BestScore;
        }

        public void AddScore(long value)
        {
            _score.Value += value;
        }

        public void Reset()
        {
            _score.Value = 0;
        }

        public void UpdateNewRecords()
        {
            var currentRecords = _saveService.Data.BestScore;

            for (var i = 0; i < currentRecords.Length; i++)
            {
                if (_score.Value <= currentRecords[i]) continue;
                
                for (var j = currentRecords.Length - 1; j > i; j--) 
                    currentRecords[j] = currentRecords[j - 1];

                currentRecords[i] = _score.Value;
                break;
            }
            
            _saveService.Save();
        }

        public void Dispose()
        {
            _score.Dispose();
        }
    }
}