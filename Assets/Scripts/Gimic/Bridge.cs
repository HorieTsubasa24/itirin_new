using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Gimic
{
	// Use this for initialization
	public override void Init()
	{
		Span = 90;
		Hp = 1;
		vec = transform.position;
		var rb = GetComponent<Rigidbody2D>();
		acs = new Vector2(0.0f, 0.0f);
		vel = new Vector2(-0.14f, 0.0f);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}
}
