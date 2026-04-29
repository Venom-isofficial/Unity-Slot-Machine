using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Reel reel1, reel2, reel3;
    public WinManager winManager;
    public HandleController handle;

    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI winText;
    public GameObject winPopup;

    public CameraShake cameraShake;

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

        StartCoroutine(cameraShake.Shake(0.2f, 0.1f));

        AudioManager.instance.PlaySpin();

        //  Spin reels with stagger
        yield return StartCoroutine(reel1.Spin());
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(reel2.Spin());
        yield return new WaitForSeconds(0.15f);

        //  Check anticipation
        bool anticipation = reel1.GetSymbolType() == reel2.GetSymbolType();

        yield return StartCoroutine(reel3.Spin(true, anticipation));

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

            StartCoroutine(WinEffect(payout));

            AudioManager.instance.PlayWin();
            StartCoroutine(cameraShake.Shake(0.3f, 0.2f));
        }
        else if (winManager.IsNearMiss(r1, r2, r3))
        {
            //  Near miss effect
            StartCoroutine(cameraShake.Shake(0.15f, 0.05f));
        }

        UpdateUI();
    }

    IEnumerator WinEffect(int payout)
    {
        winPopup.SetActive(true);

        int display = 0;

        while (display < payout)
        {
            display += Mathf.CeilToInt(payout * Time.deltaTime * 5);
            display = Mathf.Min(display, payout);

            winText.text = "YOU WIN: " + display;
            yield return null;
        }

        // 🎯 Bounce effect
        StartCoroutine(Bounce(winPopup.transform));
    }

    IEnumerator Bounce(Transform obj)
    {
        Vector3 original = obj.localScale;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            float scale = 1 + Mathf.Sin(t * Mathf.PI) * 0.2f;
            obj.localScale = original * scale;
            yield return null;
        }

        obj.localScale = original;
    }

    void UpdateUI()
    {
        balanceText.text = "Balance: " + balance;
    }
}