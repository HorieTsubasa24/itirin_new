using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gimic : MonoBehaviour {
	protected System.Random rand;
	public float[] heightline;
    public WaveDraw wd;
	public GameObject ob_unicycle;
	public Unicycle unicycle;
	public int Hp;
	public Vector2 vec;
	public Vector2 vel;
	public Vector2 acs;
	public float rot;
	public int Span = 0;
    public Like like;
    public enum Like
    { Player, Enemy, Gimic}

	public abstract void Init(float height);
	public abstract void Move();

	private void Start()
	{
        Hp = 1;
		ob_unicycle = GameObject.Find("Unicycle");
        wd = GameObject.Find("SplineWitter").GetComponent<WaveDraw>();
        unicycle = ob_unicycle.GetComponent<Unicycle>();
	}

    public void Update()
    {
        if (Hp <= 0)
            wd.GimicObjectDelete(gameObject, this);
    }
}
