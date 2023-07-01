using System;
using System.Collections.Generic;
using Data;
using Enums;
using EventArguments;
using Helpers;
using Interfaces;
using StaticData.StartingData;
using UnityEngine;

namespace Services
{
    public class TileService : ITileService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IGameFactory _gameFactory;

        private readonly string _levelName;

        private readonly ITile[,] _tiles;

        private ITile _clickedTile;
        private ITile _targetTile;

        public event EventHandler<MoveRequestedEventArgs> OnMoveRequested;

        public TileService(string levelName, IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService, IGameFactory gameFactory)
        {
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _gameFactory = gameFactory;

            _levelName = levelName;

            _tiles = new ITile[staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight];
        }

        public void Initialize()
        {
            StartingTilesStaticData startingTilesStaticData = _staticDataService.GetDataForLevel(_levelName)?.StartingTiles;
            if (startingTilesStaticData != null)
            {
                foreach (StartingTileStaticData startingTile in startingTilesStaticData.StartingTiles)
                {
                    SpawnTile(startingTile.Type, startingTile.X, startingTile.Y);
                }
            }

            for (int i = 0; i < _tiles.GetWidth(); i++)
            {
                for (int j = 0; j < _tiles.GetHeight(); j++)
                {
                    if (!TryGetTileAt(i, j, out _))
                    {
                        SpawnTile(TileType.Normal, i, j);
                    }
                }
            }
        }

        public void Initialize(List<TileSaveData> tiles)
        {
            foreach (TileSaveData tile in tiles)
            {
                SpawnTile(tile.Type, tile.Position.x, tile.Position.y);
            }
        }

        public void UpdateProgress()
        {
            List<TileSaveData> tilesSaveData = new List<TileSaveData>();

            foreach (ITile tile in _tiles)
            {
                tilesSaveData.Add(new TileSaveData(tile.Type, tile.Position));
            }

            _persistentProgressService.Progress.BoardData.LevelBoardData.Tiles = tilesSaveData;
        }

        public void ProcessTileMatchAt(Vector2Int position)
        {
            if (TryGetTileAt(position.x, position.y, out ITile tile))
            {
                tile.ProcessMatch();
            }
        }

        public bool IsObstacleAt(int column, int row)
        {
            return _tiles[column, row].IsObstacle;
        }

        private void SpawnTile(TileType tileType, int x, int y)
        {
            ITile tile = _gameFactory.CreateTile(tileType, x, y);
            RegisterTile(x, y, tile);
        }

        private void RegisterTile(int x, int y, ITile tile)
        {
            tile.OnClicked += OnTileClicked;
            tile.OnMouseEntered += OnTileMouseEntered;
            tile.OnMouseReleased += OnTileMouseReleased;

            _tiles[x, y] = tile;
        }

        private void OnTileClicked(ITile tile)
        {
            _clickedTile ??= tile;
        }

        private void OnTileMouseEntered(ITile tile)
        {
            if (_clickedTile != null && TileHelper.IsNeighbours(_clickedTile, tile))
            {
                _targetTile = tile;
            }
        }

        private void OnTileMouseReleased()
        {
            if (_clickedTile != null && _targetTile != null)
            {
                OnMoveRequested?.Invoke(this, new MoveRequestedEventArgs(_clickedTile.Position, _targetTile.Position));
            }

            _clickedTile = null;
            _targetTile = null;
        }

        private bool TryGetTileAt(int x, int y, out ITile tile)
        {
            tile = _tiles[x, y];

            return tile != null;
        }
    }
}