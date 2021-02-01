using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CoinCollection : MonoBehaviour
{
    public float speed;
    public Transform target;
    //public Transform initial;
    public GameObject coinPreFab;
    public Camera cam;
    public Ease easeType;

    public TMP_Text coinUIText;
    public float coins = 0;

    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;

    [SerializeField] float spread;

    private int _c = 0;

    public int Coins
    {
        get { return _c; }
        set
        {
            _c = value;
            //update UI text whenever "Coins" variable is changed
            coinUIText.text = Coins.ToString();
        }
    }

    void Awake()
    {
        //prepare pool
        PrepareCoins();
    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(coinPreFab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }



    private void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    public void StartCoinMove(Vector3 initial, int amount)
    {

        Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(
                target.position.x, target.position.y, cam.transform.position.z * -1));

        for (int i = 0; i < amount; i++)
        {

            if (coinsQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = initial + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(targetPos, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);

                    Coins++;
                });
            }
        }
        
    }

}
