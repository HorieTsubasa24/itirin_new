using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Jump : Gimic {
    private readonly float[] height = { -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f, -10f};
	public override void Init(float height)
	{
        Span = rand.Next(45) + 90;
		vel = new Vector2(-0.14f, 0.0f);
		vec = new Vector2(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x, height - GetComponent<SpriteRenderer>().bounds.size.y / 2);
        transform.position = vec;
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}
    
}
