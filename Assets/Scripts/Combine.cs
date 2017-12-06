using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Combine : MonoBehaviour
{
    public Material targetMaterial;
    private List<CombineInstance> combines;

    private void Start()
    {
        combines = new List<CombineInstance>();
    }

    public void MeshCombine(GameObject ob)
    {
        Component meshFilters = ob.GetComponent<MeshFilter>();
        CombineInstance combine = new CombineInstance();

        combine.mesh = ((MeshFilter)meshFilters).sharedMesh;
        combine.transform = meshFilters.transform.localToWorldMatrix;
        meshFilters.gameObject.SetActive(false);

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        combines.Add(combine);
        if (combines.Count > 1000)
        {
            combines.RemoveRange(0, 800);
        }
        print(combines.Count);

        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combines.ToArray());
        transform.gameObject.SetActive(true);

        //?}?e???A???????
        transform.gameObject.GetComponent<Renderer>().material = targetMaterial;
    }
}