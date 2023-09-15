using Data;

namespace Services
{
    public interface ISaveService : IService
    {
        void SaveData(PlayerData playerData);
    }
}