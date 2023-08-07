using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySectionUI : MonoBehaviour
{
    public RectTransform sectionPanelWeapon;
    public RectTransform sectionPanelSkill;
    public RectTransform sectionPanelUlt;
    public RectTransform sectionPanelConsumable;
    public RectTransform sectionPanelQuest;

    public List<RectTransform> Sections = new List<RectTransform>();

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        //sectionPanelWeapon.gameObject.SetActive(false);
        //sectionPanelSkill.gameObject.SetActive(false);
        //sectionPanelUlt.gameObject.SetActive(false);
        //sectionPanelConsumable.gameObject.SetActive(false);

        Sections.Add(sectionPanelWeapon);
        Sections.Add(sectionPanelSkill);
        Sections.Add(sectionPanelUlt);
        Sections.Add(sectionPanelQuest);

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void EnableSectionWeapon()
    {
        for (int i = 0; i < Sections.Count; i++)
        {
            Sections[i].gameObject.SetActive(false);
            Debug.Log("Panels");
        }
    }


}
