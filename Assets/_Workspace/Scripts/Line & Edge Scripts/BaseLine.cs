using System.Collections.Generic;
using _Workspace.Scripts.Shape_Scripts;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public abstract class BaseLine : MonoBehaviour, IPlaceable
    {
        #region Variables

        [Header("Line Settings")]
        public float _lineHeight = 1.5975f;
        [SerializeField] private float _lineOffset = 1.1f;
        [SerializeField] private BoxCollider _lineCollider;
        [SerializeField] public LineDirection _lineDirection;
        
        [Header("Animation")]
        [SerializeField] private SpriteRenderer _lineSprite;
        [SerializeField] private Color _lineColor;
        [SerializeField] private Color _lineHighlightColor;
        
        [Header("Line Status")]
        private BaseShape _placedShape;
        public bool _isAvailable => _placedShape == null;
        
        [Header("Edge Settings")]
        [SerializeField] protected List<StandardEdge> _edgeList = new List<StandardEdge>();
        
        private int _lineIndex;

        #endregion

        #region UnityFuncs

        public virtual void Start()
        {
            SetLineCollider();
        }

        #endregion

        #region Line Funcs

        protected void SetLineCollider()
        {
            var colSize = _lineCollider.size;
            colSize.x *= _lineOffset;
            _lineCollider.size = colSize;
        }

        public void SetLineIndex(int index)
        {
            _lineIndex = index;
        }
        
        public int GetLineIndex()
        {
            return _lineIndex;
        }
        
        #endregion

        #region Edge Funcs

        [Button]
        private void ToggleEdges()
        {
            foreach (var edge in _edgeList)
            {
                edge.gameObject.SetActive(!edge.gameObject.activeSelf);
            }
        }
        
        public void SetEdges(List<StandardEdge> edgeList)
        {
            _edgeList = edgeList;
        }

        #endregion

        #region IPlaceable
        
        public bool CheckShapePieceCanPlace(ShapePiece shapeToPlace)
        {
            if (!IsAvailable())
                return false;
            
            if(shapeToPlace.GetDirection() != GetDirection()) 
                return false;
            
            return true;
        }

        public void Place(BaseShape placedShape)
        {
            Transform placedTransform = placedShape.transform;
            placedTransform.SetParent(transform);
            placedTransform.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.Flash);
        }

        public BaseShape GetPlacedShape()
        {
            return _placedShape;
        }

        public bool IsAvailable()
        {
            return _isAvailable;
        }

        public LineDirection GetDirection()
        {
            return _lineDirection;
        }

        public void OnPlaceableEnter()
        {
            Debug.Log("OnPlaceableEnter");
            _lineSprite.color = _lineHighlightColor;
        }

        public void OnPlaceableExit()
        {
            Debug.Log("OnPlaceableExit");
            _lineSprite.color = _lineColor;
        }

        #endregion

        
    }
}