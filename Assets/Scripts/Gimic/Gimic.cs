using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gimic : MonoBehaviour {
	public GameObject ob_unicycle;
	public Unicycle unicycle;
	public int Hp;
	public Vector2 vec;
	public Vector2 vel;
	public Vector2 acs;
	public float rot;
	public int Span = 0;

	public abstract void Init();
	public abstract void Move();

	private void Start()
	{
		ob_unicycle = GameObject.Find("Unicycle");
		unicycle = ob_unicycle.GetComponent<Unicycle>();
	}
}
