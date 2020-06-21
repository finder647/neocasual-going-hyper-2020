using UnityEngine;

namespace NeoCasual.GoingHyper.UIs
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _handUI;

        private bool _isFirstSwipeDone;

        public void OnStartSwiping(Vector2 position)
        {
            if (_isFirstSwipeDone) return;

            _handUI.gameObject.SetActive(false);
            _isFirstSwipeDone = true;
        }
    }
}
