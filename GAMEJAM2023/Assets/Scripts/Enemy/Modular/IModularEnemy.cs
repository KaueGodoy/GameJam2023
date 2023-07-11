public interface IModularEnemy
{
    public float MoveSpeed { get; set; }
    public float PatrolDistance { get; set; }
    public float ChaseDistance { get; set; }
    public float AttackDistance { get; set; }
    public float AttackDamage { get; set; }
    public float AttackCooldown { get; set; }
    void ChangeBehavior(IEnemyBehavior newBehavior);
}
