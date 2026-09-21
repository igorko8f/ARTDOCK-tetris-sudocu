using System.Collections.Generic;
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

        public AnimationState(IGameStateMachine stateMachine,
            IProjectResourcesProvider resourcesProvider,
            IAudioService audioService)
        {
            _stateMachine = stateMachine;
            _resourcesProvider = resourcesProvider;
            _audioService = audioService;
        }

        public override void Enter(List<CompletedLine> completedLines)
        {
            base.Enter(completedLines);

            PlayDestroyAnimation(completedLines);
            PlayDestroySFX();

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