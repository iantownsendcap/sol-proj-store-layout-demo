using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

// using static UnityEditor.Rendering.CameraUI;
//[ExecuteInEditMode]

public class CameraScript : MonoBehaviour
{

    //public PostProcessVolume volume;

    float currentDist;
    float goalDist;
    public float focustime = 0.1f;
    public float Offset = 0;

    public float high = 70;
    public float low = 17;
    public float alph = 0.1f;

    public Volume vol;
    public DepthOfField DOF;


    // Start is called before the first frame update
    void Start()
    {
        //postProcessVolume = GetComponent<Camera>().gameObject.GetComponent<PostProcessVolume>();

        vol = gameObject.GetComponent<Volume>();
        vol.sharedProfile.TryGet<DepthOfField>(out DOF);

        

    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))//, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            //Debug.Log(hit.transform.name);
            goalDist = hit.distance + Offset;
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }

        currentDist += (goalDist - currentDist) * focustime;// * Time.deltaTime);

        //gameObject.GetComponent<Camera>().focusDistance = currentDist;


        // m_LensShift
        // physicalParameters.m_FocusDistance



        //PhysicalCamera.Styles.lens.
        // Camera.main.usePhysicalProperties = true;
        //Camera.current.GetComponent<UnityEditor.Rendering.CameraUI.PhysicalCamera>

        //UnityEditor.Rendering.CameraUI.PhysicalCamera.Styles.
        float n = Mathf.Clamp((currentDist * alph), 0, 1);
        //Camera.main.focalLength = Mathf.Lerp(high, low, n);

        DOF.focusDistance.value = currentDist;

        //DOF.nearFocusStart.value = 0;
        //DOF.nearFocusEnd.value = 4;
        //DOF.farFocusStart.value = 10;
        //DOF.farFocusEnd.value = 20;

        DOF.nearFocusStart.value = 0;
        DOF.nearFocusEnd.value = currentDist - 3;
        DOF.farFocusStart.value = currentDist + 3;
        DOF.farFocusEnd.value = currentDist + 3 + 10;

        // Camera.current.foc

        //Debug.Log($"{currentDist}, {n}");

    }
}
