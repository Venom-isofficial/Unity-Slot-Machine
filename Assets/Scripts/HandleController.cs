using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandleController : MonoBehaviour
{
    [SerializeField] private Image handleImage;
    [SerializeField] private Sprite handleUp;
    [SerializeField] private Sprite handleDown;
    [SerializeField] private float pressTime = 0.15f;

    public IEnumerator PullHandle()
    {
        AudioManager.instance.PlayLever();
        handleImage.sprite = handleDown;
        yield return new WaitForSeconds(pressTime);
        handleImage.sprite = handleUp;
    }
}