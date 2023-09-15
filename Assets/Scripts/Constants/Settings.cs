namespace Constants
{
    public static class Settings
    {
        public const string BootstrapSceneName = "BootstrapScene";
        public const string MainMenuSceneName = "MainMenuScene";
        public const string GameLevelScene = "GameLevelScene";

        public const string EndlessLevelName = "EndlessLevel";

        public const string SavedPlayerDataKey = "SavedPlayerData";

        public const bool CollectibleGamePieceEnabled = false;

        public const float CameraPositionZ = -10f;

        public const float TimeToMoveGamePiece = 0.3f;
        public const int MinMatchesCount = 3;

        public const int BombAdjacentGamePiecesRange = 2;
        public const int MatchesToSpawnBomb = 4;
        public const int MatchesToSpawnColorBomb = 5;

        public const int MaxCollectibles = 5;
        public const int PercentChanceToSpawnCollectible = 50;

        public const int FillBoardOffsetY = 5;

        public const float SecondsTillShowHint = 5f;
        public const float SecondsBetweenReshowHint = 2f;

        public static class Timeouts
        {
            public const float FillBoardTimeout = 0f;
            public const float ClearGamePiecesTimeout = 0.3f;
            public const float CollapseColumnsTimeout = 0.3f;
        }

        public static class ScreenFader
        {
            public const float SolidAlpha = 1;
            public const float ClearAlpha = 0;
            public const float Delay = 1f;
            public const float TimeToFade = 1f;
        }

        public static class Sound
        {
            public const float LowPitch = 0.95f;
            public const float HighPitch = 1.05f;
            public const float MusicVolume = 0.5f;
            public const float FxVolume = 0.5f;
            public const int AdditionalTimeBeforeDestroyAudioSource = 1;
        }

        public static class Score
        {
            public const int UpdateScoreTextIncrement = 5;
            public const float ScoreTextUpdateIntervalInSeconds = 0.01f;
            public const int MinNumberOfBreakGamePiecesToGrantBonus = 4;
            public const int BonusScore = 20;
        }
    }
}