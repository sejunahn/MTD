using UnityEngine;


public static class EnemyFactory
{
    public static IEnemy Create(string id)
    {
        return id switch
        {
            "M001" => new Slime(),
            
            _ => null
        };
    }
}