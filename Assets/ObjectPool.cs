using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public enum PoolType { Color, Gold }

    [SerializeField] private GameObject colorBallPrefab;
    [SerializeField] private GameObject goldBallPrefab;

    [SerializeField] private int initialColorPoolSize = 10;
    [SerializeField] private int initialGoldPoolSize = 10;

    private readonly Dictionary<PoolType, Queue<GameObject>> pools = new();
    private readonly Dictionary<PoolType, GameObject> prefabs = new();
    private readonly List<GameObject> activeBalls = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        prefabs[PoolType.Color] = colorBallPrefab;
        prefabs[PoolType.Gold] = goldBallPrefab;

        pools[PoolType.Color] = new Queue<GameObject>();
        pools[PoolType.Gold] = new Queue<GameObject>();

        InitializePool(PoolType.Color, initialColorPoolSize);
        InitializePool(PoolType.Gold, initialGoldPoolSize);
    }

    private void InitializePool(PoolType type, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefabs[type]);
            obj.SetActive(false);
            pools[type].Enqueue(obj);
        }
    }

    public GameObject GetFromPool(PoolType type)
    {
        if (!pools.ContainsKey(type)) return null;

        GameObject obj = pools[type].Count > 0 ? pools[type].Dequeue() : Instantiate(prefabs[type]);

        obj.SetActive(true);
        obj.transform.DOKill(); 
        obj.transform.position = new Vector3(0, obj.transform.position.y, 0);

        // Ensure ball properties are reset
        if (obj.TryGetComponent<BaseBall>(out var baseBall))
        {
            baseBall.ResetBall();
        }

        activeBalls.Add(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj, PoolType type)
    {
        if (!pools.ContainsKey(type)) return;

        obj.SetActive(false);
        pools[type].Enqueue(obj);
        activeBalls.Remove(obj);
    }

    public List<GameObject> GetAllActiveBalls() => new List<GameObject>(activeBalls);
}
