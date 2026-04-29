using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Reel reel1;
    public Reel reel2;
    public Reel reel3;

    public WinManager winManager;

    public HandleController handle;

    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI winText;
    public GameObject winPopup;

    private int balance = 1000;
    private int currentBet = 10;

    private bool spinning = false;

    void Start()
    {
        UpdateUI();
    }

    public void SetBet(int bet)
    {
        currentBet = bet;
        Debug.Log("Bet set: " + bet);
    }

    public void Spin()
    {
        if (spinning) return;
        if (balance < currentBet) return;

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        spinning = true;
        winPopup.SetActive(false);

        balance -= currentBet;
        UpdateUI();

        yield return StartCoroutine(handle.PullHandle());

        AudioManager.instance.PlaySpin();

        yield return StartCoroutine(reel1.Spin());
        yield return StartCoroutine(reel2.Spin());
        yield return StartCoroutine(reel3.Spin());

        AudioManager.instance.StopSpin();

        CheckWin();

        spinning = false;
    }

    void CheckWin()
    {
        SymbolType r1 = reel1.GetSymbolType();
        SymbolType r2 = reel2.GetSymbolType();
        SymbolType r3 = reel3.GetSymbolType();

        int payout = winManager.CalculateWin(r1, r2, r3, currentBet);

        if (payout > 0)
        {
            balance += payout;
            winPopup.SetActive(true);
            winText.text = "YOU WIN: " + payout;

            AudioManager.instance.PlayWin();

            // ✨ future hook: glow symbols
            Debug.Log("WIN EFFECT");
        }
        else if (winManager.IsNearMiss(r1, r2, r3))
        {
            Debug.Log("NEAR MISS 🎯");

            // 🔥 optional polish:
            // play tease sound
            // slight shake
        }

        UpdateUI();
    }



    void UpdateUI()
    {
        balanceText.text = "Balance: " + balance;
    }
}