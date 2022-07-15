using UnityEngine;

namespace Project.Utils.Extension.AnimatorNS
{
    public static class AnimatorExtension
    {
        public static float GetCurrentAnimationLength(this Animator targetAnim, int layer = 0)
        {
            return targetAnim.GetCurrentAnimatorStateInfo(layer).length;
        }
    }
}
