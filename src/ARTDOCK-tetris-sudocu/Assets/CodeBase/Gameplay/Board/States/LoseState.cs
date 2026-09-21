using CodeBase.Gameplay.PlayerScore;
using CodeBase.Gameplay.UI;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using UnityEngine;

namespace CodeBase.Gameplay.Board.States
{
    public class LoseState : SimpleState
    {
        private readonly IPlayerScoreService _playerScoreService;
        private readonly UIPopups _popups;

        public LoseState(IPlayerScoreService playerScoreService,
            UIPopups popups)
        {
            _playerScoreService = playerScoreService;
            _popups = popups;
        }
        
        public override void Enter()
        {
            base.Enter();

            _playerScoreService.UpdateNewRecords();
            _popups.ShowLoseGamePopup();
        }
    }
}