using System.Collections.Generic;
using _Workspace.Scripts.Managers.Input_Manager;
using _Workspace.Scripts.SO_Scripts;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private int _shapeIndex;
        
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

            foreach (var shapePiece in _shapePieces)
            {
                shapePiece.SetSpriteRendererOrder();
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

        #region Abstract Funcs

        public async void BreakShape()
        {
            foreach (var shapePiece in _shapePieces)
            {
                await UniTask.Delay(Random.Range(0, 150));
                
                shapePiece.transform.SetParent(null);
                BreakShapePiece(shapePiece.transform).OnComplete(() =>
                {
                    Destroy(shapePiece.gameObject);
                    Destroy(gameObject);
                });
            }
        }

        #endregion
        
        #region Tweens

        private Sequence BreakShapePiece(Transform shapePiece)
        {
            Sequence sequence = DOTween.Sequence();
            
            Vector3 randomRotation = Random.Range(0, 100) % 2 == 0 ? new Vector3(0, Random.Range(100,300), 0) : new Vector3(0, Random.Range(-100,-300), 0);

            sequence.Join(shapePiece.DOScale(Vector3.zero, .6f).SetEase(Ease.Linear))
                .Join(shapePiece.DOLocalRotate(randomRotation, .5f,RotateMode.WorldAxisAdd).SetEase(Ease.Linear))
                .Join(shapePiece.DOMoveZ(-20,.8f).SetEase(Ease.Linear));

            return sequence;
        }
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

        public void SetShapeIndex(int index)
        {
            _shapeIndex = index;
        }
        
        public int GetShapeIndex()
        {
            return _shapeIndex;
        }
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
