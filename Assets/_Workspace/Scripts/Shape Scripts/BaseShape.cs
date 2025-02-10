using System.Collections.Generic;
using _Workspace.Scripts.Managers.Input_Manager;
using _Workspace.Scripts.SO_Scripts;
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
        [SerializeField] protected Transform _shapeTransform;
        private CurrentPlaceable _placeableController = new CurrentPlaceable();
        [SerializeField] private Collider[] _colliders;
        
        [Header("So References")]
        [SerializeField] private AnimationSO _animationSO;

        #endregion

        #region Unity Funcs

        public virtual void Start()
        {
            _placeableController.SetPrimaryShape(primaryPiece);
            _placeableController.SetShapePieces(_shapePieces);
        }

        #endregion
        
        #region IClickable
        public void OnClickDown(Vector3 worldPosition)
        {
            ScaleUpShakeSequence();
            transform.DOMove(worldPosition + _animationSO.dragOffset, 0.05f).SetEase(Ease.Linear);
        }

        public void OnClickUp()
        {
            ScaleDownShakeSequence();

            if (!_placeableController.CheckForPlacement())
            {
                transform.DOLocalMove(Vector3.zero, .35f).SetEase(Ease.Flash);
                return;
            }
            
            _placeableController.Place(this);

            OnShapePlaced();
        }
        
        protected virtual void OnShapePlaced()
        {
            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
        }
        public void OnDrag(Vector3 worldPosition)
        {
            transform.position = worldPosition + _animationSO.dragOffset;

            if (!_placeableController.CheckForPlacement())
            {
                _placeableController.OnPlaceableExit(true);
            }
            _placeableController.OnPlaceableEnter();
        }

        #endregion

        #region Tweens

        private Sequence ScaleUpShakeSequence()
        {
            DOTween.Kill(transform);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(transform.DOScale(_animationSO.shapeScaleUpMultiplier, 0.1f).SetEase(Ease.Linear))
                .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));

            return sequence;
        }
        
        private Sequence ScaleDownShakeSequence()
        {
            DOTween.Kill(transform);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(transform.DOScale(_animationSO.shapeScaleDownMultiplier, 0.1f).SetEase(Ease.Linear))
                .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));

            return sequence;
        }
        

        #endregion

        public Transform GetShapeTransform()
        {
            return _shapeTransform;
        }

        public void MoveToSpawnPoint(Transform shapeSpawnPoint)
        {
            transform.SetParent(shapeSpawnPoint);
            transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        }
    }
}
