using UnityEngine;

public class PetBob : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.1f;
    private Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * height;
        transform.position = startPos + new Vector3(0, y, 0);
    }
}