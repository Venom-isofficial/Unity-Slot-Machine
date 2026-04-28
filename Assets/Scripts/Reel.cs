using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Image[] slots;              // size = 3 (top, center, bottom)
    public Sprite[] symbolSprites;     // your symbols (7, cherry, bell, etc)

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
    }

    void SpinStep()
    {
        // shift down
        slots[2].sprite = slots[1].sprite;
        slots[1].sprite = slots[0].sprite;

        // new random at top
        int rand = Random.Range(0, symbolSprites.Length);
        slots[0].sprite = symbolSprites[rand];

        // update center index
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