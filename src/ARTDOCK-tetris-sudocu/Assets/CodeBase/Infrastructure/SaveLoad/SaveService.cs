using System;
using CodeBase.Infrastructure.SaveLoad.Data;
using CodeBase.Infrastructure.SaveLoad.Serialization;
using CodeBase.Infrastructure.SaveLoad.Storage;

namespace CodeBase.Infrastructure.SaveLoad
{
    public class SaveService : ISaveService
    {
        public SaveData Data => _saveData;

        private readonly ISaveStorage _saveStorage;
        private readonly ISaveSerializer _saveSerializer;

        private SaveData _saveData;
        
        public SaveService()
        {
            _saveStorage = new FileSaveStorage();
            _saveSerializer = new NewtonsoftSaveSerializer();
        }

        public void Load()
        {
            if (_saveStorage.Exists() == false)
                _saveData = new SaveData();

            var payload = _saveStorage.Read();
            if (string.IsNullOrWhiteSpace(payload))
                _saveData = new SaveData();

            _saveData = _saveSerializer.Deserialize<SaveData>(payload) ?? new SaveData();
        }

        public void Save()
        {
            if (_saveData == null)
                throw new ArgumentNullException(nameof(_saveData));

            var payload = _saveSerializer.Serialize(_saveData);
            _saveStorage.Write(payload);
        }
        
        public void ResetProgress()
        {
            _saveData = new SaveData();
            Save();
        }
    }
}