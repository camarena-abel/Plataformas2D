using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float minX = -10f;
    [SerializeField]
    float maxX = 10f;

    [SerializeField]
    [Range(0.1f, 1f)]
    float speed = 0.5f; // velocidad de la interpolacion

    void FixedUpdate()
    {        
        float x = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 desiredPos = new Vector3(x, transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, desiredPos, 0.5f);
    }
}
