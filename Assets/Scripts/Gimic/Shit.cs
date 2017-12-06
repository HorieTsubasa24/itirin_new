using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
	public int Hp = 1;
	public Vector2 vec;

	// Use this for initialization
	private void Start()
	{
		Hp = 1;
		vec = transform.position;
	}

	/// <summary>
	/// 自機に接触
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerStay2D(Collider2D collision)
	{
		Destroy(gameObject);
	}
}
