using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GAS.General.Util
{
    public static class GASAnimatorUtil
    {
        /// <summary>
        ///     Only For Editor
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static Dictionary<string, AnimationClip> GetAllAnimationState(this Animator animator, int layerIndex = 0)
        {
            var result = new Dictionary<string, AnimationClip>();

            var runtimeController = animator.runtimeAnimatorController;
            if (runtimeController == null)
            {
                Debug.LogError("RuntimeAnimatorController must not be null.");
                return null;
            }

            if (animator.runtimeAnimatorController is AnimatorOverrideController)
            {
                var overrideController =
                    AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(
                        AssetDatabase.GetAssetPath(runtimeController));
                if (overrideController == null)
                {
                    Debug.LogErrorFormat("AnimatorOverrideController must not be null.");
                    return null;
                }

                var controller =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(
                        AssetDatabase.GetAssetPath(overrideController.runtimeAnimatorController));
                var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                overrideController.GetOverrides(overrides);
                // 获取 Layer 的状态机
                var stateMachine = controller.layers[layerIndex].stateMachine;
                // 遍历所有状态并打印名称
                foreach (var state in stateMachine.states)
                {
                    foreach (var pair in overrides)
                    {
                        if (state.state.motion is AnimationClip clip)
                        {
                            result.Add(state.state.name, pair.Key.name == clip.name ? pair.Value : pair.Key);
                            break;
                        }
                    }
                }
            }
            else
            {
                var controller =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(runtimeController));
                if (controller == null)
                {
                    Debug.LogErrorFormat("AnimatorController must not be null.");
                    return null;
                }

                // 获取第一个 Layer 的状态机
                var stateMachine = controller.layers[layerIndex].stateMachine;
                // 遍历所有状态并打印名称
                foreach (var state in stateMachine.states)
                    result.Add(state.state.name, state.state.motion as AnimationClip);
            }

            return result;
        }
    }
}