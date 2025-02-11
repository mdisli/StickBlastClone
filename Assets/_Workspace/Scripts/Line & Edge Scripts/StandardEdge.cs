using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public class StandardEdge : MonoBehaviour
    {
        #region Variables

        [Header("Edge Type References")] 
        [SerializeField] private GameObject _emptyEdge;
        [SerializeField] private GameObject _filledEdge;
        [SerializeField] private GameObject _shadowEdge;
        
        private Vector2Int _coordinate;
        public EdgeNeighbours _edgeNeighbours;
        public List<StandardEdge> _neighbourEdges = new List<StandardEdge>();
        public List<StandardEdge> connectedEdgesList = new List<StandardEdge>();

        #endregion

        #region Get & Set
        
        public Vector2Int GetCoordinate()
        {
            return _coordinate;
        }
        
        public void SetCoordinate(Vector2Int coordinate)
        {
            _coordinate = coordinate;
        }

        #endregion
        
        public void ClearConnectedEdges(List<StandardEdge> allEdges)
        {
            foreach (var edge in allEdges)
            {
                if (connectedEdgesList.Contains(edge))
                {
                    connectedEdgesList.Remove(edge);
                }
            }

            if (connectedEdgesList.Count != 0) return;
            
            CloseShadowEdge();
            CloseFilledEdge();
            _emptyEdge.SetActive(true);
        }

        #region Visual Effects
        public void OpenShadowEdge()
        {
            _shadowEdge.SetActive(true);
        }
        
        public void CloseShadowEdge()
        {
            _shadowEdge.SetActive(false);
        }
        
        public void OpenFilledEdge()
        {
            _emptyEdge.SetActive(false);
            _filledEdge.SetActive(true);
            CloseShadowEdge();
        }

        public void CloseFilledEdge()
        {
            _emptyEdge.SetActive(true);
            _filledEdge.SetActive(false);
            CloseShadowEdge();
        }

        #endregion
    }
    

    [Serializable]
    public class EdgeNeighbours
    {
        public StandardEdge _rightEdge;
        public StandardEdge _leftEdge;
        public StandardEdge _topEdge;
        public StandardEdge _bottomEdge;
    }
}