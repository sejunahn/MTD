public interface IEnemy
{
    string Id { get; }
    string Name { get; }
    int Hp { get; }
    float MoveSpeed { get; }
    int Reward { get; }
    string Trait { get; }

    void TakeDamage(int damage);
}