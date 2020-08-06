using UnityEngine;

namespace XREngine.Core.Scripts.Common
{
    public static class CombineMeshes
    {
        public static GameObject Combine(GameObject objectToCombine, Transform parent, Material material)
        {
            var meshFilters = objectToCombine.GetComponentsInChildren<MeshFilter>();
            var combine = new CombineInstance[meshFilters.Length];

            var pTransform = parent.worldToLocalMatrix;

            var i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = pTransform * meshFilters[i].transform.localToWorldMatrix;
                i++;
            }

            var combinedMesh = new GameObject("Combined Mesh");
            combinedMesh.AddComponent<MeshFilter>();
            combinedMesh.AddComponent<MeshRenderer>();
            
            combinedMesh.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            combinedMesh.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            combinedMesh.transform.GetComponent<MeshRenderer>().material = material;
            combinedMesh.transform.gameObject.SetActive(true);

            return combinedMesh;
        }
    }
}


