using UnityEngine;

public static class TowerFactory
{
    public static ITower Create(string id)
    {
        return id switch
        {
            "T001" => new ArrowTower(),
            _ => null
        };
    }
}