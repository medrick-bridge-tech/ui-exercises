using UnityEngine;

public class CollectAndProgress : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private CoinSpawner _coinSpawner;

    private void Awake()
    {
        _progressBar.Setup(10, 0);
        _coinSpawner.SetAddCoinAction((amount) => _progressBar.AddAmountToProgress(amount));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _coinSpawner.AddCoin(3);
        }
    }
}