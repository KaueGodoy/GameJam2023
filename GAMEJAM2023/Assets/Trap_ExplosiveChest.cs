using System.Collections.Generic;
using UnityEngine;

public class Trap_ExplosiveChest : ActionItem
{
    private Player _player;

    [Header("Drop")]
    public PickupItem pickupItem;
    public DropTable DropTable { get; set; }

    [Header("Explosion")]
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _explosionDamage = 50f;
    [SerializeField] private float _explosionRadius = 5f;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("coin", 100),
        };
    }

    private void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }

    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacting with chest item");
    }

    public override void InteractWithItem()
    {
        if (_playerInput.Player.Interaction.triggered)
        {
            Explode();
            DropLoot();
            Destroy();
            Debug.Log("Chest item");
        }
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PerformAttack();
                Debug.Log("Player damaged by explosion");
            }
        }

        GameObject pfExplosionEffect = Instantiate(_explosionEffect, transform.position, transform.rotation);
        Destroy(pfExplosionEffect, 0.2f);

        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        _player.TakeDamage(_explosionDamage);
        DamagePopup.Create(_player.transform.position + Vector3.right + Vector3.up, (int)_explosionDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
