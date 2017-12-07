using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gimic
{
	// Use this for initialization
	public override void Init(float height)
	{
		Hp = 3;
		acs = new Vector2(-0.00001f, 0.0f);
		vel = new Vector2(-0.15f, 0.0f);
		vec = new Vector2(transform.position.x, height + 2.0f);
        transform.position = vec;
    }

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}
}
