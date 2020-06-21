using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static readonly object _locker = new object ();

        private static T _instance;
        public static T Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        T singleton = GameObject.FindObjectOfType<T> ();
                        if (singleton != null)
                        {
                            _instance = singleton;
                        }
                        else
                        {
                            GameObject obj = new GameObject (typeof (T).ToString ());
                            GameObject.DontDestroyOnLoad (obj);

                            _instance = obj.AddComponent<T> ();
                        }
                    }

                    return _instance;
                }
            }
        }
    }
}