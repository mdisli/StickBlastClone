using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using _Workspace.Scripts.Managers.Input_Manager;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Shape_Scripts
{
    public abstract class BaseShape : MonoBehaviour, IClickable
    {
        #region Variables

        [Header("Shape Settings")]
        [SerializeField] protected List<ShapePiece> _shapePieces = new List<ShapePiece>();
        [SerializeField] protected ShapePiece primaryPiece;
        private BaseLine _placeableLine = null;
        
        public Vector3 positionOffset;
        private Vector3 _scaleEffect = Vector3.one * 1.1f;

        #endregion

        #region Abstracts

        public virtual bool CheckForPlacement()
        {
            bool status = true;
            
            foreach (var shapePiece in _shapePieces)
            {
                if (!shapePiece.CheckForPlacement())
                {
                    status = false;
                }
            }
            
            return status;
        }

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

            if (_placeableLine != null)
            {
                transform.SetParent(_placeableLine.transform);
                transform.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.Linear);
                Debug.Log("Placed");
            }
        }

        public void OnDrag(Vector3 worldPosition)
        {
            transform.position = worldPosition + positionOffset;

            if (CheckForPlacement())
            {
                _placeableLine = primaryPiece.placaebleLine;
                Debug.Log("Placeable");
            }
            else
            {
                _placeableLine = null;
                Debug.Log("Not Placeable");
            }
            //_placeableLine = CheckForPlacement() ? primaryPiece.placaebleLine : null;
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
