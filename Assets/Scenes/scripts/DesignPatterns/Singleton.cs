using UnityEngine;


namespace DesignPatterns
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected void SingleTonInit()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                //_instance = GetComponent<T>(); 무거워서
                _instance = this as T;
                DontDestroyOnLoad(_instance);
            }
        }
    }
}
