using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T instance;
    public static T Instance { get { return instance; } }

    [SerializeField] bool _dontDestroyOnload = false;
    protected virtual void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject); // Phá hủy nếu đã có một instance khác
            return;
        }

        instance = (T)this;

        if (_dontDestroyOnload)
        {
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}
