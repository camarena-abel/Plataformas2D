using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    Transform tgtA;
    Transform tgtB;
    bool forward = true;

    [SerializeField]
    float speed = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tgtA = transform.parent.Find("TargetA");
        tgtB = transform.parent.Find("TargetB");
    }

    void FixedUpdate()
    {
        Vector3 dist;
        if (forward)
        {
            dist = (tgtB.position - transform.position);
        } else
        {
            dist = (tgtA.position - transform.position);
        }

        if (dist.magnitude < 0.01f)
        {
            forward = !forward;
        }
        rb.MovePosition(transform.position + dist.normalized * Time.fixedDeltaTime * speed);
    }
}
