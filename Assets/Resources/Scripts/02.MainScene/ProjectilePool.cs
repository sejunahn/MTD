using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewProjectile();
        }
    }

    private void CreateNewProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, transform);
        proj.SetActive(false);
        pool.Enqueue(proj);
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
            CreateNewProjectile();

        GameObject proj = pool.Dequeue();
        proj.SetActive(true);
        return proj;
    }

    public void Return(GameObject proj)
    {
        proj.SetActive(false);
        pool.Enqueue(proj);
    }
}