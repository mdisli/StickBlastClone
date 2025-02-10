using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public class StandardEdge : MonoBehaviour
    {
        #region Variables

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
        }
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