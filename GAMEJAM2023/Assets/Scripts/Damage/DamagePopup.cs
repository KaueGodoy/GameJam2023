using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    
    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.damagePopupPrefab, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }
        
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
      
    }

    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
    }


}
