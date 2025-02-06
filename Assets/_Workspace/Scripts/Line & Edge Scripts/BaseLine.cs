using System.Collections.Generic;
using _Workspace.Scripts.Shape_Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public abstract class BaseLine : UnityEngine.MonoBehaviour
    {
        #region Variables

        [Header("Line Settings")]
        public float _lineHeight = 1.5975f;
        [SerializeField] private float _lineOffset = 1.1f;
        [SerializeField] private BoxCollider _lineCollider;
        [SerializeField] public LineDirection _lineDirection;
        
        [Header("Line Status")]
        private BaseShape _placedShape;
        public bool _isAvailable => _placedShape == null;
        
        [Header("Edge Settings")]
        [SerializeField] protected List<StandardEdge> _edgeList = new List<StandardEdge>();

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
    }
}