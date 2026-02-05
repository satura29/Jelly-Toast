using UnityEngine;
using System;

public class OnOffGlobalManager : MonoBehaviour
{
    public static OnOffGlobalManager Instance;

    public bool globalState = false;
    public event Action<bool> OnStateChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void ToggleState()
    {
        globalState = !globalState;
        OnStateChange?.Invoke(globalState);
        Debug.Log("Global state toggled: " + globalState);
    }

    public void SetState(bool state)
    {
        globalState = state;
        OnStateChange?.Invoke(globalState);
        Debug.Log("Global state set to: " + globalState);
    }
}
