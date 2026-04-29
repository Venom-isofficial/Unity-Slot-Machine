using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Reel reel1, reel2, reel3;
    [SerializeField] private WinManager winManager;
    [SerializeField] private HandleController handle;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private CameraShake cameraShake;

    private int balance = 1000;
    private int currentBet = 10;
    private bool spinning = false;

    private void Start() => UpdateUI();

    public void SetBet(int bet) => currentBet = bet;

    public void Spin()
    {
        if (spinning || balance < currentBet) return;
        StartCoroutine(ExecuteSpin(null, null, null));
    }

    // Used in debug menu...
    public void SimulateWin(SymbolType s1, SymbolType s2, SymbolType s3)
    {
        if (spinning || balance < currentBet) return;
        StartCoroutine(ExecuteSpin(s1, s2, s3));
    }

    private IEnumerator ExecuteSpin(SymbolType? symbol1, SymbolType? symbol2, SymbolType? symbol3)
    {
        spinning = true;
        winPopup.SetActive(false);
        balance -= currentBet;
        UpdateUI();

        // Pull the lever and play spin sound
        yield return StartCoroutine(handle.PullHandle());
        StartCoroutine(cameraShake.Shake(0.2f, 0.1f));
        AudioManager.instance.PlaySpin();

        // Spin reels with timee - gives player time to observe
        yield return StartCoroutine(symbol1 == null ? reel1.Spin() : reel1.SpinToSymbol(symbol1.Value));
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(symbol2 == null ? reel2.Spin() : reel2.SpinToSymbol(symbol2.Value));
        yield return new WaitForSeconds(0.15f);

        // If first two match, speed up the third reel (casino type logicc)
        SymbolType reel1Type = reel1.GetSymbolType();
        SymbolType reel2Type = reel2.GetSymbolType();
        bool anticipation = reel1Type == reel2Type;

        yield return StartCoroutine(symbol3 == null
            ? reel3.Spin(true, anticipation)
            : reel3.SpinToSymbol(symbol3.Value, true, anticipation));

        AudioManager.instance.StopSpin();
        CheckWin();
        spinning = false;
    }

    private void CheckWin()
    {
        SymbolType r1 = reel1.GetSymbolType();
        SymbolType r2 = reel2.GetSymbolType();
        SymbolType r3 = reel3.GetSymbolType();
        int payout = winManager.CalculateWin(r1, r2, r3, currentBet);

        if (payout > 0)
        {
            balance += payout;
            StartCoroutine(WinEffect(payout));
            winManager.PlayWinFX(payout, currentBet, reel1.GetCenterSlot(), reel2.GetCenterSlot(), reel3.GetCenterSlot());
            AudioManager.instance.PlayWin();
        }
        else if (winManager.IsNearMiss(r1, r2, r3))
        {
            StartCoroutine(cameraShake.Shake(0.15f, 0.05f));
        }

        UpdateUI();
    }

    private IEnumerator WinEffect(int payout)
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

        RectTransform popup = winPopup.GetComponent<RectTransform>();
        yield return StartCoroutine(AnimateBounce(popup));
    }

    private IEnumerator AnimateBounce(Transform obj)
    {
        Vector3 originalScale = obj.localScale;
        float duration = 0.4f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = 1 + Mathf.Sin(t * Mathf.PI) * 0.2f;
            obj.localScale = originalScale * scale;
            yield return null;
        }

        obj.localScale = originalScale;
    }

    private void UpdateUI() => balanceText.text = "Balance: " + balance;

    // For-- Debug 
    public int GetCurrentBet() => currentBet;
    public void AddBalance(int amount) { balance += amount; UpdateUI(); }
}