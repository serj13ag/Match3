mergeInto(LibraryManager.library, {

  SaveToPlayerData: function (key, jsonDataString) {
    SaveToPlayerData(UTF8ToString(key), UTF8ToString(jsonDataString));
  },

  LoadFromPlayerData: function () {
    LoadFromPlayerData();
  },

  ShowFullAd: function () {
    ShowFullAd();
  },

  ShowRewardedAd: function () {
    ShowRewardedAd();
  },
});