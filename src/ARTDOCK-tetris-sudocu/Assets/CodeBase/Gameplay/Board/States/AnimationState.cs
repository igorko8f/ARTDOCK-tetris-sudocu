using System.Collections.Generic;
using CodeBase.Gameplay.PlayerScore;
using CodeBase.Infrastructure.Audio;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using DG.Tweening;

namespace CodeBase.Gameplay.Board.States
{
    public class AnimationState : SimplePayloadState<List<CompletedLine>>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IProjectResourcesProvider _resourcesProvider;
        private readonly IAudioService _audioService;
        private readonly IPlayerScoreService _playerScoreService;
        public AnimationState(IGameStateMachine stateMachine,
            IProjectResourcesProvider resourcesProvider,
            IPlayerScoreService playerScoreService,
            IAudioService audioService)
        {
            _stateMachine = stateMachine;
            _resourcesProvider = resourcesProvider;
            _playerScoreService = playerScoreService;
            _audioService = audioService;
        }

        public override void Enter(List<CompletedLine> completedLines)
        {
            base.Enter(completedLines);

            CalculateAndAddPlayerScore(completedLines);
            PlayDestroyAnimation(completedLines);
            PlayDestroySFX();
        }

        private void CalculateAndAddPlayerScore(List<CompletedLine> completedLines)
        {
            long scoreToAdd = 0;

            for (var i = 0; i < completedLines.Count; i++) 
                scoreToAdd += completedLines[i].Line.Count * _playerScoreService.AmountPerCell;
            
            _playerScoreService.AddScore(scoreToAdd);
        }

        private void PlayDestroySFX()
        {
            var soundConfig = _resourcesProvider.LoadResource<SoundsConfig>();
            _audioService.PlaySfx(soundConfig.DestroySFX);
        }

        private void PlayDestroyAnimation(List<CompletedLine> completedLines)
        {
            var sequence = DOTween.Sequence();
            
            foreach (var completedLine in completedLines)
            {
                var lineSequence = DOTween.Sequence();
                lineSequence.SetTarget(this);
                lineSequence.SetAutoKill(true);

                foreach (var cell in completedLine.Line) 
                    lineSequence.Append(cell.DestroyAnimation());
                
                sequence.Join(lineSequence);
            }
            
            sequence.OnComplete(() => _stateMachine.Enter<CheckLoseState>());
            sequence.Play();
        }
    }
}