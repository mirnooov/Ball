using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int _coinCount;

    public Action<int> CoinCountChange;
    
    private void Awake() => Instance = this;

    public void AddCoin() => CoinCountChange?.Invoke(++_coinCount);
    public void RemoveCoin() => CoinCountChange?.Invoke(--_coinCount);
}
