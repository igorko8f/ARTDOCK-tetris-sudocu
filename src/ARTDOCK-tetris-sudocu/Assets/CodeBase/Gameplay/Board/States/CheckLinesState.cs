using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Board.States.Payloads;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class CheckLinesState : SimplePayloadState<IEnumerable<(int x, int y)>>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GameBoard _board;

        public CheckLinesState(IGameStateMachine stateMachine,
            GameBoard board)
        {
            _stateMachine = stateMachine;
            _board = board;
        }

        public override void Enter(IEnumerable<(int x, int y)> positions)
        {
            base.Enter(positions);
            
            var completedLines = _board.GetCompletedLines(positions);
            if (!completedLines.Any())
            {
                _stateMachine.Enter<IdleState>();
            }
            else
            {
                _stateMachine.Enter<AnimationState, List<CompletedLine>>(completedLines);
            }
        }
    }
}