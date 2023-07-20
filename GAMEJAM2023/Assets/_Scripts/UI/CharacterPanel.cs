using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    //[Header("Health")]
    [SerializeField] private Player player;
    //[SerializeField] private TextMeshProUGUI health;
    //[SerializeField] private Image healthFill;

    //[Header("Level")]
    //[SerializeField] private TextMeshProUGUI level;
    //[SerializeField] private Image levelFill;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI playerStatPrefab;
    [SerializeField] private Transform playerStatPanel;
    private List<TextMeshProUGUI> playerStatTexts = new List<TextMeshProUGUI>();

    [Header("Weapon")]
    [SerializeField] private Sprite defaultWeaponSprite;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Transform weaponStatPanel;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI weaponStatPrefab;
    private PlayerWeaponController PlayerWeaponController;
    private List<TextMeshProUGUI> weaponStatTexts = new List<TextMeshProUGUI>();

    [Header("Skill")]
    [SerializeField] private Sprite defaultSkillSprite;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Transform skillStatPanel;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillStatPrefab;
    private PlayerSkillController PlayerSkillController;
    private List<TextMeshProUGUI> skillStatTexts = new List<TextMeshProUGUI>();

    [Header("Ult")]
    [SerializeField] private Sprite defaultUltSprite;
    [SerializeField] private Image ultIcon;
    [SerializeField] private Transform ultStatPanel;
    [SerializeField] private TextMeshProUGUI ultNameText;
    [SerializeField] private TextMeshProUGUI ultStatPrefab;
    private PlayerUltController PlayerUltController;
    private List<TextMeshProUGUI> ultStatTexts = new List<TextMeshProUGUI>();


    private void Awake()
    {
        PlayerWeaponController = player.GetComponent<PlayerWeaponController>();
        PlayerSkillController = player.GetComponent<PlayerSkillController>();
        PlayerUltController = player.GetComponent<PlayerUltController>();

        //UIEventHandler.OnPlayerHealthChanged += UpdateHealth;
        UIEventHandler.OnStatsChanged += UpdateStats;
        UIEventHandler.OnItemEquipped += UpdateEquippedWeapon;
        UIEventHandler.OnSkillEquipped += UpdateEquippedSkill;
        UIEventHandler.OnUltEquipped += UpdateEquippedUlt;
        InitializeStats();

    }

    //private void UpdateHealth(float currentHealth, float maxHealth)
    //{
    //    this.health.text = currentHealth.ToString();
    //    this.healthFill.fillAmount = currentHealth / maxHealth;
    //}

    private void InitializeStats()
    {
        Debug.Log("Stats init");
        for (int i = 0; i < player.characterStats.stats.Count; i++)
        {
            playerStatTexts.Add(Instantiate(playerStatPrefab));
            playerStatTexts[i].transform.SetParent(playerStatPanel);
        }

        UpdateStats();
    }

    private void UpdateStats()
    {
        for (int i = 0; i < player.characterStats.stats.Count; i++)
        {
            playerStatTexts[i].text = player.characterStats.stats[i].StatName + ": " +
                player.characterStats.stats[i].GetCalculatedStatValue().ToString();

            playerStatTexts[i].transform.localScale = Vector3.one;
        }
    }

    private void UpdateEquippedWeapon(Item item)
    {
        weaponIcon.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
        weaponNameText.text = item.ItemName;

        for (int i = 0; i < item.Stats.Count; i++)
        {
            weaponStatTexts.Add(Instantiate(weaponStatPrefab));
            weaponStatTexts[i].transform.SetParent(weaponStatPanel);
            weaponStatTexts[i].text = item.Stats[i].StatName + ": " + item.Stats[i].GetCalculatedStatValue().ToString();

            weaponStatTexts[i].transform.localScale = Vector3.one;
        }
    }

    private void UpdateEquippedSkill(Item item)
    {
        skillIcon.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
        skillNameText.text = item.ItemName;

        for (int i = 0; i < item.Stats.Count; i++)
        {
            skillStatTexts.Add(Instantiate(weaponStatPrefab));
            skillStatTexts[i].transform.SetParent(skillStatPanel);
            skillStatTexts[i].text = item.Stats[i].StatName + ": " + item.Stats[i].GetCalculatedStatValue().ToString();

            skillStatTexts[i].transform.localScale = Vector3.one;
        }
    }

    private void UpdateEquippedUlt(Item item)
    {
        ultIcon.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
        ultNameText.text = item.ItemName;

        for (int i = 0; i < item.Stats.Count; i++)
        {
            ultStatTexts.Add(Instantiate(weaponStatPrefab));
            ultStatTexts[i].transform.SetParent(ultStatPanel);
            ultStatTexts[i].text = item.Stats[i].StatName + ": " + item.Stats[i].GetCalculatedStatValue().ToString();

            ultStatTexts[i].transform.localScale = Vector3.one;
        }
    }

    public void UnequipWeapon()
    {
        if (PlayerWeaponController.EquippedWeapon != null)
        {
            weaponNameText.text = " ";
            weaponIcon.sprite = defaultWeaponSprite;

            for (int i = 0; i < weaponStatTexts.Count; i++)
            {
                Destroy(weaponStatTexts[i].gameObject);
            }
            weaponStatTexts.Clear();
            PlayerWeaponController.UnequipWeapon();
        }
    }

    public void UnequipSkill()
    {
        if (PlayerSkillController.EquippedSkill != null)
        {
            skillNameText.text = " ";
            skillIcon.sprite = defaultWeaponSprite;

            for (int i = 0; i < skillStatTexts.Count; i++)
            {
                Destroy(skillStatTexts[i].gameObject);
            }
            skillStatTexts.Clear();
            PlayerSkillController.UnequipSkill();
        }
    }

    public void UnequipUlt()
    {
        if (PlayerUltController.EquippedUlt != null)
        {
            ultNameText.text = " ";
            ultIcon.sprite = defaultWeaponSprite;

            for (int i = 0; i < ultStatTexts.Count; i++)
            {
                Destroy(ultStatTexts[i].gameObject);
            }
            ultStatTexts.Clear();
            PlayerUltController.UnequipUlt();
        }
    }

}
