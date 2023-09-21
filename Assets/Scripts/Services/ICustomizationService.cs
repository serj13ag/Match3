using UnityEngine;

namespace Services
{
    public interface ICustomizationService : IService
    {
        string CurrentBackgroundItemCode { get; }
        void SetBackgroundItem(string itemCode);
        Color GetCurrentBackgroundColor();
    }
}