using System;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.PlayerScore;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.UI
{
    public class LoseGameUI : UIBase
    {
        [SerializeField] private TMP_Text[] _bestScoreTexts;
        [SerializeField] private TMP_Text _currentResult;
        [SerializeField] private Button _restartButton;

        private CompositeDisposable _disposable;
        private IPlayerScoreService _playerScore;
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(IPlayerScoreService playerScoreService,
            IGameStateMachine gameStateMachine)
        {
            _playerScore = playerScoreService;
            _stateMachine = gameStateMachine;
            _disposable = new CompositeDisposable();
            
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => RestartGame())
                .AddTo(_disposable);
        }

        public override void Show()
        {
            ShowBestResults();
            base.Show();
        }

        private void ShowBestResults()
        {
            _currentResult.text = _playerScore.Score.CurrentValue.ToString();
            
            var bestScore = _playerScore.GetActualRecords();
            var scoreCount = Mathf.Clamp(_bestScoreTexts.Length, 0, bestScore.Length);
            for (var i = 0; i < scoreCount; i++)
            {
                var scoreText = _bestScoreTexts[i];
                scoreText.text = bestScore[i].ToString();
            }
        }
        
        private void RestartGame()
        {
            _stateMachine.Enter<RestartState>();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}