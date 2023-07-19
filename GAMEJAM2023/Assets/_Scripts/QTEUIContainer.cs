using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEUIContainer : MonoBehaviour
{
    public Image QTEPanel;
    public TextMeshProUGUI QTEText;

    public float requiredPressAmount;

    QTELogic QTELogic;

    private void Start()
    {
        transform.localScale = Vector3.one;
        QTELogic = GetComponent<QTELogic>();
    }

    public void SetRequiredPressAmount(float amount)
    {
        requiredPressAmount = amount;
    }

    public void Update()
    {
        QTEPanel.fillAmount = QTELogic.target;
    }
}
