using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reel : MonoBehaviour
{
    public Image symbolImage;
    public Sprite[] symbols;

    public float spinSpeed = 0.05f;

    private Sprite finalSymbol;

    public IEnumerator Spin(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            symbolImage.sprite = GetWeightedSymbol();
            timer += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        finalSymbol = symbolImage.sprite;
    }

    Sprite GetWeightedSymbol()
    {
        int rand = Random.Range(0, 100);

        if (rand < 50) return symbols[0]; // cherry
        if (rand < 75) return symbols[1]; // bell
        if (rand < 90) return symbols[2]; // bar
        return symbols[3]; // seven
    }

    public Sprite GetFinalSymbol()
    {
        return finalSymbol;
    }
}