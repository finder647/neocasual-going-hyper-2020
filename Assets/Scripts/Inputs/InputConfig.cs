using UnityEngine;

namespace NeoCasual.GoingHyper.Inputs
{
    [CreateAssetMenu (menuName = Constant.CONFIG_MENU_ROOT + "Input")]
    public class InputConfig : ScriptableObject
    {
        public float HoldMinDetectTime = 0.04f;
    }
}