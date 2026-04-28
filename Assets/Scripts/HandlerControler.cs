using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleController : MonoBehaviour
{
    public Image handleImage;
    public Sprite upSprite;
    public Sprite downSprite;

    public IEnumerator Pull()
    {
        handleImage.sprite = downSprite;
        yield return new WaitForSeconds(0.2f);
        handleImage.sprite = upSprite;
    }
}