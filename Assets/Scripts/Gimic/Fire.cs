using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gimic
{
	// Use this for initialization
	public override void Init(float height)
	{
		Hp = 3;
		acs = new Vector2(0.0f, 0.0f);
		vel = new Vector2(-0.05f, 0.0f);
		vec = new Vector2(transform.position.x, height);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}
}
