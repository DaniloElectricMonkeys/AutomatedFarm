using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float time;

    private void Start() 
    {
        Destroy(gameObject, time);
    }
}
