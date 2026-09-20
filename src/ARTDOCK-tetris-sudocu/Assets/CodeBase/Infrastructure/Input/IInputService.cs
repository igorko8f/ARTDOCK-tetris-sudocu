using R3;
using UnityEngine;

namespace CodeBase.Infrastructure.Input
{
    public interface IInputService
    {
        bool IsInputEnabled { get; }

        Observable<Unit> RotatePressed { get; }
        Observable<Unit> PausePressed { get; }
        Observable<Unit> MouseClicked { get; }
        Observable<Unit> MouseReleased { get; }
        
        void EnableInput();
        void DisableInput();
        Vector3 GetMouseWorldPosition();
    }
}