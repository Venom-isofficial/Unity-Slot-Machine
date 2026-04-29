using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;

    public AudioClip spinLoop;
    public AudioClip stopTick;
    public AudioClip winSound;
    public AudioClip leverSound;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySpin()
    {
        sfxSource.loop = true;
        sfxSource.clip = spinLoop;
        sfxSource.Play();
    }

    public void StopSpin()
    {
        sfxSource.Stop();
    }

    public void PlayStopTick()
    {
        sfxSource.PlayOneShot(stopTick);
    }

    public void PlayWin()
    {
        sfxSource.PlayOneShot(winSound);
    }

    public void PlayLever()
    {
        sfxSource.PlayOneShot(leverSound);
    }
}