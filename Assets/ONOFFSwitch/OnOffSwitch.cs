using UnityEngine;

public class OnOffSwitch : MonoBehaviour
{
    public Animator animator;
    private bool canToggle = true;

    private void OnEnable()
    {
        if (OnOffGlobalManager.Instance != null)
            OnOffGlobalManager.Instance.OnStateChange += UpdateSwitchAnimation;

        UpdateSwitchAnimation(OnOffGlobalManager.Instance.globalState);
    }

    private void OnDisable()
    {
        if (OnOffGlobalManager.Instance != null)
            OnOffGlobalManager.Instance.OnStateChange -= UpdateSwitchAnimation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canToggle) return;

        if (collision.CompareTag("Player") || collision.CompareTag("Toast"))
        {
            ToggleSwitch();
        }
    }

    private void ToggleSwitch()
    {
        canToggle = false;

        if (animator != null)
            animator.SetTrigger("Switch");

        OnOffGlobalManager.Instance.ToggleState();

        Invoke(nameof(ResetToggle), 0.3f);
    }

    private void UpdateSwitchAnimation(bool isOn)
    {
        if (animator != null)
            animator.SetBool("StateOn", isOn);
    }

    private void ResetToggle()
    {
        canToggle = true;
    }
}
