using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SymbolType
{
    Apple,
    Bar,
    Bonus,
    Cherry,
    Coin,
    Crown,
    Seven,
    Watermelon,
    Wild
}

public enum WinTier
{
    Normal,
    Big,
    Mega
}

public class WinManager : MonoBehaviour
{
    [Header("FX")]
    public Sprite coinSprite;
    public Transform coinCanvas;
    public CameraShake cameraShake;

    [Header("Glow")]
    public Color glowColor = Color.yellow;
    public float glowIntensity = 2f;

    // 🎯 MAIN WIN CALCULATION
    public int CalculateWin(SymbolType a, SymbolType b, SymbolType c, int bet)
    {
        int bonusCount = Count(SymbolType.Bonus, a, b, c);
        if (bonusCount == 3) return bet * 30;
        if (bonusCount == 2) return bet * 5;

        if (a == SymbolType.Wild && b == SymbolType.Wild && c == SymbolType.Wild)
            return bet * 25;

        if (!IsValidMatch(a, b, c))
            return 0;

        SymbolType main = GetMainSymbol(a, b, c);

        switch (main)
        {
            case SymbolType.Crown: return bet * 20;
            case SymbolType.Coin: return bet * 15;
            case SymbolType.Seven: return bet * 12;
            case SymbolType.Bar: return bet * 10;
            case SymbolType.Watermelon: return bet * 8;
            case SymbolType.Apple: return bet * 6;
            case SymbolType.Cherry: return bet * 5;
        }

        return 0;
    }

    // 🎯 MAIN FX ENTRY
    public void PlayWinFX(int payout, int bet, Image s1, Image s2, Image s3)
    {
        WinTier tier = GetTier(payout, bet);

        // ✨ Glow
        StartCoroutine(GlowSymbol(s1));
        StartCoroutine(GlowSymbol(s2));
        StartCoroutine(GlowSymbol(s3));

        // 💰 Coin rain
        if (tier == WinTier.Big)
        {
            StartCoroutine(CoinRain(20));
        }
        else if (tier == WinTier.Mega)
        {
            StartCoroutine(CoinRain(40));
            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.6f, 0.3f));
            }
            else
            {
                Debug.LogWarning("Camera shake not assigned to WinManager");
            }
        }
    }

    // 🎯 WIN TIERS
    WinTier GetTier(int payout, int bet)
    {
        if (payout >= bet * 20) return WinTier.Mega;
        if (payout >= bet * 10) return WinTier.Big;
        return WinTier.Normal;
    }

    // ✨ GLOW EFFECT
    IEnumerator GlowSymbol(Image img)
    {
        Color original = img.color;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            img.color = Color.Lerp(original, glowColor * glowIntensity, Mathf.PingPong(t, 1));
            yield return null;
        }

        img.color = original;
    }

    // 💰 COIN RAIN
    IEnumerator CoinRain(int count)
    {
        if (coinSprite == null || coinCanvas == null)
        {
            Debug.LogWarning("Coin rain FX skipped: coinSprite or coinCanvas not assigned");
            yield break;
        }

        for (int i = 0; i < count; i++)
        {
            // Create a new GameObject with Image component
            GameObject coinObj = new GameObject("FallingCoin");
            coinObj.transform.SetParent(coinCanvas, false);

            // Add Image component
            Image coinImage = coinObj.AddComponent<Image>();
            coinImage.sprite = coinSprite;
            coinImage.SetNativeSize();

            // Scale down the coin
            RectTransform rt = coinObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            rt.anchoredPosition = new Vector2(Random.Range(-300, 300), 500);

            // Animate falling
            StartCoroutine(FallCoin(rt));

            // Destroy after animation
            Destroy(coinObj, 2.5f);

            yield return new WaitForSeconds(0.03f);
        }
    }

    // 💰 COIN FALLING ANIMATION
    IEnumerator FallCoin(RectTransform coin)
    {
        float duration = 2f;
        float elapsed = 0;
        Vector2 startPos = coin.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(Random.Range(-100, 100), -600);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            coin.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            // Fade out towards the end
            if (t > 0.8f)
            {
                Color color = coin.GetComponent<Image>().color;
                color.a = Mathf.Lerp(1f, 0f, (t - 0.8f) / 0.2f);
                coin.GetComponent<Image>().color = color;
            }

            yield return null;
        }
    }

    // 🔧 HELPERS
    bool IsValidMatch(SymbolType a, SymbolType b, SymbolType c)
    {
        SymbolType baseSymbol = SymbolType.Wild;

        if (a != SymbolType.Wild) baseSymbol = a;
        else if (b != SymbolType.Wild) baseSymbol = b;
        else if (c != SymbolType.Wild) baseSymbol = c;

        return (a == baseSymbol || a == SymbolType.Wild) &&
               (b == baseSymbol || b == SymbolType.Wild) &&
               (c == baseSymbol || c == SymbolType.Wild);
    }

    SymbolType GetMainSymbol(SymbolType a, SymbolType b, SymbolType c)
    {
        if (a != SymbolType.Wild) return a;
        if (b != SymbolType.Wild) return b;
        return c;
    }

    int Count(SymbolType t, SymbolType a, SymbolType b, SymbolType c)
    {
        int count = 0;
        if (a == t) count++;
        if (b == t) count++;
        if (c == t) count++;
        return count;
    }

    public bool IsNearMiss(SymbolType a, SymbolType b, SymbolType c)
    {
        if (a == b && a != c) return true;
        if (b == c && b != a) return true;
        if (a == c && a != b) return true;

        return false;
    }
}