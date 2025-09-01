using UnityEngine;

public enum TileType
{
    Path,
    Ground
}

public class TileController : MonoBehaviour
{
    public Vector2Int gridPos;
    public TileType tileType;
    public GameObject towerOnTile;
    public bool IsEmpty => towerOnTile == null && tileType == TileType.Ground;
}