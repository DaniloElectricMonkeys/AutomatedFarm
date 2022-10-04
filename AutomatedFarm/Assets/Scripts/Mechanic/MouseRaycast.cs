using UnityEngine;

public class MouseRaycast : Singleton<MouseRaycast>
{
    public Camera cam;
    public LayerMask buildableLayer;
    Ray ray;

    ///<summary>
    /// Fire a raycast that checks for the buildable layer.
    ///</summary>
    public RaycastHit FireRaycast()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        //ray = new Ray(cam.transform.position, cam.transform.forward * 100);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildableLayer))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 100, Color.red, 1f);
            return hit;
        }
        return hit;
    }

    ///<summary>
    /// Fire a raycast that checks for the specific layer.
    ///</summary>
    public RaycastHit FireRaycast(LayerMask layer)
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        //ray = new Ray(cam.transform.position, cam.transform.forward * 100);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layer))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 100, Color.red, 1f);
            return hit;
        }
        return hit;
    }
}
