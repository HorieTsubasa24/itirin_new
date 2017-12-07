using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : Gimic {
	public GameObject prefab_Droppings;
	public readonly int DroppingSpan = 120;
	public readonly int DroppingSpanBase = 40;

	// Use this for initialization
	public override void Init(float height)
	{
		Hp = 1;
		acs = new Vector2(0.0f, 0.0f);
		vel = new Vector2(-0.075f, 0.0f);
		vec = new Vector2(transform.position.x, height + 4.0f);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
		ThrowDropping();
	}

	int droppingWait = -1;
	/// <summary>
	/// 鳩のフン
	/// </summary>
	private void ThrowDropping()
	{
		if (droppingWait == 0)
		{
			Instantiate(prefab_Droppings, transform.position, Quaternion.identity);
		}
		else if(droppingWait == -1)
		{
			droppingWait = rand.Next(DroppingSpan) + DroppingSpanBase;
		}
		droppingWait--;
	}

}
