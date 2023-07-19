using UnityEngine;

public class QTEUI : MonoBehaviour
{
    QTEUIContainer container;
    public RectTransform content;

    private void Awake()
    {
        container = Resources.Load<QTEUIContainer>("UI/QTE_Container");
    }

    public void InstantiateQTEUI(float requiredPressAmount)
    {
        // Check if an instance of the UI element already exists
        if (content.childCount > 0)
        {
            Debug.Log("UI element already instantiated.");
            return; // Exit the method if an instance already exists
        }

        QTEUIContainer newContainer = Instantiate(container);
        newContainer.transform.SetParent(content);

        // Set the required press amount on the UI element
        newContainer.requiredPressAmount = requiredPressAmount;

        // Attach the QTELogic script to the instantiated UI element
        QTELogic qteLogic = newContainer.gameObject.GetComponent<QTELogic>();
        qteLogic.Initialize();
    }
}
