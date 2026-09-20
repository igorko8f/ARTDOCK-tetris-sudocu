using System.Collections.Generic;
using CodeBase.Gameplay.Board.States.Payloads;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using DG.Tweening;

namespace CodeBase.Gameplay.Board.States
{
    public class AnimationState : SimplePayloadState<List<CompletedLine>>
    {
        private readonly IGameStateMachine _stateMachine;

        public AnimationState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Enter(List<CompletedLine> completedLines)
        {
            base.Enter(completedLines);

            PlayDestroyAnimation(completedLines);
            
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
            
            sequence.OnComplete(() => _stateMachine.Enter<IdleState>());
            sequence.Play();
        }
    }
}