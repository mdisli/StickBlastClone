using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Board_Scripts
{
    public class FilledSquare : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Vector2 _coordinate; 
        private List<StandardEdge> _edgeList = new List<StandardEdge>();

        #endregion

        #region Getter & Setter

        public Vector2 GetCoordinate()
        {
            return _coordinate;
        }

        private void SetCoordinate(Vector2 coordinate)
        {
            _coordinate = coordinate;
        }

        #endregion

        #region Generating

        public void GenerateSquare(Vector3 worldPosition, Vector2 coordinate)
        {
            SetCoordinate(coordinate);
            transform.localPosition = worldPosition;
            transform.localScale = Vector3.zero;
            OpenSquare();
        }
        
        public void OpenSquare()
        {
            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
        }

        #endregion
        
        public void SetEdges(List<StandardEdge> edgeList)
        {
            _edgeList = edgeList;
        }
        
        public bool CompareEdges(List<StandardEdge> edgeList)
        {
            foreach (var edge in edgeList)
            {
                if (!_edgeList.Contains(edge))
                    return false;
            }
            
            return true;
        }
    }
}