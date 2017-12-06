using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : Gimic
{
	// Use this for initialization
	public override void Init()
	{
		Hp = 1;
		acs = new Vector2(0.0f, -0.001f);
		vel = new Vector2(-0.031f, -0.1f);
		vec = transform.position;
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}

	/// <summary>
	/// 地面に着いたら消える
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerStay2D(Collider2D collision)
	{
		Destroy(gameObject);
	}
}
