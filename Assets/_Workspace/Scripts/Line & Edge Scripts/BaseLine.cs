using System.Collections.Generic;
using _Workspace.Scripts.Shape_Scripts;
using _Workspace.Scripts.SO_Scripts;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public abstract class BaseLine : MonoBehaviour, IPlaceable
    {
        #region Variables

        [Header("Event SO")]
        [SerializeField] protected BoardEventSO _boardEventSO;
        
        [Header("Line Settings")]
        public float _lineHeight = 1.5975f;
        [SerializeField] private float _lineOffset = 1.1f;
        [SerializeField] private BoxCollider _lineCollider;
        [SerializeField] public LineDirection _lineDirection;
        [SerializeField] private List<StandardEdge> _connectingEdges;
        
        [Header("Animation")]
        [SerializeField] private SpriteRenderer _lineSprite;
        [SerializeField] private GameObject _lineHighlighter;
        
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

        public void RemovePlacedShape()
        {
            if(_placedShape is not null)
                _placedShape.BreakShape();
            _placedShape = null;
        } 
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
        
        public void SetConnectingEdges(StandardEdge edge1, StandardEdge edge2)
        {
            _connectingEdges = new List<StandardEdge> {edge1, edge2};
        }
        
        public List<StandardEdge> GetConnectingEdges()
        {
            return _connectingEdges;
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

        public bool CheckConnectingEdges(params StandardEdge[] edges)
        {
            int count = 0;
            
            foreach (var edge in edges)
            {
                if (_connectingEdges.Contains(edge))
                    count++;
            }
            
            return count == 2;
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

        private void SetHighLighterStatus(bool status)
        {
            _lineHighlighter.SetActive(status);
        }
        
        public void Place(BaseShape placedShape, bool isAnimate = false)
        {
            _placedShape = placedShape;

            _edgeList[0].connectedEdgesList.Add(_edgeList[1]);
            _edgeList[1].connectedEdgesList.Add(_edgeList[0]);

            SetHighLighterStatus(false);
            
            foreach (var connectingEdge in _connectingEdges)
            {
                connectingEdge.OpenFilledEdge();
            }
            
            if(!isAnimate) return;
            
            // Invoke it for once per placing
            _boardEventSO.InvokeOnShapePlaced(placedShape);
            
            Transform placedTransform = placedShape.GetShapeTransform();
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
            SetHighLighterStatus(true);
            foreach (var connectingEdge in _connectingEdges)
            {
                connectingEdge.OpenShadowEdge();
            }
        }

        public void OnPlaceableExit()
        {
            SetHighLighterStatus(false);
            
            foreach (var connectingEdge in _connectingEdges)
            {
                connectingEdge.CloseShadowEdge();
            }
        }

        #endregion
    }
}