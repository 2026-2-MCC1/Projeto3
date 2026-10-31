using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static readonly List<Note> Active = new List<Note>();

    public int Lane { get; private set; }
    public Vector3 Direction => direction;
    public bool Judged { get; private set; }

    float speed;
    Vector3 direction;

    void OnEnable() { Active.Add(this); }
    void OnDisable() { Active.Remove(this); }

    public void Init(float speed, Vector3 direction, float lifetime, int lane)
    {
        this.speed = speed;
        this.direction = direction.normalized;
        Lane = lane;
        Destroy(gameObject, lifetime);
    }

    public void Consume()
    {
        Judged = true;
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
