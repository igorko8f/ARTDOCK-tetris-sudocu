namespace CodeBase.Infrastructure.SaveLoad
{
    public interface ISaveService
    {
        SaveData Data { get; }
        void Load();
        void Save();
        void ResetProgress();
    }
}