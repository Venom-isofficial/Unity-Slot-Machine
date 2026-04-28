using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Reel reel1, reel2, reel3;

    public int balance = 1000;
    public int bet = 10;

    public GameObject winPopup;
    public AudioManager audioManager;

    public void Spin()
    {
        if (balance < bet)
        {
            Debug.Log("Not enough balance!");
            return;
        }

        balance -= bet;
        audioManager.PlaySpin();

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        yield return StartCoroutine(reel1.Spin(1.5f));
        yield return StartCoroutine(reel2.Spin(2f));
        yield return StartCoroutine(reel3.Spin(2.5f));

        CheckWin();
    }

    void CheckWin()
    {
        var a = reel1.GetFinalSymbol();
        var b = reel2.GetFinalSymbol();
        var c = reel3.GetFinalSymbol();

        if (a == b && b == c)
        {
            int winAmount = bet * 5;
            balance += winAmount;

            Debug.Log("WIN +" + winAmount);
            audioManager.PlayWin();

            winPopup.SetActive(true);
        }
        else
        {
            Debug.Log("LOSE");
        }

        Debug.Log("Balance: " + balance);
    }

    public void SetBet(int value)
    {
        bet = value;
        Debug.Log("Bet: " + bet);
    }

    public void ClosePopup()
    {
        winPopup.SetActive(false);
    }
}