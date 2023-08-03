using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerUltController : MonoBehaviour
{
    public GameObject playerHandUlt;
    public CharacterPanel characterPanel;
    public GameObject EquippedUlt { get; set; }

    Transform spawnProjectile;
    CharacterStats characterStats;
    Item currentlyEquippedItem;
    IUlt ultEquipped;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        spawnProjectile = transform.Find("UltSpawn");
        characterStats = GetComponent<Player>().characterStats;
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void EquipUlt(Item itemToEquip)
    {
        if (EquippedUlt != null)
        {
            UnequipUlt();
        }

        EquippedUlt = Instantiate(Resources.Load<GameObject>("Ults/" + itemToEquip.ObjectSlug),
       playerHandUlt.transform.position, playerHandUlt.transform.rotation);

        ultEquipped = EquippedUlt.GetComponent<IUlt>();

        if (EquippedUlt.GetComponent<IProjectileUlt>() != null)
        {
            EquippedUlt.GetComponent<IProjectileUlt>().ProjectileSpawn = spawnProjectile;
        }

        EquippedUlt.transform.SetParent(playerHandUlt.transform);

        ultEquipped.Stats = itemToEquip.Stats;
        currentlyEquippedItem = itemToEquip;

        characterStats.AddStatBonus(itemToEquip.Stats);

        UIEventHandler.UltEquipped(itemToEquip);
        UIEventHandler.StatsChanged();

        Debug.Log("Ult equipped");
        Debug.Log(ultEquipped.Stats[0].GetCalculatedStatValue());

    }

    public void UnequipUlt()
    {
        if (EquippedUlt != null)
        {
            InventoryController.Instance.GiveItem(currentlyEquippedItem.ObjectSlug);
            characterStats.RemoveStatBonus(ultEquipped.Stats);
            characterPanel.UnequipUlt();
            UIEventHandler.StatsChanged();
            Destroy(EquippedUlt.transform.gameObject);
        }
    }

    private void Update()
    {
        if (playerControls.Player.Ult.triggered)
        {
            if (EquippedUlt != null)
            {
                PerformUltAttack();
            }
        }
    }

    public void PerformUltAttack()
    {
        ultEquipped?.PerformAttack(CalculateDamage());
    }

    private float CalculateDamage()
    {
        float baseDamage = (characterStats.GetStat(BaseStat.BaseStatType.Attack).GetCalculatedStatValue())
                             * (1 + (characterStats.GetStat(BaseStat.BaseStatType.AttackBonus).GetCalculatedStatValue() / 100))
                             + (characterStats.GetStat(BaseStat.BaseStatType.FlatAttack).GetCalculatedStatValue());

        float damageToDeal = baseDamage * (1 + characterStats.GetStat(BaseStat.BaseStatType.DamageBonus).GetCalculatedStatValue() / 100);

        damageToDeal += CalculateCrit(damageToDeal);
        Debug.Log("Damage dealt: " + damageToDeal);
        return damageToDeal;
    }

    private float CalculateCrit(float damage)
    {
        if (Random.value <= (characterStats.GetStat(BaseStat.BaseStatType.CritRate).GetCalculatedStatValue() / 100))
        {
            float critDamage = (damage * ((characterStats.GetStat(BaseStat.BaseStatType.CritDamage).GetCalculatedStatValue()) / 100));
            return critDamage;
        }
        return 0;
    }

    public void PerformWeaponSkillAttack()
    {
        ultEquipped.PerformSkillAttack();
    }

    public void PerformWeaponUltAttack()
    {
        ultEquipped.PerformUltAttack();
    }

}
