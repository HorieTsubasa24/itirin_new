using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Jump : Gimic {
	public override void Init(float height)
	{
		vel = new Vector2(-0.14f, 0.0f);
		vec = new Vector2(transform.position.x, height);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}
}
