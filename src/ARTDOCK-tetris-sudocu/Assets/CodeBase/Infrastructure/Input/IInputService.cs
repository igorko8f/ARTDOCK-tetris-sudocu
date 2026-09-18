using R3;

namespace CodeBase.Infrastructure.Input
{
    public interface IInputService
    {
        bool IsInputEnabled { get; }

        Observable<Unit> RotatePressed { get; }
        Observable<Unit> PausePressed { get; }
        Observable<Unit> MouseClicked { get; }
        
        void EnableInput();
        void DisableInput();
    }
}