using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public static class ResourceManager
    {
        private static Dictionary<string, GameObject> _prefabCaches = new Dictionary<string, GameObject> ();
        private static Dictionary<string, ScriptableObject> _scriptableCaches = new Dictionary<string, ScriptableObject> ();

        public static GameObject GetOrCreatePrefab (string name, Transform parent = null)
        {
            CheckAndLoadPrefab (name);

            GameObject obj = GameObject.Instantiate (_prefabCaches[name]);
            obj.name = name;

            if (parent != null)
            {
                obj.transform.SetParent (parent, false);
            }

            return obj;
        }

        public static T GetOrCreatePrefabComponent<T> (string name, Transform parent = null) where T : Component
        {
            GameObject obj = GetOrCreatePrefab (name, parent);
            return obj.GetComponent<T> ();
        }

        public static T GetScriptable<T> (string name) where T : ScriptableObject
        {
            CheckAndLoadScriptable (name);

            return _scriptableCaches[name] as T;
        }

        private static void CheckAndLoadPrefab (string name)
        {
            if (!_prefabCaches.ContainsKey (name))
            {
                _prefabCaches.Add (name, Resources.Load (Constant.PREFAB_ROOT + name) as GameObject);
            }
        }

        private static void CheckAndLoadScriptable (string name)
        {
            if (!_scriptableCaches.ContainsKey (name))
            {
                _scriptableCaches.Add (name, Resources.Load (Constant.SCRIPTABLE_ROOT + name) as ScriptableObject);
            }
        }
    }
}