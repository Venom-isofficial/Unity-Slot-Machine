using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Reel reel1;
    public Reel reel2;
    public Reel reel3;

    public GameObject winPopup;

    public HandleController handle;

    private bool spinning = false;

    public void Spin()
    {
        if (spinning) return;

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        spinning = true;

        winPopup.SetActive(false);

        // handle animation
        yield return StartCoroutine(handle.PullHandle());

        // start sound
        AudioManager.instance.PlaySpin();

        yield return StartCoroutine(reel1.Spin());
        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(reel2.Spin());
        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(reel3.Spin());

        // stop spin sound
        AudioManager.instance.StopSpin();

        CheckWin();

        spinning = false;
    }

    void CheckWin()
    {
        int r1 = reel1.GetResult();
        int r2 = reel2.GetResult();
        int r3 = reel3.GetResult();

        if (r1 == r2 && r2 == r3)
        {
            Debug.Log("WIN 🎉");
            winPopup.SetActive(true);
            AudioManager.instance.PlayWin();
        }
    }
}