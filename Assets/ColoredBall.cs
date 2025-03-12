
using UnityEngine;

public enum PlayerColor { Default, Red, Green, Blue, Yellow }

public class ColoredBall : BaseBall,IPoolItem
{
    public PlayerColor currentColor = PlayerColor.Default;
    private Renderer renderer;

    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                currentColor = player.selectedColor;
                ApplyColor(currentColor);
            }
        }
    }

    private void ApplyColor(PlayerColor color)
    {
        if (renderer == null) return;
        renderer.material.color = GetColor(color);
    }
    private Color GetColor(PlayerColor color)
    {
        return color switch
        {
            PlayerColor.Red => Color.red,
            PlayerColor.Blue => Color.blue,
            PlayerColor.Green => Color.green,
            PlayerColor.Yellow => Color.yellow,
            _ => Color.white
        };
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        //gameObject.SetActive(false);
        ObjectPool.Instance.ReturnToPool(gameObject,ObjectPool.PoolType.Color);
    }

    void OnEnable()
    {
        ResetBall();
        currentColor = PlayerColor.Default;
        ApplyColor(currentColor);
        currentSpeed = 5;
    }
}