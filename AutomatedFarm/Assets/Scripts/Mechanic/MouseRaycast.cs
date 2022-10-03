using UnityEngine;

public class MouseRaycast : Singleton<MouseRaycast>
{
    public Camera cam;
    public LayerMask buildableLayer;
    Ray ray;

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
}
