using System.Collections.Generic;
using UnityEngine;

public class TileGridManager : MonoBehaviour
{
    public static TileGridManager Instance; 
    [SerializeField] private TileController[] tiles;

    public List<TileSlot> Grid = new List<TileSlot>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < tiles.Length; i++)
        {
            Grid.Add(new TileSlot(i, tiles[i]));
        }
    }

    public TileSlot GetSlotFromTile(TileController tile)
    {
        return Grid.Find(s => s.tile == tile);
    }
}
