using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.VisualEffects
{
    public class VisualEffectProvider : SingletonMono<VisualEffectProvider>
    {
        [SerializeField]
        private VisualEffectPoolObject[] _assetBank;

        private bool _isInitialized;

        public List<VisualEffectPoolObject> RuntimePool { get; protected set; }

        public void PlayVFXAt(string assetId, Vector3 position)
        {
            GetObject(assetId)?.Play(position);
        }

        public VisualEffectPoolObject GetObject(string assetId)
        {
            LazyInit();

            var runtimeAsset = RuntimePool.Find(
                poolObj => poolObj != null && !poolObj.IsPlaying && poolObj.name.Equals(assetId));
            if (runtimeAsset != null)
            {
                return runtimeAsset;
            }
            else
            {
                foreach (var resourceAsset in _assetBank)
                {
                    if (resourceAsset.name.Equals(assetId))
                    {
                        runtimeAsset = Instantiate(resourceAsset, transform);
                        runtimeAsset.name = assetId;
                        RuntimePool.Add(runtimeAsset);
                        return runtimeAsset;
                    }
                }
            }

            Debug.LogWarning($"can't find asset with id `{assetId}` in asset bank...");
            return null;
        }

        public void LazyInit()
        {
            if (_isInitialized) return;

            RuntimePool = new List<VisualEffectPoolObject>();
            foreach (var resourceAsset in _assetBank)
            {
                if (resourceAsset != null)
                {
                    var runtimeAsset = Instantiate(resourceAsset, transform);
                    runtimeAsset.name = resourceAsset.name;
                    RuntimePool.Add(runtimeAsset);
                }
            }

            _isInitialized = true;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
