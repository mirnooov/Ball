using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<Ball>();

        if (ball != null)
        {
            CoinManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
