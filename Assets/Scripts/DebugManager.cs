using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    [SerializeField] private GameObject debugPanel;
    [SerializeField] private GameManager gameManager;
    private bool debugActive = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (debugPanel != null)
            debugPanel.SetActive(false);
    }

    private void Update()
    {
        // Press Ctrl+D to open debug menu ...
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard[Key.D].wasPressedThisFrame && keyboard[Key.LeftCtrl].isPressed)
            ToggleDebugMenu();
    }

    private void ToggleDebugMenu()
    {
        debugActive = !debugActive;
        if (debugPanel != null)
        {
            debugPanel.SetActive(debugActive);
            Debug.Log($"Debug: {(debugActive ? "ON" : "OFF")}");
        }
    }

    // Win scenario test
    public void SimulateJackpot() => gameManager.SimulateWin(SymbolType.Seven, SymbolType.Seven, SymbolType.Seven);
    public void SimulateMegaWin() => gameManager.SimulateWin(SymbolType.Crown, SymbolType.Crown, SymbolType.Crown);
    public void SimulateBigWin() => gameManager.SimulateWin(SymbolType.Coin, SymbolType.Coin, SymbolType.Coin);
    public void SimulateNearMiss() => gameManager.SimulateWin(SymbolType.Seven, SymbolType.Seven, SymbolType.Apple);

}
