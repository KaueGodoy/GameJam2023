using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerSkillController : MonoBehaviour
{
    public GameObject playerHandSkill;
    public CharacterPanel characterPanel;

    public GameObject EquippedSkill { get; set; }

    Transform spawnProjectile;
    CharacterStats characterStats;
    Item currentlyEquippedItem;
    //IWeapon weaponEquipped;
    ISkill skillEquipped;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        spawnProjectile = transform.Find("SkillSpawn");
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

    public void EquipSkill(Item itemToEquip)
    {
        if (EquippedSkill != null)
        {
            UnequipSkill();
        }

        EquippedSkill = Instantiate(Resources.Load<GameObject>("Skills/" + itemToEquip.ObjectSlug),
       playerHandSkill.transform.position, playerHandSkill.transform.rotation);

        skillEquipped = EquippedSkill.GetComponent<ISkill>();

        if (EquippedSkill.GetComponent<IProjectileSkill>() != null)
        {
            EquippedSkill.GetComponent<IProjectileSkill>().ProjectileSpawn = spawnProjectile;
        }

        EquippedSkill.transform.SetParent(playerHandSkill.transform);

        skillEquipped.Stats = itemToEquip.Stats;
        currentlyEquippedItem = itemToEquip;

        characterStats.AddStatBonus(itemToEquip.Stats);

        UIEventHandler.SkillEquipped(itemToEquip);
        UIEventHandler.StatsChanged();

        Debug.Log("Skill equipped");
        Debug.Log(skillEquipped.Stats[0].GetCalculatedStatValue());

    }

    public void UnequipSkill()
    {
        if (EquippedSkill != null)
        {
            InventoryController.Instance.GiveItem(currentlyEquippedItem.ObjectSlug);
            characterStats.RemoveStatBonus(skillEquipped.Stats);
            characterPanel.UnequipSkill();
            UIEventHandler.StatsChanged();
            Destroy(EquippedSkill.transform.gameObject);
        }
    }

    private void Update()
    {
        if (playerControls.Player.Skill.triggered)
        {
            if (EquippedSkill != null)
            {
                PerformSkillAttack();
            }
        }
    }

    public void PerformSkillAttack()
    {
        skillEquipped.PerformAttack(CalculateDamage());
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
        skillEquipped.PerformSkillAttack();
    }

    public void PerformWeaponUltAttack()
    {
        skillEquipped.PerformUltAttack();
    }

}
