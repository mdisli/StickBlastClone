using UnityEngine;

namespace _Workspace.Scripts.SO_Scripts
{
    [CreateAssetMenu(fileName = "Animation SO", menuName = "SO/Animation SO", order = 0)]
    public class AnimationSO : ScriptableObject
    {
        #region Variables

        [Header("Dragging Animation")] 
        public Vector3 dragOffset;

        [Header("Shape Animations")] 
        public float shapeScaleUpMultiplier;
        public float shapeScaleDownMultiplier;
        

        #endregion
    }
}