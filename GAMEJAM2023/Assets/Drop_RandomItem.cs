using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_RandomItem : MonoBehaviour, IEnemy
{
    [Header("Drop")]
    public PickupItem pickupItem;
    public DropTable DropTable { get; set; }

    public int ID { get; set; }
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public bool IsDead { get; set; }

    private float _currentHealth;
    private float _maxHealth = 1f;

    private void Start()
    {
        _currentHealth = _maxHealth;

        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("coin", 100),
        };
    }

    void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }

    public void PerformAttack()
    {

    }

    public void Die()
    {

    }
}
