using System.Collections.Generic;
using Enums;
using PersistentData;
using UnityEngine;

public class GameDataRepository
{
    public Dictionary<GamePieceColor, Color> Colors { get; private set; }

    public MoveInterpolationType MoveInterpolationType { get; }

    public GameDataRepository(ColorData colorData, MoveData moveData)
    {
        SetupColors(colorData);

        MoveInterpolationType = moveData.MoveInterpolationType;
    }

    private void SetupColors(ColorData colorData)
    {
        Colors = new Dictionary<GamePieceColor, Color>();

        foreach (ColorDataEntry colorDataEntry in colorData.GamePieceColors)
        {
            Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
        }
    }
}