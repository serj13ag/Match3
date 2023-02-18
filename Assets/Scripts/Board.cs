using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private Tile _tilePrefab;

    private Tile[,] _tiles;

    public int Width => _width;
    public int Height => _height;

    public void SetupTiles()
    {
        _tiles = new Tile[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.name = $"Tile ({i}, {j})";
                tile.transform.SetParent(transform);

                _tiles[i, j] = tile;
            }
        }
    }
}