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

        private void Start()
        {
            _coinsText.text = _coinService.Coins.ToString();
        }
    }
}