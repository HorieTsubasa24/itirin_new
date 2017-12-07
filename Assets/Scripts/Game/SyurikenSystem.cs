using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyurikenSystem : MonoBehaviour {
    public GameObject ob_unicycle;
    public readonly int Span = 30;
    
    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
                ThrowSyuriken();
        }
    }

    void ThrowSyuriken()
    {

    }
}
