using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syuriken : MonoBehaviour
{
    public Vector2 acs;
    public Vector2 vel;
    public Vector2 vec;
    Unicycle unicycle;

    // Use this for initialization
    private void Start()
    {
        unicycle = GameObject.Find("Unicycle").GetComponent<Unicycle>();
        acs = new Vector2(0.0f, -0.001f);
        vel = new Vector2(-0.075f, 0.0f);
        vec = new Vector2(transform.position.x, transform.position.y + 0.5f);
        vec = transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.x > 20.0f || transform.position.x < -20.0f ||
            transform.position.y < -10.0f || transform.position.y > 20.0f)
            Destroy(gameObject);
        vel += acs;
        vec += vel;
        transform.position = vec;
    }

    /// <summary>
    /// 地面に接触
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 自機に接触
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Unicycle")
            unicycle.Damage(10);
    }
}
