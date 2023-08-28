using EventArguments;
using Infrastructure;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class CoinsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;

        private ICoinService _coinService;

        private void Awake()
        {
            _coinService = ServiceLocator.Instance.Get<ICoinService>();
        }

        private void OnEnable()
        {
            _coinService.OnCoinsChanged += OnCoinsChanged;
        }

        private void Start()
        {
            UpdateCoinsText(_coinService.Coins);
        }

        private void OnCoinsChanged(object sender, CoinsChangedEventArgs e)
        {
            UpdateCoinsText(e.Coins);
        }

        private void UpdateCoinsText(int coins)
        {
            _coinsText.text = coins.ToString();
        }

        private void OnDisable()
        {
            _coinService.OnCoinsChanged -= OnCoinsChanged;
        }
    }
}