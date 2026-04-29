using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    public GameObject debugPanel;
    public GameManager gameManager;

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
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard[Key.D].wasPressedThisFrame && keyboard[Key.LeftCtrl].isPressed)
        {
            ToggleDebugMenu();
        }
    }

    void ToggleDebugMenu()
    {
        debugActive = !debugActive;
        Debug.Log($"[DEBUG] Menu toggled: {debugActive}");
        if (debugPanel != null)
        {
            debugPanel.SetActive(debugActive);
        }
        else
        {
            Debug.LogError("[DEBUG] DebugPanel not assigned in inspector!");
        }
    }

    // 🎰 JACKPOT - 3x Wild
    public void SimulateJackpot()
    {
        gameManager.SimulateWin(SymbolType.Wild, SymbolType.Wild, SymbolType.Wild);
    }

    // 👑 MEGA WIN - 3x Crown
    public void SimulateMegaWin()
    {
        gameManager.SimulateWin(SymbolType.Crown, SymbolType.Crown, SymbolType.Crown);
    }

    // 💰 BIG WIN - 3x Coin
    public void SimulateBigWin()
    {
        gameManager.SimulateWin(SymbolType.Coin, SymbolType.Coin, SymbolType.Coin);
    }

    // 😬 NEAR MISS - 2-of-3 match
    public void SimulateNearMiss()
    {
        gameManager.SimulateWin(SymbolType.Seven, SymbolType.Seven, SymbolType.Apple);
    }

    // 💸 SET BET
    public void SetBetAmount(int amount)
    {
        gameManager.SetBet(amount);
        Debug.Log($"[DEBUG] Bet set to: {amount}");
    }

    // ➕ Increase bet
    public void IncreaseBet()
    {
        SetBetAmount(gameManager.GetCurrentBet() + 10);
    }

    // ➖ Decrease bet
    public void DecreaseBet()
    {
        int newBet = Mathf.Max(1, gameManager.GetCurrentBet() - 10);
        SetBetAmount(newBet);
    }

    // 💳 Add balance
    public void AddBalance(int amount)
    {
        gameManager.AddBalance(amount);
        Debug.Log($"[DEBUG] Added {amount} to balance");
    }
}
