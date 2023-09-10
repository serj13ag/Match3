mergeInto(LibraryManager.library, {

  InitYandexSDK: function () {
    InitYandexSDK();
  },

  SaveToPlayerData: function (key, jsonDataString) {
    SaveToPlayerData(UTF8ToString(key), UTF8ToString(jsonDataString));
  },

  LoadFromPlayerData: function (key) {
    LoadFromPlayerData(UTF8ToString(key));
  },

  ShowFullAd: function () {
    ShowFullAd();
  },

  ShowRewardedAd: function () {
    ShowRewardedAd();
  },
});