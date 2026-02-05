using UnityEngine;

public class OnOffBridge : MonoBehaviour
{
    public Animator animator;
    public Collider2D bridgeCollider;
    public bool activeWhenOn = true;

    private void OnEnable()
    {
        StartCoroutine(InitializeBridge());
    }

private System.Collections.IEnumerator InitializeBridge()
    {
    yield return null; // apparently we gotta wait a sec before it checks all the bridges whether they're on or not
        if (OnOffGlobalManager.Instance != null)
        {
            OnOffGlobalManager.Instance.OnStateChange += UpdateBridgeState;

            UpdateBridgeState(OnOffGlobalManager.Instance.globalState);
        }
    }

    private void OnDisable()
    {
        if (OnOffGlobalManager.Instance != null)
            OnOffGlobalManager.Instance.OnStateChange -= UpdateBridgeState;
    }

    // If ON, then is SOLID AND ON! WOOOO!
    public void UpdateBridgeState(bool globalIsOn)
    {
        bool shouldBeSolid = (globalIsOn == activeWhenOn);

        if (bridgeCollider != null)
            bridgeCollider.enabled = shouldBeSolid;

        if (animator != null)
            animator.SetBool("IsOn", globalIsOn);
    }
}
