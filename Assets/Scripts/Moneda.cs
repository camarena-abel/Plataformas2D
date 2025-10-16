using UnityEngine;

public class Moneda : MonoBehaviour
{
    Transform dibujo;

    [SerializeField]
    float radius = 0.2f;

    private void Start()
    {
        dibujo = transform.Find("Dibujo");
    }

    void Update()
    {
        float y = Mathf.Sin(Time.realtimeSinceStartup) * radius;
        dibujo.localPosition = new Vector3(dibujo.localPosition.x, y, dibujo.localPosition.z);
    }
}
