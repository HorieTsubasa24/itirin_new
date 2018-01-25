using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDeleteOnCollision : MonoBehaviour {
    public WaveDraw wd;
    /// <summary>
    /// 侵入したドットを削除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    /// <summary>
    /// 入ってきた障害物を削除
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dot")
            Destroy(collision.gameObject);
        else if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "gimic")
           wd.GimicObjectDelete(collision.gameObject);
    }
}
