using System;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LanguagesData", menuName = "StaticData/Languages")]
    public class LanguagesStaticData : ScriptableObject
    {
        public LanguageStaticData[] Languages;
    }

    [Serializable]
    public class LanguageStaticData
    {
        public LanguageType Type;
        public TextAsset Translations;
        public string NameString;
    }
}