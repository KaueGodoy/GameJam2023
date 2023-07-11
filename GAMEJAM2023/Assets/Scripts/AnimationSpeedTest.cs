using UnityEngine;

public class AnimationSpeedTest : MonoBehaviour
{
    private string animationName = "animationTest"; // Name of the animation you want to modify
    public float animationSpeedMultiplier = 1f; // Adjust this value to change the speed

    private Animator animator;
    private int animationHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animationHash = Animator.StringToHash(animationName); // Make sure animationName is assigned correctly
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change the speed of the animation
            animator.SetFloat(animationHash, animationSpeedMultiplier);
        }
    }
}
