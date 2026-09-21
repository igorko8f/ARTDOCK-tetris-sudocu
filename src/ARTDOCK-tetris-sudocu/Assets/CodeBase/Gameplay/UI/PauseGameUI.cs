using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.PlayerScore;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.UI
{
    public class PauseGameUI : UIBase
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        private CompositeDisposable _disposable;
        private IGameStateMachine _stateMachine;
        private IPlayerScoreService _playerScoreService;

        [Inject]
        private void Construct(IGameStateMachine stateMachine,
            IPlayerScoreService playerScoreService)
        {
            _stateMachine = stateMachine;
            _playerScoreService = playerScoreService;
            _disposable = new CompositeDisposable();
            
            _resumeButton.OnClickAsObservable()
                .Subscribe(_ => ResumeGame())
                .AddTo(_disposable);
            
            _quitButton.OnClickAsObservable()
                .Subscribe(_ => QuitGame())
                .AddTo(_disposable);
        }

        private void ResumeGame()
        {
            Hide();
            _stateMachine.Enter<IdleState>();
        }

        private void QuitGame()
        {
            _playerScoreService.UpdateNewRecords();
            Application.Quit();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}