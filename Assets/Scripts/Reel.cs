using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Image[] slots;              
    public Sprite[] symbolSprites;

    public float spinSpeed = 0.05f;
    public int spinCycles = 20;

    private int currentCenterIndex;

    public IEnumerator Spin()
    {
        for (int i = 0; i < spinCycles; i++)
        {
            SpinStep();
            yield return new WaitForSeconds(spinSpeed);
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

    public int GetResult()
    {
        return currentCenterIndex;
    }
}