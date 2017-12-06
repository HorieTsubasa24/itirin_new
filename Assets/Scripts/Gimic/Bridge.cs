using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Gimic
{
private readonly float[] heights = { 0f, -0.0445f, -0.088f, -0.1305f, -0.172f, -0.2125f, -0.252f, -0.2905f, -0.328f, -0.3645f, -0.4f, -0.4345f, -0.468f, -0.5005f, -0.532f, -0.5625f, -0.592f, -0.6205f, -0.648f, -0.6745f, -0.7f, -0.7245f, -0.748f, -0.7705f, -0.792f, -0.8125f, -0.832f, -0.8505f, -0.868f, -0.8845f, -0.9f, -0.9145f, -0.928f, -0.9405f, -0.952f, -0.9625f, -0.972f, -0.9805f, -0.988f, -0.9945f, -1f, -1.0045f, -1.008f, -1.0105f, -1.012f, -1.0125f, -1.012f, -1.0105f, -1.008f, -1.0045f, -1f, -0.9945f, -0.988f, -0.9805f, -0.972f, -0.9625f, -0.952f, -0.9405f, -0.928f, -0.9145f, -0.9f, -0.8845f, -0.868f, -0.8505f, -0.832f, -0.8125f, -0.792f, -0.7705f, -0.748f, -0.7245f, -0.7f, -0.6745f, -0.648f, -0.6205f, -0.592f, -0.5625f, -0.532f, -0.5005f, -0.468f, -0.4345f, -0.4f, -0.3645f, -0.328f, -0.2905f, -0.252f, -0.2125f, -0.172f, -0.1305f, -0.088f, -0.0445f, 0f};

	// Use this for initialization
	public override void Init(float height)
	{
		Span = 90;
		heightline = heights;
		Hp = 1;
		vec = new Vector2(transform.position.x, height);
		var rb = GetComponent<Rigidbody2D>();
		acs = new Vector2(0.0f, 0.0f);
		vel = new Vector2(-0.14f, 0.0f);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
		if (vec.x < -20.0f) Destroy(gameObject);
	}
}
