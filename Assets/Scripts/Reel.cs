using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Image[] slots;
    public Sprite[] symbolSprites;

    public float minSpeed = 0.02f;
    public float maxSpeed = 0.12f;
    public int spinCycles = 25;

    private int currentCenterIndex;

    public IEnumerator Spin(bool isLast = false, bool anticipation = false)
    {
        float speed = minSpeed;

        for (int i = 0; i < spinCycles; i++)
        {
            SpinStep();

            if (anticipation && i > spinCycles * 0.6f)
                speed = Mathf.Lerp(speed, maxSpeed, 0.1f);

            yield return new WaitForSeconds(speed);
        }

        if (isLast)
        {
            for (int i = 0; i < 5; i++)
            {
                SpinStep();
                yield return new WaitForSeconds(maxSpeed + i * 0.02f);
            }
        }
    }

    void SpinStep()
    {
        slots[2].sprite = slots[1].sprite;
        slots[1].sprite = slots[0].sprite;

        int rand = Random.Range(0, symbolSprites.Length);
        slots[0].sprite = symbolSprites[rand];

        currentCenterIndex = GetSpriteIndex(slots[1].sprite);
    }

    int GetSpriteIndex(Sprite s)
    {
        for (int i = 0; i < symbolSprites.Length; i++)
        {
            if (symbolSprites[i] == s) return i;
        }
        return -1;
    }

    public SymbolType GetSymbolType()
    {
        return (SymbolType)GetSpriteIndex(slots[1].sprite);
    }

    public Image GetCenterSlot()
    {
        return slots[1];
    }

    // 🐛 DEBUG: Spin to specific symbol
    public IEnumerator SpinToSymbol(SymbolType targetSymbol, bool isLast = false, bool anticipation = false)
    {
        float speed = minSpeed;

        for (int i = 0; i < spinCycles; i++)
        {
            SpinStep();

            if (anticipation && i > spinCycles * 0.6f)
                speed = Mathf.Lerp(speed, maxSpeed, 0.1f);

            yield return new WaitForSeconds(speed);
        }

        if (isLast)
        {
            for (int i = 0; i < 5; i++)
            {
                SpinStep();
                yield return new WaitForSeconds(maxSpeed + i * 0.02f);
            }
        }

        // Force the target symbol on center
        ForceSymbol(targetSymbol);
    }

    // 🐛 DEBUG: Force a specific symbol to center
    public void ForceSymbol(SymbolType targetSymbol)
    {
        int targetIndex = (int)targetSymbol;
        if (targetIndex < symbolSprites.Length)
        {
            slots[1].sprite = symbolSprites[targetIndex];
            currentCenterIndex = targetIndex;
        }
    }
}