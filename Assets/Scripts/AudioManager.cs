using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip spinSound;
    public AudioClip winSound;

    public void PlaySpin()
    {
        source.PlayOneShot(spinSound);
    }

    public void PlayWin()
    {
        source.PlayOneShot(winSound);
    }
}