using System;
using Data;
using Enums;
using EventArguments;
using Helpers;
using Interfaces;
using StaticData;
using StaticData.StartingData;
using UnityEngine;

namespace Services
{
    public class TileService : ITileService
    {
        private readonly IGameFactory _gameFactory;

        private readonly ITile[,] _tiles;

        private ITile _clickedTile;
        private ITile _targetTile;

        public ITile[,] Tiles => _tiles;

        public event EventHandler<MoveRequestedEventArgs> OnMoveRequested;

        public TileService(string levelName, IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;

            int width = staticDataService.Settings.BoardWidth;
            int height = staticDataService.Settings.BoardHeight;

            _tiles = new ITile[width, height];

            LevelBoardData levelBoardData = persistentProgressService.Progress.BoardData.LevelBoardData;
            if (levelName == levelBoardData.LevelName && levelBoardData.Tiles != null)
            {
                SetupFromLoadData(levelBoardData);
            }
            else
            {
                SetupNewBoard(staticDataService.GetDataForLevel(levelName), width, height);
            }
        }

        public void ProcessTileMatchAt(Vector2Int position)
        {
            if (TryGetTileAt(position.x, position.y, out ITile tile))
            {
                tile.ProcessMatch();
            }
        }

        private void SetupFromLoadData(LevelBoardData levelBoardData)
        {
            foreach (TileSaveData tile in levelBoardData.Tiles)
            {
                SpawnTile(tile.Type, tile.Position.x, tile.Position.y);
            }
        }

        private void SetupNewBoard(LevelStaticData levelStaticData, int width, int height)
        {
            foreach (StartingTileStaticData startingTile in levelStaticData.StartingTiles.StartingTiles)
            {
                SpawnTile(startingTile.Type, startingTile.X, startingTile.Y);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!TryGetTileAt(i, j, out _))
                    {
                        SpawnTile(TileType.Normal, i, j);
                    }
                }
            }
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