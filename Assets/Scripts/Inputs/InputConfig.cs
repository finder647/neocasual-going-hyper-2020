using UnityEngine;

namespace NeoCasual.GoingHyper.Inputs
{
    [CreateAssetMenu (menuName = Constant.CONFIG_MENU_ROOT + "Input")]
    public class InputConfig : ScriptableObject
    {
        [Range (0f, 1f)] public float SwipeMinDetectDistance = 0.01f;
    }
}