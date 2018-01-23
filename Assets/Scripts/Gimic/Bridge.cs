using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Gimic
{
    private float[] heights = { -0.004889976f, -0.026650367f, -0.04792176f, -0.068704156f, -0.088997555f, -0.108801956f, -0.128117359f, -0.146943765f, -0.165281174f, -0.183129584f, -0.200488998f, -0.217359413f, -0.233740831f, -0.249633252f, -0.265036675f, -0.2799511f, -0.294376528f, -0.308312958f, -0.321760391f, -0.334718826f, -0.347188264f, -0.359168704f, -0.370660147f, -0.381662592f, -0.392176039f, -0.402200489f, -0.411735941f, -0.420782396f, -0.429339853f, -0.437408313f, -0.444987775f, -0.45207824f, -0.458679707f, -0.464792176f, -0.470415648f, -0.475550122f, -0.480195599f, -0.484352078f, -0.48801956f, -0.491198044f, -0.493887531f, -0.49608802f, -0.497799511f, -0.499022005f, -0.499755501f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.499755501f, -0.499022005f, -0.497799511f, -0.49608802f, -0.493887531f, -0.491198044f, -0.48801956f, -0.484352078f, -0.480195599f, -0.475550122f, -0.470415648f, -0.464792176f, -0.458679707f, -0.45207824f, -0.444987775f, -0.437408313f, -0.429339853f, -0.420782396f, -0.411735941f, -0.402200489f, -0.392176039f, -0.381662592f, -0.370660147f, -0.359168704f, -0.347188264f, -0.334718826f, -0.321760391f, -0.308312958f, -0.294376528f, -0.2799511f, -0.265036675f, -0.249633252f, -0.233740831f, -0.217359413f, -0.200488998f, -0.183129584f, -0.165281174f, -0.146943765f, -0.128117359f, -0.108801956f };    // Use this for initialization
    public override void Init(float height)
	{
        like = Like.Gimic;
        Span = 90;
        heightline = new float[heights.Length];
        heightline = heights;
        for(int i = 0; i < heightline.Length; i++)
            heightline[i] += height;
		Hp = 1;
		vec = new Vector2(transform.position.x, height);
		var rb = GetComponent<Rigidbody2D>();
		acs = new Vector2(0.0f, 0.0f);
		vel = new Vector2(-0.14f, 0.0f);
        transform.position = vec;
    }

	public override void Move()
	{
		vec += vel;
		vel += acs;
		transform.position = vec;
		if (vec.x < -30.0f) Destroy(gameObject);
	}
}
