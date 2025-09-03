using System;
using Unity.VisualScripting;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    private Ray ray;
    public void Update()
    {
        
    }

    public void OnMouseDown()
    {
        GetTileController();
    }

    public TileController GetTileController()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            Debug.Log("Hit at: " + hit.point + " | Tile pos: " + hit.collider.transform.position);
            return hit.collider.GetComponent<TileController>();
        }
        else return null;
    }
}
