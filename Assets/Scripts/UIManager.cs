using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text balanceText;

    public void UpdateBalance(int amount)
    {
        balanceText.text = "Balance: " + amount;
    }
}