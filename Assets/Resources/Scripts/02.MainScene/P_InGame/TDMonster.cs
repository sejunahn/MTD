using UnityEngine;

public class TDMonster : MonoBehaviour
{
    public int hp = 50;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
