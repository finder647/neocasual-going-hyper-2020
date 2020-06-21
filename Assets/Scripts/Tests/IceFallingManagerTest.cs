using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class IceFallingManagerTest : MonoBehaviour
    {
        [SerializeField]
        private FallingIce _fallingIce;
        [SerializeField]
        private int _fallingIcePerClick = 3;

        public void SpawnFallenIce(Vector3 position)
        {
            for (int i = 0; i < _fallingIcePerClick; i++)
            {
                var fallingIce = Instantiate(_fallingIce, null);
                fallingIce.Drop(position);
            }            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.position.z * -1;
                SpawnFallenIce(Camera.main.ScreenToWorldPoint(mousePos));
            }
        }
    }
}

