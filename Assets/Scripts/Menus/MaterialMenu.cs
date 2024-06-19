using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]

public class MaterialMenu : MonoBehaviour
{
    //You can see this under GameObject/UI
    //Grouped together with the UI components
    [MenuItem("MyHelperTools/Materials/Make Material From Textures", false, 10)]
    static Object MakeMaterialFromTextures()
    {

        Material mat = null;
        Object matObj = null;

        foreach (Object obj in Selection.objects)
        {
            if (obj is Material)
            {
                mat = obj as Material;
                matObj = obj;
            }
        }


        Dictionary<string, string> ChannelReplace = new Dictionary<string, string>(){
            {"_a", "_BaseColorMap" },
            {"_e", "_EmissiveColorMap" },
            {"_n", "_NormalMap" },
            {"_o", "_MaskMap" },
            //{"_m", "_MaskMap" },
        };


        // BoxPizza_ehb1pe_Var02
        // Pizza_ehb1pe_a_Var02


        bool named = false;
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D)
            {
                string channel = obj.name;

                string ns = System.Text.RegularExpressions.Regex.Replace(channel, "_Var[0-9][0-9]", "");

                if (channel != ns)
                {
                    channel = channel.Substring(0, channel.Length - 6);
                }

                channel = channel.Substring(channel.Length - 2);

                if (!named)
                {
                    if (mat == null)
                    {
                        // get the full path
                        string assetPath = AssetDatabase.GetAssetPath(obj); // matObj
                        string dir = Path.GetDirectoryName(assetPath);
                        string matFullPath = dir + "\\" + obj.name.Replace(channel, "") + ".mat";
                        mat = new Material(Shader.Find("HDRP/Lit"));
                        if (mat == null) return null;
                        AssetDatabase.CreateAsset(mat, matFullPath);
                        matObj = AssetDatabase.LoadAssetAtPath(matFullPath, typeof(Material));
                    }
                    else
                    {
                        string assetPath = AssetDatabase.GetAssetPath(mat); // matObj
                        AssetDatabase.RenameAsset(assetPath, obj.name.Replace(channel, ""));
                        AssetDatabase.Refresh();
                    }

                    named = true;
                }

                if (ChannelReplace.ContainsKey(channel)) { mat.SetTexture(ChannelReplace[channel], obj as Texture2D); }

                if (channel == "_e")
                {
                    UnityEngine.Rendering.HighDefinition.HDMaterial.SetUseEmissiveIntensity(mat, true);
                    UnityEngine.Rendering.HighDefinition.HDMaterial.SetEmissiveColor(mat, Color.white);
                    UnityEngine.Rendering.HighDefinition.HDMaterial.SetEmissiveIntensity(
                        mat,
                        14f,
                        UnityEditor.Rendering.HighDefinition.EmissiveIntensityUnit.EV100);
                }
            }
        }

        return matObj;

    }


    static string varnum(string name)
    {
        string varnum = "";
        if (name.ToLower().Contains("_var"))
        {
            varnum = name.Substring(name.Length - 5);
        }
        return varnum;
    }


    [MenuItem("MyHelperTools/Materials/Use Materials One level up", false, 10)]
    static void MaterialUpTextures()
    {

        // find all the materials
        // get the folder and go up one
        // get all the textures in that folder

        foreach (Object obj in Selection.objects)
        {

            if (obj.GetType() != typeof(Material))
            {
                continue;
            }

            string matpth = AssetDatabase.GetAssetPath(obj);
            string fn = Path.GetFileNameWithoutExtension(matpth);
            Material mat = obj as Material;

            string parpath = Directory.GetParent(Path.GetDirectoryName(matpth)).FullName;

            List<Object> texs = new List<Object>();
            texs.Add(obj);

            string[] files = Directory.GetFiles(parpath, "*.*", SearchOption.TopDirectoryOnly);

            // get the var number to compare it to the texture
            string matvar = varnum(fn);

            foreach (string f in files)
            {
                string g = f.Substring(f.IndexOf("\\Assets\\") + 1);
                Object tex = AssetDatabase.LoadAssetAtPath(g, typeof(Texture2D)) as Object;

                if (tex == null) continue;
                string texfn = Path.GetFileNameWithoutExtension(f);

                // the material and texture need to match the Var num
                // get the var num of the texture
                string texvar = varnum(texfn);

                // I can't select two different _a's
                // the var might be the last one because of sorting

                if (texvar == matvar && texvar != "")
                {
                    texs.Add(tex);
                    continue;
                }
                // if there is no texture with a Var then use the one without it
                if (texvar == "")
                {
                    texs.Add(tex);
                }

            }
            // the makematerial will disregard the var anyway

            Selection.objects = texs.ToArray();
            MakeMaterialFromTextures();

            AssetDatabase.SaveAssets();

            Selection.activeObject = mat;
        }
    }


    static List<Material> skippedmats = new List<Material>();
    [MenuItem("MyHelperTools/Materials/Find Materials without Main Texture", false, 10)]
    static void FindMaterialswithoutMainTexture()
    {
        // string[] allAssets = Directory.GetFiles(@"C:\Users\ian.townsend\Desktop\Unity\Demo_ModernSupermarket_HDRP\Assets\Resources\ModernSupermarket", "*.*", SearchOption.AllDirectories);

        string[] allAssets = AssetDatabase.FindAssets("t:Material");
        Debug.Log(allAssets.Length);

        foreach (string _pth in allAssets)
        {
            string pth = AssetDatabase.GUIDToAssetPath(_pth);
            if (!pth.Contains("Props/Products")) continue;
            if (!pth.Contains("/Textures")) continue;
            // 
            Material mat = AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material;
            if (skippedmats.Contains(mat)) continue;
            if (mat.GetTexture("_BaseColorMap") == null)
            {
                Selection.objects = new Object[] { mat };
                skippedmats.Add(mat);
                break;
            }
            //}
        }
    }


    [MenuItem("MyHelperTools/Materials/Run Automated Process", false, 10)]
    static void RunAutomatedProcess()
    {
        // string[] allAssets = Directory.GetFiles(@"C:\Users\ian.townsend\Desktop\Unity\Demo_ModernSupermarket_HDRP\Assets\Resources\ModernSupermarket", "*.*", SearchOption.AllDirectories);

        List<Material> selmats = new List<Material>();

        foreach (Object obj in Selection.objects)
        {
            if (obj.GetType() == typeof(Material))
            {
                selmats.Add(obj as Material);
            }
        }

        string[] allAssets = AssetDatabase.FindAssets("t:Object");
        Debug.Log(allAssets.Length);

        Dictionary<string, List<Texture2D>> nm_tex = new Dictionary<string, List<Texture2D>>();
        Dictionary<string, List<Material>> nm_mat = new Dictionary<string, List<Material>>();

        foreach (string _pth in allAssets)
        {
            string pth = AssetDatabase.GUIDToAssetPath(_pth);

            string fn = Path.GetFileNameWithoutExtension(pth.ToLower());

            //if (pth.Contains("Checkout_4295b5")) {

            if (Path.GetExtension(pth) == ".png")
            {
                if (fn.Length - 2 == fn.LastIndexOf("_"))
                {
                    fn = fn.Substring(0, fn.Length - 2);
                }

                if (!nm_tex.ContainsKey(fn))
                {
                    nm_tex.Add(fn, new List<Texture2D>());
                }
                nm_tex[fn].Add(AssetDatabase.LoadAssetAtPath(pth, typeof(Texture2D)) as Texture2D);
            }

            if (Path.GetExtension(pth) == ".mat")
            {
                if (!nm_mat.ContainsKey(fn))
                {
                    nm_mat.Add(fn, new List<Material>());
                }
                nm_mat[fn].Add(AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material);
            }
            //}

        }

        // if there were materials selected overwrite the material list
        // with the selected ones
        if (selmats.Count > 0)
        {
            nm_mat.Clear();
            foreach (Material mat in selmats)
            {
                nm_mat[mat.name.ToLower()] = new List<Material> { mat };
            }
        }


        foreach (string key in nm_mat.Keys)
        {
            Debug.Log(key);
            foreach (Material mat in nm_mat[key])
            {
                Debug.Log(mat.name);

                if (mat.shader.name != "HDRP/Lit") mat.shader = Shader.Find("HDRP/Lit");

                if (!nm_tex.ContainsKey(key)) continue;

                Texture2D[] texs = nm_tex[key].ToArray();

                List<Object> sels = new List<Object>();
                sels.AddRange(texs);
                sels.Add(mat);

                Selection.objects = sels.ToArray();
                Object newmat = MakeMaterialFromTextures();
            }
        }

        AssetDatabase.SaveAssets();

    }


    [MenuItem("MyHelperTools/Materials/Lower Smooth", false, 10)]
    static void LowerSmooth()
    {
        string[] allAssets = AssetDatabase.FindAssets("t:Object");
        Debug.Log(allAssets.Length);

        foreach (string _pth in allAssets)
        {
            string pth = AssetDatabase.GUIDToAssetPath(_pth);

            string fn = Path.GetFileNameWithoutExtension(pth.ToLower());

            if (Path.GetExtension(pth) != ".mat")
            {
                continue;
            }

            // if (fn != "Avocado_pd302k".ToLower()) continue;

            Material mat = AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material;
            Debug.Log(mat.name);

            //UnityEngine.Rendering.HighDefinition.HDMaterial

            mat.SetFloat("_MetallicRemapMin", 0);
            mat.SetFloat("_MetallicRemapMax", 0);

            mat.SetFloat("_SmoothnessRemapMin", 0);
            mat.SetFloat("_SmoothnessRemapMax", 0.5f);
            mat.SetFloat("_Smoothness", 0.5f);

            if (mat.name.ToLower().Contains("label"))
            {
                mat.SetFloat("_SmoothnessRemapMin", 0);
                mat.SetFloat("_SmoothnessRemapMax", 0.15f);
                mat.SetFloat("_Smoothness", 0.15f);
                continue;
            }
            if (mat.name.ToLower().Contains("plastic"))
            {
                mat.SetFloat("_SmoothnessRemapMin", 0);
                mat.SetFloat("_SmoothnessRemapMax", 0.55f);
                mat.SetFloat("_Smoothness", 0.55f);
                continue;
            }
            if (mat.name.ToLower().Contains("glass"))
            {
                mat.SetFloat("_SmoothnessRemapMin", 0);
                mat.SetFloat("_SmoothnessRemapMax", 0.85f);
                mat.SetFloat("_Smoothness", 0.85f);
                continue;
            }


            // _SmoothnessRemapMin("_SmoothnessRemapMin", Float) = 0.0
            // _SmoothnessRemapMax("_SmoothnessRemapMax", Float) = 1.0

        }

        AssetDatabase.SaveAssets();
    }


    class GoName
    {
        public string justname;
        public string varnum;
        public string original;

        public GoName(string goname)
        {
            int var_ind = goname.ToLower().IndexOf("var");
            original = goname;
            justname = goname.ToLower();
            varnum = "";
            if (var_ind > -1)
            {
                justname = justname.Substring(0, var_ind);
                if (justname.EndsWith("_"))
                {
                    justname = justname.Substring(0, justname.Length - 1);
                }
                varnum = goname.Replace(justname, "");
                if (varnum.StartsWith("_"))
                {
                    varnum = varnum.Substring(1, varnum.Length - 1);
                }
            }
        }
    }

    [MenuItem("MyHelperTools/Materials/Auto ImportSettings Material", false, 10)]
    static void AutoImportSettingsMaterial()
    {

        // UnityEditor.AssetImporters.MaterialDescription
        GameObject go = (Selection.objects[0] as GameObject);
        GoName gnObj = new GoName(go.name);

        string p = AssetDatabase.GetAssetPath(go);

        Renderer rend = go.GetComponent<Renderer>();

        Debug.Log(rend.sharedMaterial);

        string model_dir = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(p));

        //string model_dir = p.Substring(0, p.LastIndexOf("/"));
        string mat_dir = $"{model_dir}\\Textures\\Materials";

        //string[] mats = AssetDatabase.FindAssets(mat_dir);
        string[] mats = System.IO.Directory.GetFiles(mat_dir, "*.mat", SearchOption.TopDirectoryOnly);

        int g = 0;
        // put the materials into buckets
        for (int i = 0; i < mats.Length; i++)
        {
            // Debug.Log(mats[i]);

            // check the names

            // rend.sharedMaterials.Length

            // glass, main, liquid

            string matname = System.IO.Path.GetFileNameWithoutExtension(mats[i]);
            GoName mn = new GoName(matname);

            if (mn.justname.StartsWith(gnObj.justname))
            {
                if (mn.varnum == gnObj.varnum)
                {
                    rend.sharedMaterials[g] = AssetDatabase.LoadAssetAtPath(mats[i], typeof(Material)) as Material;
                    g++;
                } else
                {
                    rend.sharedMaterials[g] = AssetDatabase.LoadAssetAtPath(mats[i], typeof(Material)) as Material;
                    g++;
                }
            }

        }

    }
}
