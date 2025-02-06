using _Workspace.Scripts.Managers.Input_Manager;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Shape_Scripts
{
    public abstract class BaseShape : MonoBehaviour, IClickable
    {
        #region Variables

        public Vector3 positionOffset;
        private Vector3 _scaleEffect = Vector3.one * 1.1f;

        #endregion
        #region IClickable

        public void OnClickDown(Vector3 worldPosition)
        {
            ScaleShakeSequence();
            transform.DOMove(worldPosition + positionOffset, 0.05f).SetEase(Ease.Linear);
        }

        public void OnClickUp()
        {
            ScaleShakeSequence();
        }

        public void OnDrag(Vector3 worldPosition)
        {
            transform.position = worldPosition + positionOffset;
        }

        #endregion

        #region Tweens

        private Sequence ScaleShakeSequence()
        {
            DOTween.Kill(transform);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(transform.DOScale(_scaleEffect, 0.1f).SetEase(Ease.Linear))
                .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));

            return sequence;
        }

        #endregion
        
    }
}
