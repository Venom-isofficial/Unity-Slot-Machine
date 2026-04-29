using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SymbolType { Apple, Bar, Bonus, Cherry, Coin, Crown, Seven, Watermelon, Wild }
public enum WinTier { Normal, Big, Mega }

public class WinManager : MonoBehaviour
{
    [Header("FX Sprites & References")]
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Transform coinCanvas;
    [SerializeField] private CameraShake cameraShake;

    [Header("Glow Animation Settings")]
    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float glowIntensity = 2f;

    // Payout multipliers: Apple, Bar, Bonus, Cherry, Coin, Crown, Seven, Watermelon, Wild
    private readonly int[] SYMBOL_PAYOUTS = { 6, 10, 30, 5, 15, 20, 25, 8, 12 };

    public int CalculateWin(SymbolType a, SymbolType b, SymbolType c, int bet)
    {
        // 3x Bonus = 30x, 2x Bonus = 5x
        int bonusCount = CountSymbols(SymbolType.Bonus, a, b, c);
        if (bonusCount == 3) return bet * 30;
        if (bonusCount == 2) return bet * 5;

        // 3x Seven = 25x (jackpot)
        if (a == SymbolType.Seven && b == SymbolType.Seven && c == SymbolType.Seven)
            return bet * 25;

        // Check if match is valid (wildcards will bee considered)
        if (!IsValidMatch(a, b, c))
            return 0;

        SymbolType main = GetMainSymbol(a, b, c);
        return bet * SYMBOL_PAYOUTS[(int)main];
    }

    public void PlayWinFX(int payout, int bet, Image s1, Image s2, Image s3)
    {
        WinTier tier = GetWinTier(payout, bet);
        ApplySymbolEffects(s1, s2, s3);
        StartCoroutine(CoinRain(tier == WinTier.Mega ? 40 : 20));

        if (tier == WinTier.Mega && cameraShake != null)
            StartCoroutine(cameraShake.Shake(0.6f, 0.3f));
    }

    private void ApplySymbolEffects(Image s1, Image s2, Image s3)
    {
        foreach (Image symbol in new[] { s1, s2, s3 })
        {
            StartCoroutine(GlowSymbol(symbol));
            StartCoroutine(BounceSymbol(symbol));
        }
    }

    private WinTier GetWinTier(int payout, int bet)
    {
        if (payout >= bet * 20) return WinTier.Mega;
        if (payout >= bet * 10) return WinTier.Big;
        return WinTier.Normal;
    }

    // Glow winningsymbols
    private IEnumerator GlowSymbol(Image img)
    {
        Color original = img.color;
        float elapsed = 0;

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime * 4;
            img.color = Color.Lerp(original, glowColor * glowIntensity, Mathf.PingPong(elapsed, 1));
            yield return null;
        }

        img.color = original;
    }

    // Bounce for 3 bounces using sine wave
    private IEnumerator BounceSymbol(Image symbolImage)
    {
        RectTransform rt = symbolImage.GetComponent<RectTransform>();
        Vector3 originalScale = rt.localScale;
        float duration = 0.6f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float bounceHeight = Mathf.Sin(t * Mathf.PI * 3) * (1 - t);
            rt.localScale = originalScale * (1 + bounceHeight * 0.25f);
            yield return null;
        }

        rt.localScale = originalScale;
    }

    // Coin rain - spawn coin And drop it as falling from top
    private IEnumerator CoinRain(int count)
    {
        if (coinSprite == null || coinCanvas == null)
        {
            Debug.LogWarning("Coin rain skipped: missing sprite or canvas");
            yield break;
        }

        for (int i = 0; i < count; i++)
        {
            SpawnFallingCoin();
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void SpawnFallingCoin()
    {
        GameObject coinObj = new GameObject("Coin");
        coinObj.transform.SetParent(coinCanvas, false);

        Image coinImage = coinObj.AddComponent<Image>();
        coinImage.sprite = coinSprite;
        coinImage.SetNativeSize();

        RectTransform rt = coinObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        rt.anchoredPosition = new Vector2(Random.Range(-300, 300), 500);

        StartCoroutine(AnimateCoinFall(rt));
        Destroy(coinObj, 2.5f);
    }

    private IEnumerator AnimateCoinFall(RectTransform coin)
    {
        Vector2 startPos = coin.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(Random.Range(-100, 100), -600);
        Image coinImage = coin.GetComponent<Image>();
        float duration = 2f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            coin.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            if (t > 0.8f)
            {
                Color color = coinImage.color;
                color.a = Mathf.Lerp(1f, 0f, (t - 0.8f) / 0.2f);
                coinImage.color = color;
            }

            yield return null;
        }
    }

    // Helper: Check if Matched is valid (wildcards }
    private bool IsValidMatch(SymbolType a, SymbolType b, SymbolType c)
    {
        SymbolType baseSymbol = SymbolType.Wild;
        if (a != SymbolType.Wild) baseSymbol = a;
        else if (b != SymbolType.Wild) baseSymbol = b;
        else if (c != SymbolType.Wild) baseSymbol = c;

        return (a == baseSymbol || a == SymbolType.Wild) &&
               (b == baseSymbol || b == SymbolType.Wild) &&
               (c == baseSymbol || c == SymbolType.Wild);
    }

    private SymbolType GetMainSymbol(SymbolType a, SymbolType b, SymbolType c)
    {
        if (a != SymbolType.Wild) return a;
        if (b != SymbolType.Wild) return b;
        return c;
    }

    private int CountSymbols(SymbolType t, SymbolType a, SymbolType b, SymbolType c)
    {
        int count = 0;
        if (a == t) count++;
        if (b == t) count++;
        if (c == t) count++;
        return count;
    }

    // Near-miss: 2 of 3 symbols match
    public bool IsNearMiss(SymbolType a, SymbolType b, SymbolType c)
    {
        if (a == b && a != c) return true;
        if (b == c && b != a) return true;
        if (a == c && a != b) return true;
        return false;
    }
}