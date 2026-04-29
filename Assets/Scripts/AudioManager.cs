using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip spinLoop;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip leverSound;

    private void Awake() => instance = this;

    public void PlaySpin()
    {
        sfxSource.loop = true;
        sfxSource.clip = spinLoop;
        sfxSource.Play();
    }

    public void StopSpin() => sfxSource.Stop();
    public void PlayWin() => sfxSource.PlayOneShot(winSound);
    public void PlayLever() => sfxSource.PlayOneShot(leverSound);
}