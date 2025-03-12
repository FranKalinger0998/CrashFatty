
//using DG.Tweening;
//using UnityEngine;

//public enum PlayerColor { Default, Red, Green, Blue, Yellow }
//public enum BallType { Default, Golden }
//public class BallScript : MonoBehaviour
//{

//    public PlayerColor currentColor = PlayerColor.Default;

//    public float minSpeed = 2f;  // Starting speed
//    public float maxSpeed = 10f; // Maximum speed cap
//    public float lerpDuration = 20f; // Time to reach max speed
//    public float deviationAngle = 15f; // Adds randomness to each bounce

//    private Rigidbody rb;
//    private Vector3 lastVelocity;
//    private float currentSpeed;
//    private Renderer renderer;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.useGravity = false;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        rb.interpolation = RigidbodyInterpolation.Interpolate;

//        renderer = GetComponent<Renderer>();
//        if (renderer != null)
//        {
//            renderer.material.color = Color.white; // Default color
//        }

//        Collider col = GetComponent<Collider>();
//        if (col != null && col.material == null)
//        {
//            PhysicsMaterial bouncyMat = new PhysicsMaterial("Bouncy")
//            {
//                bounciness = 1f,
//                dynamicFriction = 0f,
//                staticFriction = 0f
//            };
//            col.material = bouncyMat;
//        }

//        Vector3 randomDirection = Random.onUnitSphere;
//        randomDirection.y = 0;
//        currentSpeed = minSpeed;

//        DOTween.To(() => currentSpeed, x => currentSpeed = x, maxSpeed, lerpDuration)
//            .SetEase(Ease.Linear);

//        rb.linearVelocity = randomDirection.normalized * currentSpeed;
//    }

//    void Update()
//    {
//        lastVelocity = rb.linearVelocity;
//        rb.linearVelocity = lastVelocity.normalized * currentSpeed;
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
//            if (player != null)
//            {
//                currentColor = player.selectedColor;
//                ApplyColor(currentColor);
//            }
//        }

//        Vector3 normal = collision.contacts[0].normal;
//        Vector3 reflectDirection = Vector3.Reflect(lastVelocity.normalized, normal);
//        rb.linearVelocity = reflectDirection * currentSpeed;
//    }

//    private void ApplyColor(PlayerColor color)
//    {
//        if (renderer == null) return;
//        switch (color)
//        {
//            case PlayerColor.Red:
//                renderer.material.color = Color.red;
//                break;
//            case PlayerColor.Green:
//                renderer.material.color = Color.green;
//                break;
//            case PlayerColor.Blue:
//                renderer.material.color = Color.blue;
//                break;
//            case PlayerColor.Yellow:
//                renderer.material.color = Color.yellow;
//                break;
//            default:
//                renderer.material.color = Color.gray;
//                break;
//        }
//    }
//    public void ReturnToPool()
//    {
//        //
//    }
//}
