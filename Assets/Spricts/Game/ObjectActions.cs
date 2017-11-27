using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectActions : MonoBehaviour {
    /// <summary>
    /// イベントのID
    /// </summary>
    public int IventId;

    /// <summary>
    /// 印
    /// </summary>
    public GameObject dot;

    /// <summary>
    /// 前回フレームの位置座標
    /// </summary>
    public Vector2 BefVecThis;

    /// <summary>
    /// 一輪車
    /// </summary>
    GameObject ob_uni;

    /// <summary>
    /// 一輪車のサイズ
    /// </summary>
    public Vector2 ob_uni_size;

    /// <summary>
    /// 一輪車のコンポーネント
    /// </summary>
    Unicycle unicycle;

    /// <summary>
    /// オブジェクトのサイズ
    /// </summary>
    public Vector2 ob_size;

	// Use this for initialization
	void Start () {
        //  一輪車のコンポーネントを取得
        unicycle = GameObject.Find("Unicycle").GetComponent<Unicycle>();
        ob_uni = GameObject.Find("Unicycle");

        BefVecThis = new Vector2(0.0f, 0.0f);

        ob_size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        ob_uni_size = ob_uni.GetComponent<SpriteRenderer>().bounds.size;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Action();
        BefVecThis = (Vector2)transform.position - GetComponent<Rigidbody2D>().velocity;
    }

    /// <summary>
    /// オブジェクトのIDごとに分岐して処理をする。
    /// </summary>
    public void Action()
    {
        switch (IventId)
        {
            // 一輪車の座標(pivot=bottom)と台の座標(pivot=lefttop)を見て、
            case 1:
                var val = CollidCheck(ob_uni, gameObject);

                // 0:非接触 1 ~ 4:上下左右
                if (val != 0)
                {
                    unicycle.isBeforeCollidDirection = unicycle.isCollidDirection;
                    unicycle.isCollidDirection = val;
                }
                else
                {
                    unicycle.isBeforeCollidDirection = 0;
                    unicycle.isCollidDirection = 0;
                }

                Table(unicycle.isCollidDirection, unicycle.isBeforeCollidDirection);

                return;
            default:
                return;
        }
    }

    /// <summary>
    /// 台のアルゴリズム
    /// </summary>
    /// <param name="val"></param>
    void Table(int val, int beforeval)
    {
        switch (val)
        {
            case 0:
                if (unicycle.isOntheObject == true)
                {
                    unicycle.jumpvec = new Vector2(0, gameObject.transform.position.y);
                    unicycle.ToFly();
                }
                break;
            case 1:
                if (unicycle.isOntheObject == true || beforeval == 1 || beforeval == 0)
                {
                    unicycle.vec = new Vector2(unicycle.vec.x, transform.position.y);
                    unicycle.Landing();
                    unicycle.isOntheObject = true;
                }
                break;
            case 2:
                if (beforeval == 0)
                {
                    unicycle.vel = new Vector2(0.0f, -0.1f);
                }
                break;
        }
    }

    /// <summary>
    /// コライダーを使わずにスクリプト上で衝突判定
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    int CollidCheck(GameObject ply, GameObject ob)
    {
        Vector2 plyvec = ply.transform.position;
        Vector2 obvec = ob.transform.position;

        // Test_CollisiondotSet(ply, ob, plyvec, obvec);

        // 矩形での判定
        if (plyvec.y <= obvec.y && 
            (plyvec.y + ob_uni_size.y) >= (obvec.y - ob_size.y) &&
            (plyvec.x >= obvec.x && 
            plyvec.x <= (obvec.x + ob_size.x)))
        {

            if ((plyvec.x > obvec.x && plyvec.x < obvec.x + ob_size.x))
            {
                
                // 上から衝突
                if (plyvec.y > obvec.y - ob_size.y / 2)
                {
                    print("top");
                    return 1;
                }
                
                // 下から衝突
                if (plyvec.y < obvec.y + ob_size.y / 2)
                {
                    print("bottom");
                    return 2;
                }

            }
            // 左から衝突
            if (unicycle.BeforeVec.x < plyvec.x)
            {
                //print("left");
                return 3;
            }
            // 右から衝突
            if (plyvec.x < unicycle.BeforeVec.x)
            {
                //print("right");
                return 4;
            }
        
            
        }
        // 接触していない
        return 0;
    }


    void Test_CollisiondotSet(GameObject ply, GameObject ob, Vector2 plyvec, Vector2 obvec)
    {
        Instantiate(dot, plyvec, Quaternion.identity);
        Instantiate(dot, obvec, Quaternion.identity);
        Instantiate(dot, new Vector2(plyvec.x, (plyvec.y + ply.GetComponent<SpriteRenderer>().bounds.size.y)), Quaternion.identity);
        Instantiate(dot, new Vector2(obvec.x, (obvec.y - ob.GetComponent<SpriteRenderer>().bounds.size.y)), Quaternion.identity);
        Instantiate(dot, new Vector2((obvec.x + ob.GetComponent<SpriteRenderer>().bounds.size.x), obvec.y), Quaternion.identity);

        
    }
}
