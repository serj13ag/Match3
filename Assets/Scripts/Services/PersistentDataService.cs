using System.Collections.Generic;
using Data;
using Interfaces;

namespace Services
{
    public class PersistentDataService : IPersistentDataService
    {
        private readonly ISaveService _saveService;

        private readonly List<IPersistentDataReader> _dataReaders;

        private PlayerData _playerData;

        public PlayerProgress Progress => _playerData.PlayerProgress;
        public Purchases Purchases => _playerData.Purchases;
        public Customizations Customizations => _playerData.Customizations;
        public GameSettings GameSettings => _playerData.GameSettings;

        public PersistentDataService(ISaveService saveService)
        {
            _saveService = saveService;

            _dataReaders = new List<IPersistentDataReader>();
        }

        public void InitWithLoadedData(PlayerData loadedPlayerData)
        {
            _playerData = loadedPlayerData ?? new PlayerData();

            NotifyDataReaders();
        }

        public void RegisterDataReader(IPersistentDataReader reader)
        {
            _dataReaders.Add(reader);
        }

        public void ResetProgressAndSave()
        {
            _playerData.PlayerProgress = new PlayerProgress();

            Save();

            NotifyDataReaders();
        }

        public void Save()
        {
            _saveService.SaveData(_playerData);
        }

        private void NotifyDataReaders()
        {
            foreach (IPersistentDataReader dataReader in _dataReaders)
            {
                dataReader.ReadData(_playerData);
            }
        }
    }
}