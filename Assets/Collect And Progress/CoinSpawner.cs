using UnityEngine;
using DG.Tweening;

public class CoinSpawner : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] Transform _coinMoveTarget;
    [SerializeField] ObjectPool _coinPool;

    [Header("Coins Animation Options")]
    [SerializeField] int _maxCoinCount = 10;
    [SerializeField] Ease _easeType = Ease.InBack;
    [SerializeField] float _animationDurationInSeconds = 1f;
    [SerializeField] float _animationDurationVariance = .25f;
    [SerializeField] private float _finishingScaleFactor = 1;

    [Header("Spread Options")]
    [SerializeField] private float scaleUpDuration = .2f;
    [SerializeField] bool _spreadSpawn = true;
    [SerializeField] bool _spreadTarget = true;
    [SerializeField] [Range(0f, 1f)] float _spreadIntensity = 0;
    
    [Header("Target Feedback Options")]
    [SerializeField] bool _targetAnimationFeedback = true;
    [SerializeField] [Range(1f, 2f)] float _targetFeedbackScaleValue = 1.05f;
    [SerializeField] float _targetFeedbackAnimationDuration = 0.05f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    private System.Action<int> _addCoinAction;
    private int _spreadFactor;
    private Tween feedbackTween;

    void Awake() {
        DOTween.Init();
        _maxCoinCount = Mathf.Min(_coinPool._poolSize, _maxCoinCount);
        _spreadFactor = Screen.width / 5;
    }

    void Start() {
        // WARNING: This may affect other tweeners
        DOTween.defaultAutoPlay = AutoPlay.None;
        feedbackTween = _coinMoveTarget.DOScale(Vector3.one * _targetFeedbackScaleValue, _targetFeedbackAnimationDuration);
        feedbackTween.SetLoops(2, LoopType.Yoyo).SetAutoKill(false);
    }

    public void SetAddCoinAction(System.Action<int> action) => _addCoinAction = action;

    public void AddCoin(int amount) => AddCoin(spawnPoint.position, amount);

    public void AddCoin(Vector3 spawnPosition, int amount) {
        int coinsToSpawn = amount > _maxCoinCount ? _maxCoinCount : amount;
        int coinValue = amount / coinsToSpawn;
        int coinChange = amount % coinsToSpawn;

        for(int i = 0; i < coinsToSpawn; i++) {
            GameObject coin = _coinPool.GetObject();
            if (coin == null) {
                Debug.LogWarning("The pool is not ready yet, force adding ");
                _addCoinAction?.Invoke((coinsToSpawn - i) * coinValue + coinChange);
                return;
            }
            coin.transform.position = spawnPosition + (_spreadSpawn ? RandomHelper.RandomVector3(_spreadIntensity * _spreadFactor) : Vector3.zero);
            coin.SetActive(true);
            coin.transform.localScale = Vector3.zero;
            

            Sequence coinMoveSequence = DOTween.Sequence();
            Vector3 target = _coinMoveTarget.position + (_spreadTarget ? RandomHelper.RandomVector3(_spreadIntensity * _spreadFactor) : Vector3.zero);
            var duration = _animationDurationInSeconds + Random.Range(0, _animationDurationVariance);

            coinMoveSequence.Append(coin.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(Ease.OutQuart));
            coinMoveSequence.Append(coin.transform.DOScale(_finishingScaleFactor * Vector3.one, duration).SetEase(_easeType));
            coinMoveSequence.Insert(0, coin.transform.DOMove(target, duration).SetEase(_easeType).OnComplete(
                () =>
                {
                    coin.SetActive(false);
                    _addCoinAction?.Invoke(coinValue + coinChange);
                    coinChange = 0;
                    audioSource.Play();
                    if (_targetAnimationFeedback && !feedbackTween.IsPlaying()) feedbackTween.Restart();
                }));
            coinMoveSequence.Play();
        }
    }
}