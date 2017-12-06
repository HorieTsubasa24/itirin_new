using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletedot : MonoBehaviour {

    int deletecount = 3;
	
	// Update is called once per frame
	void Update () {
		if (deletecount != 0)
        {
            deletecount--;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
