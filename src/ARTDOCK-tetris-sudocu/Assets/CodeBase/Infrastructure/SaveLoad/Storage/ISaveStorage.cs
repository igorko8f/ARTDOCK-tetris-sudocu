namespace CodeBase.Infrastructure.SaveLoad.Storage
{
    public interface ISaveStorage
    {
        bool Exists();
        string Read();
        void Write(string payload);
        void Delete();
        string GetAbsolutePath();
    }
}


