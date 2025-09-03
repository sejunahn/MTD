using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    private TowerData data;
    private GameSceneManager gm;

    private float attackCooldown;
    private Camera mainCam;
    private bool isDragging;
    private float zOffset;
    private TileController currentTile;

    // ✅ 구분용: 같은 종류지만 Serial 다르면 합성 불가
    private int towerSerial;      
    public int TowerSerial => towerSerial;

    public int Level { get; private set; } = 1;

    public void Initialize(TowerData data, int serial, GameSceneManager gm)
    {
        this.data = data;
        this.towerSerial = serial;
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
            else
            {
                yield return null;
            }
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

    #region Drag & Drop
    private void OnMouseDown()
    {
        isDragging = true;
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;
        currentTile = GetTileUnderPosition(); //GetTileUnderPosition(transform.position);
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        TileController targetTile = GetTileUnderPosition();//GetTileUnderPosition(transform.position);

        // ⬇️ 타일이 없거나 Ground가 아니라면 복귀
        if (targetTile == null || targetTile.tileType != TileType.Ground)
        {
            Debug.Log("타일이 없거나 그라운드가 아님");
            SnapBack();
            return;
        }

        // 같은 자리라면 복귀
        if (targetTile == currentTile)
        {
            Debug.Log("같은자리임");
            SnapBack();
            return;
        }

        if (targetTile.IsEmpty)
        {
            Debug.Log("타일이 비어있다");
            MoveToTile(targetTile);
        }
        else
        {
            Debug.Log("머지한다");
            TryMergeOrSwap(targetTile);
        }
    }
    
    

    private TileController GetTileUnderPosition(Vector3 pos)
    {
        Ray ray = new Ray(pos + Vector3.up * 5f, Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            Debug.Log("Hit at: " + hit.point + " | Tile pos: " + hit.collider.transform.position);
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }

    private TileController GetTileUnderPosition()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
    
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.Log("Hit at: " + hit.point + " | Tile pos: " + hit.collider.transform.position);
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }

    private void SnapBack()
    {
        if (currentTile != null)
        {
            Vector3 snapPos = currentTile.GetComponent<BoxCollider>().bounds.center;
            transform.position = snapPos;
        }
        // transform.position = currentTile.transform.position;
    }

    private void MoveToTile(TileController tile)
    {
        if (currentTile != null)
            currentTile.towerOnTile = null;

        transform.position = tile.transform.position;
        tile.towerOnTile = gameObject;
        currentTile = tile;
    }

    private void TryMergeOrSwap(TileController tile)
    {
        GameObject other = tile.towerOnTile;
        Tower otherTower = other?.GetComponent<Tower>();

        if (otherTower != null)
        {
            // ✅ 같은 종류 && Serial 다르면 합성 가능
            if (otherTower.data.id == this.data.id && otherTower.TowerSerial != this.TowerSerial)
            {
                Destroy(other);
                Destroy(gameObject);

                GameObject merged = Instantiate(gm.TowerPrefab, tile.transform.position, Quaternion.identity);
                tile.towerOnTile = merged;
            }
            else
            {
                // 자리 교환
                Vector3 tempPos = other.transform.position;
                other.transform.position = currentTile.transform.position;
                transform.position = tempPos;

                currentTile.towerOnTile = other;
                tile.towerOnTile = gameObject;
                currentTile = tile;
            }
        }
    }
    #endregion
}
