using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyurikenSystem : MonoBehaviour
{
    public GameObject ob_unicycle;
    public Unicycle unicycle;
    public GameObject prefab_Syuriken;
    public readonly int Span = 30;
    
    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
                ThrowSyuriken(touch.position);
        }
    }

    void ThrowSyuriken(Vector2 pos)
    {
        Vector2 vec1 = ob_unicycle.transform.position;
        Vector2 vec2 = TouchPosToGlobalPos(pos);
        vec1.y += 1.22f;

        var ob = Instantiate(prefab_Syuriken, vec1, Quaternion.identity);
        ob.GetComponent<Syuriken>().Init(unicycle, vec1, vec2);
    }

    // 1920, 1080 ->(/108) 8.88, 5.00
    Vector2 TouchPosToGlobalPos(Vector2 pos)
    {
        return new Vector2((pos.x - 960.0f) / 108.0f, (pos.y - 540.0f) / 108.0f);
    }
}
