using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    MeshFilter[] meshFilters;
    CombineInstance[] combine;

    void OnEnable()
    {
        meshFilters = GetComponentsInChildren<MeshFilter>();
        combine = new CombineInstance[meshFilters.Length];
        StartCoroutine(CombineMesh());
    }

    private IEnumerator CombineMesh()
    {
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            combine[i].subMeshIndex = 0;
            if (meshFilters[i].gameObject != gameObject)
            {
                meshFilters[i].gameObject.active = false; 
            }
            i++;
           
            yield return new WaitForEndOfFrame();
        }
        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;
        transform.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        //transform.gameObject.active = true;
    }
}