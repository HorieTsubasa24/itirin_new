using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Gimic {

	// Use this for initialization
	public override void Init(float height){
		Hp = 1;
		acs = new Vector2(0.0f, -0.005f);
		vel = new Vector2(-0.075f, 0.0f);
		vec = new Vector2(transform.position.x, height + 2.0f);
	}

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
	}

	/// <summary>
	/// 地面に着いたら跳ねる
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerStay2D(Collider2D collision)
	{
		vel.y = -acs.y * 28;
	}

    /// <summary>
    /// 自機に接触
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Unicycle")
            unicycle.Damage(15);
    }
}
