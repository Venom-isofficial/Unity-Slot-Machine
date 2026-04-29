using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [SerializeField] private Image[] slots;  // top,center,Bottom
    [SerializeField] private Sprite[] symbolSprites;
    [SerializeField] private float minSpeed = 0.02f;
    [SerializeField] private float maxSpeed = 0.12f;
    [SerializeField] private int spinCycles = 25;
    [SerializeField] private int decelerationSteps = 8;

    private int currentCenterIndex;

    public IEnumerator Spin(bool isLast = false, bool anticipation = false)
    {
        float speed = minSpeed;

        // Spinnnnning
        for (int i = 0; i < spinCycles; i++)
        {
            SpinStep();
            if (anticipation && i > spinCycles * 0.6f)
                speed = Mathf.Lerp(speed, maxSpeed, 0.1f);
            yield return new WaitForSeconds(speed);
        }

        // Last reel stops with smooth deceleration
        if (isLast)
            yield return StartCoroutine(DecelerateSpin());
    }

    // For debug test all bottom 
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
            yield return StartCoroutine(DecelerateSpin());

        ForceSymbol(targetSymbol);
    }

    private IEnumerator DecelerateSpin()
    {
        for (int i = 0; i < decelerationSteps; i++)
        {
            SpinStep();
            float t = i / (float)decelerationSteps;
            float easeOut = 1 - Mathf.Pow(1 - t, 3);  // Smooth curve
            float decelerationSpeed = Mathf.Lerp(minSpeed, maxSpeed * 2, easeOut);
            yield return new WaitForSeconds(decelerationSpeed);
        }
    }

    private void SpinStep()
    {
        slots[2].sprite = slots[1].sprite;  // Bottom <- Center
        slots[1].sprite = slots[0].sprite;  // Center <- Top
        slots[0].sprite = symbolSprites[Random.Range(0, symbolSprites.Length)];
        currentCenterIndex = GetSpriteIndex(slots[1].sprite);
    }

    private int GetSpriteIndex(Sprite sprite)
    {
        for (int i = 0; i < symbolSprites.Length; i++)
            if (symbolSprites[i] == sprite) return i;
        return -1;
    }

    public SymbolType GetSymbolType() => (SymbolType)GetSpriteIndex(slots[1].sprite);
    public Image GetCenterSlot() => slots[1];

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