using UnityEngine;
using UnityEngine.UI;

public class CoinDisplayer : MonoBehaviour
{
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        CoinManager.Instance.CoinCountChange += UpdateText;
    }

    private void OnDisable()
    {
        CoinManager.Instance.CoinCountChange -= UpdateText;
    }

    private void UpdateText(int coinCount)
    {
        _text.text = coinCount.ToString();
    }
}
