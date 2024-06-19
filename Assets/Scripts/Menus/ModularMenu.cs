using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Windows;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

public class ModularMenu : MonoBehaviour
{
    Dictionary<string, List<string>> Dep_Cats = new Dictionary<string, List<string>> {
    {
        "All", new List<string>() {
        }},{
        "none", new List<string>() {
        }},{
        "Clothing", new List<string>() {
        "Shoes & Accessories","Women","Women's Plus","Men","Kids & Baby","Shoes","Jewelry & Watches","Bags & Accessories","Savings"
        }},{
        "Books", new List<string>() {
        "Shop all books","Kobo eReaders","eBooks","Children's books","Textbooks","Teen & young adult books","Magazines","Biographics & memoirs","Literature & fiction","Comics & graphic novels","Arts & entertainment","Cookbooks, food, & wine",
        }},{
        "Movies & TV Shows", new List<string>() {
        "Shop all movies & TV shows","4K Ultra HD Movies","Blu-ray Movies","DVD Movies","4K Ultra HD TV Shows","Blu-ray TV Shows","DVD TV Shows",
        }},{
        "Music & Vinyl", new List<string>() {
        "Shop all music & vinyl","Turntables","Musical instruments","Rap & hip-hop","Rock","Pop","Country","Classical",
        }},{
        "Video Games", new List<string>() {
        "Shop all video games","Xbox","PlayStation","Nintendo","Video game accessories","Virtual reality","Arcade","Collectibles",
        }},{
        "Arts", new List<string>() {
        "Crafts,Sewing & Party Supplies","Shop All Arts Crafts & Sewing","Sewing Machines & Accessories","Die Cutting","Fabric","Arts & Crafts Furniture and Storage","Craft Supplies","Art Supplies","Knitting & Crochet","Arts & Crafts for Kids","Beading & Jewelry Making","Fabric & Apparel Crafting","Artificial Plants and Flowers","Party Supplies","Gift Wrap","Wedding Shop",
        }},{
        "Auto, Tires & Industrial", new List<string>() {
        "Shop All Auto Parts","Shop All Tires","Auto Care Center","Batteries & Accessories","Oils & Fluids","OEM Parts","Performance Parts","Replacement Parts","Exterior Car Parts & Accessories","Interior Accessories","Auto Tools & Equipment","Truck Parts & Accessories","RV Parts & Accessories","Wheels & Rims","Industrial & Scientific","ATV & Off-Road","Motorcycle","Jeep Accessories + Jeep Parts",
        }},{
        "Patio & Garden", new List<string>() {
        "Patio Furniture","Outdoor Power Equipment","Grills & Outdoor Cooking","Patio & Outdoor Decor","Outdoor Shade","Garden Center","Outdoor Heating",
        }},{
        "Toys & Outdoor Play", new List<string>() {
        "Action Figures","Dolls & Dollhouses","Learning Toys","Arts & Crafts","Games & Puzzles","Collectibles","Cars, Drones & RC","LEGO & Building Sets","Pretend Play","Outdoor Play","Kids Bikes","Ride-on Toys","Video Games",
        }},{
        "Baby", new List<string>() {
        "Nursery","Car Seats","Strollers","Gear & Activity","Diapers & Wipes","Feeding & Nursing","Baby & Toddler Toys","Bath & Potty","Baby Health & Safety","Baby Clothing","Toddler Clothing",
        }},{
        "Electronics", new List<string>() {
        "TV & Video","Home Audio & Theater","Smart Home","Tablets & Accessories","Computers","Cell Phones","Wearable Tech","Cameras, Camcorders & Drones","Photo Services","Portable Audio","Auto Electronics","Video Games",
        }},{
        "Grocery", new List<string>() {
        "Fresh Produce","Meat & Seafood","Deli","Dairy & Eggs","Bread & Bakery","Frozen","Pantry","Breakfast & Cereal","Baking","Snacks","Candy","Beverages","Alcohol","Organic Foods","Gluten Free Foods","Food Gifts & Flowers Shop",
        }},{
        "Household Essentials", new List<string>() {
        "Laundry Room","Kitchen","Bathroom","Cleaning Supplies","Paper & Plastic","Air Fresheners","Batteries","Light Bulbs","Pest Control","The Clean Living Shop","As Seen on TV",
        }},{
        "Pets", new List<string>() {
        "Dog Supplies","Cat Supplies","Fish Supplies","Small Animal Supplies","Reptile Supplies","Bird Supplies","Horse Supplies","Farm Animal Supplies",
        }},{
        "Sports & Outdoors", new List<string>() {
        "Sports","Basketball","Football","Golf","Soccer","Recreation","Game Room","Outdoor Games","Trampolines","Bikes","Fitness","Outdoor Sports","Camping","Fishing","Hunting","Sports Shooting","Boats & Marine","Paddle Sports","Water Sports","Winter Sports",
        }},{
        "Stationery & Office Supplies", new List<string>() {
        "Office Supplies","School Supplies","Activity","Shipping & Moving","Safes & Lockboxes","Walmart for Business",
        }},{
        "Home",  new List<string>() {
        "Furniture & Appliances","Furniture","Mattresses & Accessories","Kitchen & Dining","Storage & Organization","Luggage & Travel","Appliances","Kitchen Appliances","Decor","Bedding","Bath","Home Savings",
        }},{
        "Beauty", new List<string>() {
        "Premium Beauty","Makeup","Skincare","Hair Care","Fragrance","Nail Care","Beauty Tech & Tools","Bath & Body","Men's Grooming","Beauty Savings","Gifts",
        }},{
        "Personal Care", new List<string>() {
        "Bath & Body","Oral Care","Shaving","Feminine Care","Men's Grooming","Incontinence","Sexual Wellness","Hair Care & Hair Tools","Sun Care & Tanning",
        }},{
        "Pharmacy",  new List<string>() {
        "Health & Wellness","Shop all Health","COVID - 19 Testing Kits","Medicine Cabinet","Pain Relievers","Allergy, Sinus & Asthma","Digestive Health","Home Health Care","Cough, Cold & Flu","Diabetes Management","Shop All Wellness","Vitamins & Supplements","Protein & Performance Nutrition","Weight Management","See All Pharmacy","Book your COVID - 19 Vaccine","COVID - 19 Digital Vaccine Record",
        }},{
        "Home Improvement", new List<string>() {
        "Heating","Cooling","Air Quality","Water Purification","Tools","Bathroom Renovation","Kitchen Renovation","Paint","Flooring","Lighting Fixtures","Light Bulbs","Fasteners","Garage Storage","Building Materials","Emergency Prep","Personal Protective Equipment","Wallpaper & Wall Coverings","Adhesives & Glues","Shower Heads",
        }},{
        "Savings & Featured Shops", new List<string>() {
        "Shop All Deals","Clearance","Rollbacks","Flash Picks","Black & Unllmited","Built for Better","Best Sellers"
        }},
    };
    public static string ProductDataPath = $"{Application.dataPath}/Data/ProductData.csv";

    public static class ProductDataSummary
    {
        public static float sales;
        public static float price;
        public static float shrink;
        public static Color minColor = new Color(1, 1, 1, 1);
        public static Color maxColor = new Color(1, 0, 0, 1);

        internal static void Reset()
        {
            price = 0;
            sales = 0;
            shrink = 0;
        }
    }

    public class ProductDataLine
    {
        public string filename;
        public string path;
        public string UPC;
        public string CATEGORY;
        public Departments DEPARTMENT = Departments.none;
        public List<string> KEYWORDS = new List<string>();
        public UnityEngine.Object obj;
        public Vector3 dimensions = new Vector3();
        public int lineIndex = -1;
        public float shrink;
        public float price;
        public float sales;

        public ProductDataLine() { }
        public ProductDataLine(string ln)
        {
            string[] data = ln.Split(";");
            this.filename = data[0];
            this.path = data[1];
            this.UPC = data[2];
            this.CATEGORY = data[3];
            Enum.TryParse<Departments>(data[4], true, out this.DEPARTMENT);
            this.KEYWORDS = new List<string>(data[5].Split(","));
            this.dimensions = StringToVector3(data[6]);
            this.shrink = float.Parse(data[7]);
            this.price = float.Parse(data[8]); // .ToString("c2")
            this.sales = float.Parse(data[9]);

            if (ProductDataSummary.sales < this.sales) ProductDataSummary.sales = this.sales;
            if (ProductDataSummary.shrink < this.shrink) ProductDataSummary.shrink = this.shrink;
            if (ProductDataSummary.price < this.price) ProductDataSummary.price = this.price;
        }
        public string GetLine()
        {
            return $"{Environment.NewLine}{this.filename};{this.path};{this.UPC};{this.CATEGORY};{this.DEPARTMENT};{string.Join(",", this.KEYWORDS)};{this.dimensions};{this.shrink};{this.price};{this.sales};";
        }
    }

    public enum Departments
    {
        All,
        none,
        Clothing,
        Books,
        Movies_and_TV_Shows,
        Music_and_Vinyl,
        Video_Games,
        Arts,
        Auto,
        Tires_and_Industrial,
        Patio_and_Garden,
        Toys_and_Outdoor_Play,
        Baby,
        Electronics,
        Grocery,
        Household_Essentials,
        Pets,
        Sports_and_Outdoors,
        Stationery_and_Office_Supplies,
        Home,
        Beauty,
        Personal_Care,
        Pharmacy,
        Home_Improvement,
        Savings_and_Featured_Shops,
        Alcohol,
    };

    public static Vector3 StringToVector3(string sVector)
    {
        if (sVector == null || sVector == "") return Vector3.zero;

        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
    public static string Vector3ToString(Vector3 sVector)
    {
        return $"({sVector.x},{sVector.y},{sVector.z})";
    }


    //[MenuItem("MyHelperTools/Modulars/Open Modular File", false, 10)]
    //static void OpenModularFile()
    //{
    //    const float inch_to_meter = 0.0254f;

    //    string[] ModShader_Segments = AssetDatabase.FindAssets("ModShader_Segment t:Material");
    //    string pth = AssetDatabase.GUIDToAssetPath(ModShader_Segments[0]);
    //    Material ModShader_Segment = AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material;

    //    string[] ModShader_Positions = AssetDatabase.FindAssets("ModShader_Position t:Material");
    //    pth = AssetDatabase.GUIDToAssetPath(ModShader_Positions[0]);
    //    Material ModShader_Position = AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material;

    //    string[] ModShader_Products = AssetDatabase.FindAssets("ModShader_Position t:Material");
    //    pth = AssetDatabase.GUIDToAssetPath(ModShader_Products[0]);
    //    Material ModShader_Product = AssetDatabase.LoadAssetAtPath(pth, typeof(Material)) as Material;

    //    string[] ProductGroups = AssetDatabase.FindAssets("ProductGroup t:Prefab");
    //    pth = AssetDatabase.GUIDToAssetPath(ProductGroups[0]);
    //    GameObject ProductGroup = AssetDatabase.LoadAssetAtPath(pth, typeof(GameObject)) as GameObject;

    //    string psa_path = EditorUtility.OpenFilePanelWithFilters("PSA Planogram File", "", new string[] { "Planogram Files", "psa" });
    //    PSA p = new PSA(psa_path);

    //    GameObject go_mod = new GameObject();
    //    go_mod.name = "Modular";
    //    // go_mod.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

    //    foreach (Segment seg in p.Segments)
    //    {
    //        // Make the segment
    //        GameObject go_seg = new GameObject();
    //        go_seg.name = $"Segment_{Mathf.Round(seg.offset_x)}";
    //        go_seg.transform.SetParent(go_mod.transform);
    //        go_seg.transform.position = new Vector3(seg.offset_x, 0, 0) * inch_to_meter;

    //        // Make a group to hold all the positions
    //        // place it in the back of the mod
    //        // mods are placed in the front left but positions
    //        // are set from back to front
    //        GameObject go_segPositionNull = new GameObject();
    //        go_segPositionNull.name = "Segment_PositionGrp";
    //        go_segPositionNull.transform.SetParent(go_seg.transform);
    //        go_segPositionNull.transform.localPosition = new Vector3(0, 0, p.plno.Depth) * inch_to_meter;
    //        //go_segPositionNull.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

    //        // The null group that contains the segment mesh
    //        GameObject go_segMeshNull = new GameObject();
    //        go_segMeshNull.name = "Segment_MeshGrp";
    //        go_segMeshNull.transform.SetParent(go_seg.transform);
    //        go_segMeshNull.transform.localScale = new Vector3(seg.width, p.highest, p.plno.Depth) * inch_to_meter;
    //        go_segMeshNull.transform.localPosition = Vector3.zero;

    //        // The segment mesh
    //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //        cube.name = "SegmentMesh";
    //        AssignMaterial(cube, ModShader_Segment);
    //        cube.transform.SetParent(go_segMeshNull.transform);
    //        cube.transform.localPosition = Vector3.one * 0.5f;
    //        cube.transform.localScale = Vector3.one;
    //        cube.AddComponent<HiddenOnPlay>();


    //        foreach (Position po in seg.positions)
    //        {
    //            // The Position group
    //            GameObject go_pos = new GameObject();
    //            go_pos.name = $"Position_{po.UPC}";
    //            go_pos.transform.SetParent(go_segPositionNull.transform);
    //            go_pos.transform.localRotation = Quaternion.identity;
    //            go_pos.transform.position = new Vector3(po.X, po.Y, p.plno.Depth - po.Z) * inch_to_meter;

    //            //// The null group that contains the position mesh
    //            //GameObject go_posMeshGrp = new GameObject();
    //            //go_posMeshGrp.name = $"Position_MeshGrp";
    //            //go_posMeshGrp.transform.SetParent(go_pos.transform);
    //            //go_posMeshGrp.transform.localPosition = Vector3.zero;
    //            //go_posMeshGrp.transform.localScale = new Vector3(po.Width, po.Height, po.Depth) * inch_to_meter;

    //            //// The position mesh
    //            //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //            //cube.name = $"PositionMesh_{po.UPC}";
    //            //AssignMaterial(cube, ModShader_Position);
    //            //cube.transform.SetParent(go_posMeshGrp.transform);
    //            //cube.transform.localPosition = new Vector3(1, 1, -1) * 0.5f;
    //            //cube.transform.localScale = Vector3.one;

    //            //po.HFacings
    //            GameObject prod = PrefabUtility.InstantiatePrefab(ProductGroup) as GameObject;
    //            //GameObject prod = GameObject.Instantiate<GameObject>(ProductGroup);
    //            prod.transform.SetParent(go_pos.transform);
    //            prod.transform.localPosition = new Vector3(0, 0, -po.Depth) * inch_to_meter;
    //            prod.GetComponent<DragVolume>().SetSize(new Vector3(po.Width, po.Height, po.Depth) * inch_to_meter);

    //        }

    //    }
    //}

    static void AssignMaterial(GameObject go, Material mat)
    {
        Renderer rend = go.GetComponent<Renderer>();
        rend.material = mat;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    [MenuItem("MyHelperTools/Modulars/Write Product Data", false, 10)]
    static void WriteProductData()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"#filename;path;UPC;CATEGORY;DEPARTMENT;KEYWORDS;dimensions;shrink;price;sales");
        System.Random rnd = new System.Random();
        //string[] prefab_paths = AssetDatabase.FindAssets("t:Prefab");
        string[] allpaths = AssetDatabase.FindAssets("t:GameObject");
        List<string> filteredpaths = new List<string>();
        // long g = 111111111111;



        foreach (string pth in allpaths)
        {
            string p = AssetDatabase.GUIDToAssetPath(pth);
            string ext = Path.GetExtension(p);
            if (!p.Contains("/Products/") || !(ext == ".fbx" || ext == ".prefab")) continue;
            filteredpaths.Add(p);
        }

        foreach (string p in filteredpaths)
        {
            string fn = Path.GetFileNameWithoutExtension(p);
            string justname = fn;
            if (justname.Contains("_"))
            {
                justname = justname.Split("_")[0].ToLower();
            }

            Departments dept = Departments.none;

            if (justname == "soap" || justname == "bleach" || justname == "cleanser") dept = Departments.Household_Essentials;

            string UPC = "";
            for (int i = 0; i < 12; i++)
            {
                UPC += $"{UnityEngine.Random.Range(0, 9)}";
            }

            GameObject product = AssetDatabase.LoadAssetAtPath(p, typeof(GameObject)) as GameObject;
            if (product == null) continue;
            Renderer rend = product.GetComponent<Renderer>();
            if (rend == null || rend.bounds == null) continue;
            Vector3 dims = rend.bounds.size;
            if (Vector3.Distance(dims, Vector3.zero) < 2) { dims *= 100f; }

            ProductDataLine pdl = new ProductDataLine()
            {
                filename = fn,
                path = p,
                UPC = UPC,
                KEYWORDS = new List<string>() { justname },
                CATEGORY = UnityEngine.Random.Range(24, 99).ToString(),
                DEPARTMENT = dept,
                dimensions = dims,
                shrink = float.Parse(UnityEngine.Random.Range(0f, 1000f).ToString("0.0000")),
                price = float.Parse(UnityEngine.Random.Range(3f, 40f).ToString("0.00")),
                sales = float.Parse(UnityEngine.Random.Range(0f, 10000f).ToString("0.0000"))
            };

            sb.Append(pdl.GetLine());
        }

        //string fiout = EditorUtility.SaveFilePanel("Product Data Write", "", "ProductData", "csv");

        //string fiout = EditorUtility.OpenFilePanelWithFilters("PSA Planogram File", "", new string[] { "Planogram Files", "psa" });

        File.WriteAllText(ProductDataPath, sb.ToString());

        Debug.Log("COMPLETE");
    }


    [MenuItem("MyHelperTools/Modulars/Update Bounds", false, 10)]
    static void UpdateBoundsMenu()
    {
        UpdateBounds(Selection.objects);
    }
    public static void UpdateBounds(UnityEngine.Object[] objs)
    {
        foreach (UnityEngine.Object obj in objs)
        {
            ProductArray pa = (obj as GameObject).GetComponent<ProductArray>();
            if (pa != null)
            {
                ProductDataLine pdl = GetProductDataByName(pa.Product.name);
                if (pdl == null) continue;
                float pad = 1f;
                pa.Space = pdl.dimensions + new Vector3(pad, 0, pad);
                Vector3 f = pdl.dimensions / 2;
                f.y = pa.Offset.y;
                pa.Offset = f;
                // pa.refresh = true;
            }
        }
    }


    [MenuItem("MyHelperTools/Modulars/Attach Modular", false, 10)]
    public static void AttachModualar()
    {
        if (Selection.objects.Length != 2) return;

        GameObject mod = (Selection.objects[0] as GameObject);
        GameObject fixture = (Selection.objects[1] as GameObject);

        MyModular mymod = mod.GetComponent<MyModular>();
        if (mymod == null)
        {
            GameObject tmp = mod;
            mod = fixture;
            fixture = tmp;

            mymod = mod.GetComponent<MyModular>();
        }

        mymod.AlignOffset(fixture.transform);
        mymod.Connection = fixture.transform;
    }

    [MenuItem("MyHelperTools/Modulars/Detach Modular", false, 10)]
    public static void DetachModualar()
    {
        if (Selection.objects.Length != 1) return;
        GameObject mod = (Selection.objects[0] as GameObject);
        MyModular mymod = mod.GetComponent<MyModular>();
        mymod.Connection = null;
        mymod.ResetOffset();
    }

    // ---

    [MenuItem("MyHelperTools/Modulars/Bake Modular", false, 10)]
    public static void BakeModualar()
    {
        if (Selection.objects.Length != 1) return;
        GameObject mod = (Selection.objects[0] as GameObject);
        MyModular mymod = mod.GetComponent<MyModular>();

        GameObject BakedMod = new GameObject();
        BakedMod.transform.position = mod.transform.position;
        BakedMod.transform.rotation = mod.transform.rotation;

        List<GameObject> children = new List<GameObject>();

        ShelfGroup[] shelves = mod.GetComponentsInChildren<ShelfGroup>();
        foreach (ShelfGroup shelfg in shelves)
        {
            for (int i = 0; i < shelfg.ProductGroups.Count; i++)
            {
                DragVolume dv = shelfg.ProductGroups[i].GetComponent<DragVolume>();
                ProductArray pa = shelfg.ProductGroups[i].GetComponent<ProductArray>();

                if (shelfg.HasShelf)
                {
                    GameObject newshelfGo = GameObject.Instantiate(shelfg.shelf);
                    newshelfGo.name = shelfg.shelf.name;

                    newshelfGo.transform.position = shelfg.shelf.transform.position;
                    newshelfGo.transform.rotation = shelfg.shelf.transform.rotation;
                    newshelfGo.transform.localScale = shelfg.shelf.transform.localScale;

                    newshelfGo.transform.SetParent(BakedMod.transform);

                }

                if (dv != null)
                {
                    Transform tr = dv.transform.Find("DynamicGroup");
                    if (tr == null || !tr.gameObject.activeSelf) continue;

                    GameObject newGo = GameObject.Instantiate(tr.gameObject);
                    newGo.name = pa.Product.name;
                    newGo.transform.position = tr.position;
                    newGo.transform.rotation = tr.rotation;
                    newGo.transform.SetParent(BakedMod.transform);

                }
            }
        }


        mod.SetActive(false);
    }


    //static ProductDataLine GetProductDataLineFromSelectedObject(GameObject obj)
    //{
    //    Transform prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj.transform);
    //    if (prefab == null)
    //    {
    //        prefab = PrefabUtility.GetCorrespondingObjectFromSource((Selection.objects[0] as GameObject).transform);
    //    }
    //    if (prefab != null)
    //    {
    //        ModularMenu.AllProductDataLines = null;
    //        return GetProductDataByName(prefab.name);
    //    }
    //    return null;
    //}

    // [MenuItem("MyHelperTools/Modulars/Open Modular File", false, 10)]
    //static void 


    public static Dictionary<string, ProductDataLine> AllProductDataLines;
    static public ProductDataLine GetProductDataByName(string objectName)
    {
        if (AllProductDataLines == null) AllProductDataLines = MakeObjectsFromLines(File.ReadAllLines(ProductDataPath));
        if (AllProductDataLines.ContainsKey(objectName)) return AllProductDataLines[objectName];
        return null;
    }

    static public bool UpdateProductData(ProductDataLine pdl)
    {
        string[] lines = File.ReadAllLines(ProductDataPath);
        lines[pdl.lineIndex] = pdl.GetLine();

        File.WriteAllLines(ProductDataPath, lines);

        return true;
    }


    // Read all the lines of data
    // convert them all to classes to check the attribute
    // send back a list of objects

    // don't have this read from the data so that it can be given custom sets
    static public Dictionary<string, ProductDataLine> MakeObjectsFromLines(string[] lines)
    {
        Dictionary<string, ProductDataLine> p = new Dictionary<string, ProductDataLine>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "" || lines[i].StartsWith("#")) continue;
            ProductDataLine pdl = new ProductDataLine(lines[i]);
            pdl.lineIndex = i;
            if (!p.ContainsKey(pdl.filename)) p.Add(pdl.filename, pdl);
        }

        return p;
    }

    static public ProductDataLine[] FindProdDataLines()
    {
        // Application.dataPath
        // 
        //string datafile = EditorUtility.OpenFilePanel("Open the Data", "", "csv");

        ProductDataSummary.Reset();

        string[] lines = File.ReadAllLines(ProductDataPath);

        Dictionary<string, ProductDataLine> ProdDataLines = MakeObjectsFromLines(lines);

        List<ProductDataLine> Found = new List<ProductDataLine>();

        List<string> keywords = new List<string>(ProductSearchWindow.productSearch.Keywords.Split(","));
        
        foreach (ProductDataLine pl in ProdDataLines.Values)
        {
            bool upc = ProductSearchWindow.productSearch.UPC == "" || pl.UPC == ProductSearchWindow.productSearch.UPC;
            bool dept = ProductSearchWindow.productSearch.Department == Departments.All || pl.DEPARTMENT == ProductSearchWindow.productSearch.Department;
            bool keyword = false;

            foreach (string key in keywords)
            {
                if ((ProductSearchWindow.productSearch.Keywords.Length == 0 && key == "") || pl.KEYWORDS.Any(s => s.Contains(key.ToLower())))
                {
                    keyword = true;
                    break;
                }
            }
                        
            if (upc && dept && keyword) Found.Add(pl);
        }


        // 

        return Found.ToArray();
    }

    static public UnityEngine.Object[] MatchingObjectsFromLines(ProductDataLine[] prods)
    {
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        foreach (ProductDataLine pl in prods)
        {
            GameObject product = AssetDatabase.LoadAssetAtPath(pl.path, typeof(GameObject)) as GameObject;
            objs.Add(product);
        }
        return objs.ToArray();
    }

}
