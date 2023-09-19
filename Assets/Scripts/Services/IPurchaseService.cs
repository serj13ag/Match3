namespace Services
{
    public interface IPurchaseService : IService
    {
        bool ItemIsPurchased(string itemCode);
        void PurchaseItemAndSave(string itemCode);
    }
}