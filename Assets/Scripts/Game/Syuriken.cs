using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syuriken : MonoBehaviour
{
    public Vector2 acs;
    public Vector2 vel;
    public Vector2 vec;
    Unicycle unicycle;

    public int ThrowFlame = 35;
    public int DestroyFlame = 60;
    public void Init(Unicycle uni, Vector2 vec1, Vector2 vec2)
    {
        float signx = Mathf.Sign(vec2.x);
        float signy = Mathf.Sign(vec2.y);

        vec1.x += signx * 1.22f;
        vec1.y += signy * 1.22f;
        if (Mathf.Abs(vec2.x) <= 1.3f) vec2.x = Mathf.Sign(vec2.x) * 1.3f;
        if (Mathf.Abs(vec2.y) <= 1.3f) vec2.y = Mathf.Sign(vec2.y) * 1.3f;

        unicycle = uni;
        Vector2 vvel = (vec2 - vec1) / ThrowFlame;
        vvel.y += 0.001f;

        acs = new Vector2(0.0f, -0.001f);
        vel = new Vector2(vvel.x, vvel.y);
        vec = new Vector2(vec1.x, vec1.y);
    }

    private void FixedUpdate()
    {
        if (DestroyFlame == 0 ||
            transform.position.x > 20.0f || transform.position.x < -20.0f ||
            transform.position.y < -10.0f || transform.position.y > 20.0f)
            Destroy(gameObject);
        vel += acs;
        vec += vel;
        transform.position = vec;

        DestroyFlame--;
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
    /// 敵に接触
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Unicycle")
            unicycle.Damage(10);
    }
}
