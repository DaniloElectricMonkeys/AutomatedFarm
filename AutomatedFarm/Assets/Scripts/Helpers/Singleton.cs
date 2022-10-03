using UnityEngine;

public class Singleton<Class> : MonoBehaviour where Class : Component
{
    protected static Class _instance;

    public static Class Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(Class)) as Class[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(Class).Name + " in the scene.");
                }
                //if (_instance == null)
                //{
                //    GameObject obj = new GameObject();
                //    obj.hideFlags = HideFlags.HideAndDontSave;
                //    _instance = obj.AddComponent<T>();
                //}
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    
}
