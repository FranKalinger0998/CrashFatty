using UnityEngine;

public class GoldenBall : BaseBall,IPoolItem
{
    private Renderer renderer;

    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1.0f, 0.84f, 0.0f); // Golden color
            
        }
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        //gameObject.SetActive(false);
        ObjectPool.Instance.ReturnToPool(gameObject, ObjectPool.PoolType.Gold);
    }
}