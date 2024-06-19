//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ProductArrayProperties : ScriptableObject
//{
//    public GameObject Product;

//    public Vector3Int Grid;
//    [SerializeField]
//    public bool refresh = false;

//    public int MaxProduct = 100;
//    public int NumberOfProducts;

//    [Header("Transform")]
//    public Vector3 Space = new Vector3(6, 18, 8);
//    public Vector3 Offset = new Vector3(3.6f, 0, 4.11f);
//    //[Range(0, 360)]
//    //public float Rotate;
//    public Vector3 direction = new Vector3(-1, 1, 1);
//    public Quaternion Rotation = Quaternion.Euler(0, 180, 0);

//    [Header("Random")]
//    //[Range(0, 180)]
//    //public float RotateRandom = 8;
//    public Quaternion RotationRandom = Quaternion.Euler(0, 8, 0);
//    public Vector3 PositionRandom = new Vector3(0.05f, 0, 0.05f);
//    public int seed;

//    GameObject[] items;

//    public int ShelfNumber;

//    const float meter_to_inch = 39.3701f;
//    const float inch_to_meter = 0.0254f;

//    public void CopySettings(ProductArray pa)
//    {
//        this.Product = pa.Product;
//        this.Grid = pa.Grid;
//        this.MaxProduct = pa.MaxProduct;
//        this.direction = pa.direction;
//        // this.name = pa.name;
//        // this.NumberOfProducts = pa.NumberOfProducts;
//        this.Offset = pa.Offset;
//        this.PositionRandom = pa.PositionRandom;
//        this.Rotation = pa.Rotation;
//        this.RotationRandom = pa.RotationRandom;
//        this.seed = pa.seed;
//        // this.ShelfNumber = pa.ShelfNumber;
//        this.Space = pa.Space;
//    }
//    public bool CompareSettings(ProductArray pa)
//    {
//        if (!this.Product.Equals(pa.Product)) return false;
//        if (!this.Grid.Equals(pa.Grid)) return false;
//        if (!this.MaxProduct.Equals(pa.MaxProduct)) return false;
//        if (!this.direction.Equals(pa.direction)) return false;
//        //if (!this.name.Equals(pa.name)) return false;
//        //if (!this.NumberOfProducts.Equals(pa.NumberOfProducts)) return false;
//        if (!this.Offset.Equals(pa.Offset)) return false;
//        if (!this.PositionRandom.Equals(pa.PositionRandom)) return false;
//        if (!this.Rotation.Equals(pa.Rotation)) return false;
//        if (!this.RotationRandom.Equals(pa.RotationRandom)) return false;
//        if (!this.seed.Equals(pa.seed)) return false;
//        //if (!this.ShelfNumber.Equals(pa.ShelfNumber)) return false;
//        if (!this.Space.Equals(pa.Space)) return false;

//        return true;
//    }
//}
