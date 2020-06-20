using UnityEngine;

namespace IceShave
{
    public class FallenIce : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Fallen Ice") && collision.contactCount > 0)
            {
                var newInstance = Instantiate(this, transform.parent);
                newInstance.transform.position = collision.GetContact(0).point;

                Destroy(collision.gameObject);
            }
        }
    }
}
