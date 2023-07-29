using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Bite : MonoBehaviour, IEnemyBehavior
{
    public float chargeTime = 1.5f; // The time it takes for the enemy to charge the attack
    public float speedForce = 10f; // The speed at which the enemy charges towards the player

    public float cooldownDefault = 0.5f; // The minimum cooldown duration
    public float cooldownMin = 0.5f; // The minimum cooldown duration
    public float cooldownMax = 1.5f; // The maximum cooldown duration
    public float stepBackCooldown = 1f; // The duration of the step back cooldown

    public bool isCooldown = false;
    public bool isCharging = false;
    public bool hasCharged = false;
    public bool playerHit = false;

    private Vector2 force;

    private Transform _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    public QTEUI QTEUI;
    public float QTEAmount = 10;

    public Item item;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void UpdateBehavior()
    {
        if (!isCooldown && !isCharging && !hasCharged)
        {
            StartCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        _spriteRenderer.color = Color.red;

        isCooldown = true;

        isCharging = true;

        Debug.Log("Charging");

        yield return new WaitForSeconds(chargeTime);

        isCharging = false;

        _spriteRenderer.color = Color.white;

        Vector2 direction = (_player.position - transform.position).normalized;

        force = new Vector2(direction.x * speedForce, _rb.velocity.y);

        _rb.AddForce(force, ForceMode2D.Impulse);

        Debug.Log("Has charged");

        hasCharged = true;

        yield return new WaitForSeconds(cooldownDefault);

        if (playerHit)
        {

        }


        float cooldown = Random.Range(cooldownMin, cooldownMax);

        yield return new WaitForSeconds(cooldown);

        if (_rb != null)
            _rb.velocity = Vector2.zero;

        hasCharged = false;
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            playerHit = true;

            QTEUI.InstantiateQTEUI(QTEAmount);
            //InventoryController.Instance.ConsumeItem("potion_hp");

            //Player player = collision.gameObject.GetComponent<Player>();
            //float oldMoveSpeed = 6;

            ////oldMoveSpeed = player.baseMoveSpeed;

            //CharacterStats stats = collision.gameObject.GetComponent<Player>().characterStats;

            //float moveSpeedDebuff = -1 * (stats.GetStat(BaseStat.BaseStatType.MoveSpeedBonus).GetCalculatedStatValue());

            //player.baseMoveSpeed *= (moveSpeedDebuff / 100);

            //if (!QTELogic.stuck)
            //{
            //    player.baseMoveSpeed = oldMoveSpeed;
            //}

            //InventoryController.Instance.ConsumeItem(item);
            //collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerHit = false;
    }

    public void Disable()
    {

    }

    private void OnDrawGizmosSelected()
    {
        // Draw the charging distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, force);
    }
}
