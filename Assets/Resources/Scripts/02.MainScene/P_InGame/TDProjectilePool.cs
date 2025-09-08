using System.Collections.Generic;
using UnityEngine;

public class TDProjectilePool : MonoBehaviour
{
    public static TDProjectilePool Instance;

    public TDProjectile projectilePrefab;
    public int poolSize = 50;

    private Queue<TDProjectile> pool = new Queue<TDProjectile>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            TDProjectile proj = Instantiate(projectilePrefab, transform);
            proj.gameObject.SetActive(false);
            proj.SetPool(this); // 총알이 자신을 풀에 반환할 수 있도록
            pool.Enqueue(proj);
        }
    }

    public TDProjectile GetProjectile()
    {
        TDProjectile proj;
        if (pool.Count > 0)
        {
            proj = pool.Dequeue();
        }
        else
        {
            proj = Instantiate(projectilePrefab, transform);
            proj.SetPool(this);
        }

        proj.gameObject.SetActive(true);
        return proj;
    }

    public void ReturnProjectile(TDProjectile proj)
    {
        proj.gameObject.SetActive(false);
        pool.Enqueue(proj);
    }
}
