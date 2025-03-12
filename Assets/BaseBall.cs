using System.Collections;
using UnityEngine;

public class BaseBall : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 10f;
    public float lerpDuration = 20f;

    protected Rigidbody rb;
    protected Vector3 lastVelocity;
    public float currentSpeed;
    private Coroutine speedIncreaseCoroutine;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider col = GetComponent<Collider>();
        if (col != null && col.material == null)
        {
            PhysicsMaterial bouncyMat = new PhysicsMaterial("Bouncy")
            {
                bounciness = 1f,
                dynamicFriction = 0f,
                staticFriction = 0f
            };
            col.material = bouncyMat;
        }

        ResetBall();
    }

    protected virtual void Update()
    {
        lastVelocity = rb.linearVelocity;
        rb.linearVelocity = lastVelocity.normalized * currentSpeed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflectDirection = Vector3.Reflect(lastVelocity.normalized, normal);
            try
            {
                rb.linearVelocity = reflectDirection * currentSpeed;
            }
            catch 
            {

                
            }
            
        }
    }

    public virtual void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    //void OnEnable()
    //{
    //    ResetBall();
    //}

    public void ResetBall()
    {
        if (speedIncreaseCoroutine != null)
        {
            StopCoroutine(speedIncreaseCoroutine);
        }

        //transform.position = Vector3.zero;
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;

        currentSpeed = minSpeed;
        try
        {
            rb.linearVelocity = randomDirection.normalized * currentSpeed;
        }
        catch 
        {

            
        } 

        speedIncreaseCoroutine = StartCoroutine(IncreaseSpeedOverTime());
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, elapsedTime / lerpDuration);
            try
            {
                rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
            }
            catch 
            {

                
            }
            

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentSpeed = maxSpeed;
        rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}
