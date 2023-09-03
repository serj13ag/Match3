using Infrastructure;
using Services;
using TMPro;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(TMP_Text))]
    public class TranslationTMPComponent : MonoBehaviour
    {
        [SerializeField] private string _key;

        private ILocalizationService _localizationService;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();

            _localizationService = ServiceLocator.Instance.Get<ILocalizationService>();
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(_key))
            {
                _text.text = _localizationService.GetTranslation(_key);
            }
        }
    }
}