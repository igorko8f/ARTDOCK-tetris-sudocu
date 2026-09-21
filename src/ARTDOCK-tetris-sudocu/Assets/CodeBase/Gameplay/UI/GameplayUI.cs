using System;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.PlayerScore;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.UI
{
    public class GameplayUI : UIBase
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TMP_Text _scoreValue;
        [SerializeField] private float _animationDuration = 0.2f;

        private IGameStateMachine _stateMachine;
        private CompositeDisposable _disposable;
        private UIPopups _popups;
        private Tween _scoreTween;

        private long _displayedScore = 0;

        [Inject]
        private void Construct(IInputService inputService,
            IPlayerScoreService playerScoreService,
            IGameStateMachine gameStateMachine,
            UIPopups popups)
        {
            _disposable = new CompositeDisposable();
            _popups = popups;
            _stateMachine = gameStateMachine;
            
            inputService.PausePressed
                .Subscribe(_ => OnPauseButtonClicked())
                .AddTo(_disposable);
            
            playerScoreService.Score
                .Subscribe(UpdateScoreValue)
                .AddTo(_disposable);
            
            _pauseButton.OnClickAsObservable()
                .Subscribe(_ => OnPauseButtonClicked())
                .AddTo(_disposable);
        }

        private void UpdateScoreValue(long score)
        {
            _scoreTween?.Kill();
            var startScore = _displayedScore;

            _scoreTween = DOTween.To(
                    () => startScore,
                    value =>
                    {
                        _displayedScore = value;
                        _scoreValue.text = value.ToString();
                    },
                    score,
                    _animationDuration)
                .SetEase(Ease.OutQuad)
                .SetAutoKill(true)
                .SetTarget(_scoreValue.gameObject);

            _scoreTween.Play();
        }

        private void OnPauseButtonClicked()
        {
            if (_stateMachine.CurrentState is PauseState)
            {
                _popups.CloseAllPopups();
                _stateMachine.Enter<IdleState>();
                return;
            }
            
            _stateMachine.Enter<PauseState>();
            _popups.ShowPausePopup();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}