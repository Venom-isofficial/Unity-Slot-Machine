using UnityEngine;

public enum SymbolType
{
    Apple,
    Bar,
    Bonus,
    Cherry,
    Coin,
    Crown,
    Seven,
    Watermelon,
    Wild
}

public class WinManager : MonoBehaviour
{
    public int CalculateWin(SymbolType a, SymbolType b, SymbolType c, int bet)
    {
        // BONUS FIRST
        int bonusCount = Count(SymbolType.Bonus, a, b, c);
        if (bonusCount == 3) return bet * 30;
        if (bonusCount == 2) return bet * 5;

        // JACKPOT (3 wild)
        if (a == SymbolType.Wild && b == SymbolType.Wild && c == SymbolType.Wild)
            return bet * 25;

        // STRICT MATCH
        if (!IsValidMatch(a, b, c))
            return 0;

        SymbolType main = GetMainSymbol(a, b, c);

        switch (main)
        {
            case SymbolType.Crown: return bet * 20;
            case SymbolType.Coin: return bet * 15;
            case SymbolType.Seven: return bet * 12;
            case SymbolType.Bar: return bet * 10;
            case SymbolType.Watermelon: return bet * 8;
            case SymbolType.Apple: return bet * 6;
            case SymbolType.Cherry: return bet * 5;
        }

        return 0;
    }

    // 🔥 STRICT MATCH CHECK
    bool IsValidMatch(SymbolType a, SymbolType b, SymbolType c)
    {
        SymbolType baseSymbol = SymbolType.Wild;

        if (a != SymbolType.Wild) baseSymbol = a;
        else if (b != SymbolType.Wild) baseSymbol = b;
        else if (c != SymbolType.Wild) baseSymbol = c;

        return (a == baseSymbol || a == SymbolType.Wild) &&
               (b == baseSymbol || b == SymbolType.Wild) &&
               (c == baseSymbol || c == SymbolType.Wild);
    }

    // pick actual symbol (ignoring wild)
    SymbolType GetMainSymbol(SymbolType a, SymbolType b, SymbolType c)
    {
        if (a != SymbolType.Wild) return a;
        if (b != SymbolType.Wild) return b;
        return c;
    }

    int Count(SymbolType t, SymbolType a, SymbolType b, SymbolType c)
    {
        int count = 0;
        if (a == t) count++;
        if (b == t) count++;
        if (c == t) count++;
        return count;
    }

    // 🎯 NEAR MISS SYSTEM
    public bool IsNearMiss(SymbolType a, SymbolType b, SymbolType c)
    {
        // example: 2 same, 1 different
        if (a == b && a != c) return true;
        if (b == c && b != a) return true;
        if (a == c && a != b) return true;

        return false;
    }
}