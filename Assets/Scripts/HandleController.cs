using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandleController : MonoBehaviour
{
    public Image handleImage;

    public Sprite handleUp;
    public Sprite handleDown;

    public float pressTime = 0.15f;

    public IEnumerator PullHandle()
    {
        // play sound
        AudioManager.instance.PlayLever();

        // switch to DOWN sprite
        handleImage.sprite = handleDown;

        yield return new WaitForSeconds(pressTime);

        // back to UP sprite
        handleImage.sprite = handleUp;
    }
}