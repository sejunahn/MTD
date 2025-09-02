using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile prefab;
    [SerializeField] private int initialSize = 20;

    private Queue<Projectile> pool = new Queue<Projectile>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            Projectile p = Instantiate(prefab, transform);
            p.gameObject.SetActive(false);
            pool.Enqueue(p);
        }
    }

    public Projectile Get()
    {
        if (pool.Count > 0)
            return pool.Dequeue();

        Projectile p = Instantiate(prefab, transform);
        p.gameObject.SetActive(false);
        return p;
    }

    public void Return(Projectile p)
    {
        p.gameObject.SetActive(false);
        pool.Enqueue(p);
    }
}