using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Reel reel1;
    public Reel reel2;
    public Reel reel3;

    public GameObject winPopup;

    public void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        yield return StartCoroutine(reel1.Spin());
        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(reel2.Spin());
        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(reel3.Spin());

        CheckWin();
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
        }
        else
        {
            Debug.Log("Lose");
        }
    }
}