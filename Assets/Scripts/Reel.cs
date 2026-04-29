using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Image[] slots;
    public Sprite[] symbolSprites;

    public float minSpinSpeed = 0.02f;
    public float maxSpinSpeed = 0.1f;
    public int spinCycles = 25;

    private int currentCenterIndex;

    public IEnumerator Spin(bool isLastReel = false, bool anticipation = false)
    {
        float speed = minSpinSpeed;

        for (int i = 0; i < spinCycles; i++)
        {
            SpinStep();

            //  Anticipation slow down
            if (anticipation && i > spinCycles * 0.6f)
                speed = Mathf.Lerp(speed, maxSpinSpeed, 0.1f);

            yield return new WaitForSeconds(speed);
        }

        //  Final slow dramatic stop
        if (isLastReel)
        {
            for (int i = 0; i < 5; i++)
            {
                SpinStep();
                yield return new WaitForSeconds(maxSpinSpeed + (i * 0.02f));
            }
        }

        AudioManager.instance.PlayStopTick();
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
            if (symbolSprites[i] == s)
                return i;
        }
        return -1;
    }

    public SymbolType GetSymbolType()
    {
        return (SymbolType)GetSpriteIndex(slots[1].sprite);
    }
}