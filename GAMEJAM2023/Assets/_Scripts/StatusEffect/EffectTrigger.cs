using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    [SerializeField] BaseEffect Effect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // apply effect if supported
        var effectableObject = collision.GetComponent<EffectableObject>();

        if (effectableObject != null)
        {
            effectableObject.ApplyEffect(Effect);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // apply effect if supported
        var effectableObject = collision.gameObject.GetComponent<EffectableObject>();

        if (effectableObject != null)
        {
            effectableObject.ApplyEffect(Effect);
        }
    }
}
