using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Gimic {
    private float[] heights = { -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f};
	public override void Init(float height)
    {
        heightline = new float[heights.Length];
        heightline = heights;
        like = Like.Gimic;
        rand = new System.Random();
        Span = rand.Next(45) + 90;
        acs = new Vector2();
		vel = new Vector2(-0.14f, 0.0f);
		vec = new Vector2(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x, height - GetComponent<SpriteRenderer>().bounds.size.y / 2);
        transform.position = vec;
	}

	public override void Move()
    {
        vel += acs;
        vec += vel;
		transform.position = vec;
	}

    /// <summary>
    /// 自機に接触
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Unicycle")
            JumpSmash();
    }

    private void JumpSmash()
    {
        unicycle.Landing();
        unicycle.Jump(true, 44);
    }
}
