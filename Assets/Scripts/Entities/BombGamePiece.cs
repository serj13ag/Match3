using Enums;
using Services;
using StaticData;
using UnityEngine;

namespace Entities
{
    public class BombGamePiece : GamePiece
    {
        private const float TimeTillChangeSpriteColor = 0.2f;

        [SerializeField] private BombType _bombType;

        private float _timeTillChangeSpriteColor;
        private GamePieceColor[] _colors;
        private int _colorIndex;

        public BombType BombType => _bombType;

        public override void Init(string levelName, GamePieceStaticData gamePieceData, GamePieceColor color, int x,
            int y, int offsetY, Transform parentTransform, IStaticDataService staticDataService)
        {
            base.Init(levelName, gamePieceData, color, x, y, offsetY, parentTransform, staticDataService);

            _colors = staticDataService.GetDataForLevel(levelName).AvailableColors;

            _timeTillChangeSpriteColor = TimeTillChangeSpriteColor;
        }

        private void Update()
        {
            if (_bombType != BombType.Color)
            {
                return;
            }

            if (_timeTillChangeSpriteColor < 0)
            {
                SetSpriteColor(_colors[_colorIndex]);
                IterateColorIndex();
                _timeTillChangeSpriteColor = TimeTillChangeSpriteColor;
            }
            else
            {
                _timeTillChangeSpriteColor -= Time.deltaTime;
            }
        }

        private void IterateColorIndex()
        {
            _colorIndex = _colorIndex < _colors.Length - 1
                ? _colorIndex + 1
                : 0;
        }
    }
}