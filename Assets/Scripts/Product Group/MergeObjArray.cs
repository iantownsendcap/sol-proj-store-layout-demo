using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MergeObjArray : MonoBehaviour
{

    public static bool MergeThem(GameObject source, GameObject destination)
    {
        if (source == null || destination == null) return false;

        bool result = true;

        Vector3 oldpos = source.transform.position;
        Quaternion oldrot = source.transform.rotation;
        source.transform.position = Vector3.zero;
        source.transform.rotation = Quaternion.identity;

        MeshFilter[] filters = source.GetComponentsInChildren<MeshFilter>(false);
        MeshRenderer[] renderers = source.transform.GetComponentsInChildren<MeshRenderer>(false);

        MeshFilter destFilter = destination.GetComponent<MeshFilter>();
        MeshRenderer destRenderer = destination.GetComponent<MeshRenderer>();
        if (destFilter == null)
        {
            destFilter = destination.AddComponent<MeshFilter>();
        };
        if (destRenderer == null)
        {
            destRenderer = destination.AddComponent<MeshRenderer>();
        }

        destFilter.sharedMesh = null;
        destRenderer.sharedMaterial = null;

        //Debug.Log(filters.Length);

        //Mesh finalMesh = new Mesh();

        // Get a list of all the materials
        List<Material> materials = new List<Material>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == destination.transform)
            {
                continue;
            }

            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localmat in localMats)
            {
                if (!materials.Contains(localmat))
                {
                    materials.Add(localmat);
                }
            }
        }

        // Find the meshes with the material
        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach (MeshFilter filter in filters)
            {
                // filter.sharedMesh.indexFormat = IndexFormat.UInt16;

                MeshRenderer rend = filter.GetComponent<MeshRenderer>();
                if (rend == null || rend == destRenderer) { continue; }

                Material[] localMats = rend.sharedMaterials;
                for (int i = 0; i < localMats.Length; i++)
                {

                    if (localMats[i] != material)
                    {
                        continue;
                    }

                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = i;
                    ci.transform = filter.transform.localToWorldMatrix;
                    combiners.Add(ci);

                }

            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // if the mesh has too many verts
            try { mesh.CombineMeshes(combiners.ToArray(), true); }
            catch(Exception ex) { Debug.LogError($"Could not sub combine: {combiners[0]} {ex.Message}"); result = false; }
            submeshes.Add(mesh);

        }


        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // if the mesh has too many verts
        try { finalMesh.CombineMeshes(finalCombiners.ToArray(), false); }
        catch(Exception ex) { Debug.LogError($"Could not sub combine: { finalMesh } {ex.Message}"); result = false; }


        destFilter.sharedMesh = finalMesh;
        destRenderer.sharedMaterials = materials.ToArray();
        //destRenderer.sharedMaterials = renderer.sharedMaterials;
        destRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        source.transform.position = oldpos;
        source.transform.rotation = oldrot;



        //// Make all the meshes of the same filters[i].sharedMesh.subMeshCount

        //// Get the submesh with material from every filter
        //// only combine the same 


        //List<CombineInstance> combiners = new List<CombineInstance>();
        //for (int i = 0; i < filters.Length; i++)
        //{
        //    // skip the source filter
        //    if (filters[i].gameObject == source) continue;
        //    if (filters[i].sharedMesh == null) continue;

        //    Mesh m = filters[i].sharedMesh;

        //    SubMeshDescriptor subMeshDescriptor = m.GetSubMesh(0);
        //    CombineInstance ci = new CombineInstance();
        //    ci.subMeshIndex = 0;
        //    ci.transform = filters[i].transform.localToWorldMatrix;
        //    ci.mesh = new Mesh();
        //    ci.mesh.SetVertices(m.vertices);
        //    ci.mesh.SetTriangles(m.GetTriangles(0), 0);
        //    ci.mesh.SetNormals(m.normals);
        //    //ci.mesh.subMeshCount = 3;
        //    //ci.mesh.SetSubMesh(0, subMeshDescriptor);
        //    //ci.mesh.SetSubMesh(1, new SubMeshDescriptor());
        //    //ci.mesh.SetSubMesh(2, new SubMeshDescriptor());
        //    combiners.Add(ci);
        //}
        //finalMesh.CombineMeshes(combiners.ToArray(), true, true);







        //finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // if the mesh has too many verts

        //// CombineInstance[] combiners = new CombineInstance[filters.Length];

        //List<CombineInstance> combiners = new List<CombineInstance>();
        //int parts = filters[1].sharedMesh.subMeshCount;

        //for (int i = 0; i < filters.Length; i++)
        //{
        //    // skip the source filter
        //    if (filters[i].gameObject == source) continue;
        //    if (filters[i].sharedMesh == null) continue;
        //    // List<CombineInstance> combiners = new List<CombineInstance>();
        //    //for (int j = 0; j < filters[i].sharedMesh.subMeshCount; j++)
        //    //{
        //        CombineInstance ci = new CombineInstance();
        //        ci.subMeshIndex = 0;
        //        ci.mesh = filters[i].sharedMesh;
        //        ci.transform = filters[i].transform.localToWorldMatrix;

        //        combiners.Add(ci);

        //        //finalMesh.CombineMeshes(combiners.ToArray(), false, true);
        //    //}

        //}
        //finalMesh.CombineMeshes(combiners.ToArray(), true, true, false);




        //MeshFilter destFilter = destination.GetComponent<MeshFilter>();
        //MeshRenderer destRenderer = destination.GetComponent<MeshRenderer>();
        //if (destFilter == null)
        //{
        //    destFilter = destination.AddComponent<MeshFilter>();
        //    // destFilter.mesh = new Mesh();
        //};
        //if (destRenderer == null)
        //{
        //    destRenderer = destination.AddComponent<MeshRenderer>();
        //}
        //destFilter.sharedMesh = null;
        //destFilter.sharedMesh = finalMesh;
        ////destRenderer.sharedMaterials = renderer.sharedMaterials;
        //destRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        //source.transform.position = oldpos;
        //source.transform.rotation = oldrot;

        return result;
    }


}
