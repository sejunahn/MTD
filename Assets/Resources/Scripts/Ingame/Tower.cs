using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    private int TowerSerial;
    private TowerData data;
    private GameSceneManager gm;
    private float attackCooldown;
    
    public int Level { get; private set; } = 1;

    private Camera mainCam;
    private bool isDragging = false;
    private float zOffset;

    private TileController currentTile;
    

    public void Initialize(TowerData data, int towerSerial, GameSceneManager gm)
    {
        this.data = data;
        this.TowerSerial = towerSerial;
        this.gm = gm;
        mainCam = Camera.main;
        attackCooldown = 1f / data.attackSpeed;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            Enemy target = FindTarget();
            if (target != null)
            {
                Shoot(target);
                yield return new WaitForSeconds(attackCooldown);
            }
            else yield return null;
        }
    }

    private Enemy FindTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist <= data.attackRange && dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }
        return nearest;
    }

    private void Shoot(Enemy target)
    {
        Projectile p = gm.GetProjectile();
        p.transform.position = transform.position;
        p.Fire(target, data.attackPower, gm);
    }

    #region TowerMovement
     void OnMouseDown()
    {
        isDragging = true;
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;
        currentTile = GetTileUnderPosition(transform.position);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        isDragging = false;

        TileController targetTile = GetTileUnderPosition(transform.position);

        // 타일이 아니면 원래 자리 복귀
        if (targetTile == null || targetTile.tileType != TileType.Ground)
        {
            SnapBack();
            return;
        }

        // 같은 자리 → 복귀
        if (targetTile == currentTile)
        {
            SnapBack();
            return;
        }

        // 빈 자리 → 이동
        if (targetTile.IsEmpty)
        {
            MoveToTile(targetTile);
        }
        else
        {
            // 다른 타워와 합성 or 교환
            TryMergeOrSwap(targetTile);
        }
    }

    private TileController GetTileUnderPosition(Vector3 pos)
    {
        // 밑으로 Raycast (오브젝트 발 밑에서 땅을 향해 쏨)
        Ray ray = new Ray(pos + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }
    
    private void SnapBack()
    {
        if (currentTile != null)
            transform.position = currentTile.transform.position;
    }

    private void MoveToTile(TileController tile)
    {
        if (currentTile != null) currentTile.towerOnTile = null;

        transform.position = tile.transform.position;
        tile.towerOnTile = gameObject;
        currentTile = tile;
    }

    private void TryMergeOrSwap(TileController tile)
    {
        GameObject other = tile.towerOnTile;
        Tower otherTower = other?.GetComponent<Tower>();

        if (otherTower != null && otherTower.data.id == this.data.id)
        {
            // 합성 (업그레이드)
            Destroy(other);
            Destroy(gameObject);

            GameObject merged = Instantiate(gm.TowerPrefab,
                                            tile.transform.position, Quaternion.identity);

            tile.towerOnTile = merged;
        }
        else
        {
            // 교환
            Vector3 tempPos = other.transform.position;
            other.transform.position = currentTile.transform.position;
            transform.position = tempPos;

            currentTile.towerOnTile = other;
            tile.towerOnTile = gameObject;
            currentTile = tile;
        }
    }
    #endregion
}