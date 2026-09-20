using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using UnityEngine;

namespace CodeBase.Gameplay.Board.States
{
    public class LoseState : SimpleState
    {
        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Game Over! You have lost the game.");
        }
    }
}