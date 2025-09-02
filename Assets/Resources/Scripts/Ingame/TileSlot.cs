[System.Serializable]
public class TileSlot
{
    public int index;
    public TileController tile;
    public Tower tower;

    public TileSlot(int index, TileController tile)
    {
        this.index = index;
        this.tile = tile;
        this.tower = null;
    }

    public bool IsEmpty => tower == null;
}