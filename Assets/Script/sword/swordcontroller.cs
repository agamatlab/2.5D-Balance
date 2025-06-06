using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour
{
    public enum SwordState { Held, Thrown, Landed };
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trail;
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    public SwordState state = SwordState.Held;
    MyPlayerMovement playerMovementScript;
    Transform playerBody;

    void Start()
    {
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (trail != null) trail.emitting = false;

        playerMovementScript = GetComponentInParent<MyPlayerMovement>();
        gameObject.layer = 3;
        playerBody = GameObject.Find("Player").transform;
    }

    public void Throw(Vector3 direction, float force)
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(direction * force, ForceMode.Impulse);
        state = SwordState.Thrown;

        if (trail != null) trail.emitting = true;
    }

    public void Retake()
    {
        rb.isKinematic = true;
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
        transform.localRotation = originalLocalRotation;
        state = SwordState.Held;
        playerMovementScript.isHoldingRight = true;

        if (trail != null) trail.emitting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == SwordState.Thrown)
        {
            state = SwordState.Landed;
            rb.velocity = Vector3.zero;
        }
    }

    public bool IsHeld => state == SwordState.Held;

    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(0.4f);
        if (!playerMovementScript.facingLeft)
        {
            Throw(new Vector3(5, 2, 0), 1f);
        }
        else
        {
            Throw(new Vector3(-5, 2, 0), 1f);
        }
    }

    bool check2DCollide()
    {
        float x1 = transform.position.x;
        float x2 = playerBody.position.x;
        float diff = x1 - x2;
        return diff < 1 && diff > -1;
    }

    void Update()
    {
        if (state == SwordState.Held && !playerMovementScript.isHoldingRight)
        {
            StartCoroutine(DelayedAction());
            state = SwordState.Thrown;
        }
        if (state == SwordState.Landed && check2DCollide())
        {
            Retake();
        }
    }
}