using System.Collections.Generic;
using _Workspace.Scripts.Diamond;
using _Workspace.Scripts.Line___Edge_Scripts;
using _Workspace.Scripts.SO_Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Board_Scripts
{
    public class FilledSquare : MonoBehaviour
    {
        #region Variables

        [SerializeField] private BoardEventSO boardEventSo;
        [SerializeField] private Transform spriteHolder;
        
        private Vector2 _coordinate; 
        private List<StandardEdge> _edgeList = new List<StandardEdge>();
        private List<BaseLine> _lineList = new List<BaseLine>();

        private DiamondController _placedDiamond;

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

        public List<BaseLine> GetLineList()
        {
            return _lineList;
        }
        
        public void SetLineList(List<BaseLine> lineList)
        {
            _lineList = lineList;
        }

        public DiamondController GetPlacedDiamond()
        {
            return _placedDiamond;
        }
        
        public void SetPlacedDiamond(DiamondController diamond)
        {
            _placedDiamond = diamond;
            
            if(_placedDiamond == null) return;
            
            Transform diamondTransform = diamond.transform;
            diamondTransform.SetParent(transform);
            diamondTransform.localPosition = Vector3.zero;
            diamondTransform.SetParent(null);
            diamondTransform.DOScale(Vector3.one, .25f).SetEase(Ease.Linear);
        }
        
        public bool CheckDiamondIsPlaced()
        {
            return _placedDiamond != null;
        }
        
        #endregion

        #region Generating

        public async UniTask GenerateSquare(Vector3 worldPosition, Vector2 coordinate)
        {
            SetCoordinate(coordinate);
            transform.localPosition = worldPosition;
            transform.localScale = Vector3.zero;
            await OpenSquare().AsyncWaitForCompletion();
        }
        
        private Tween OpenSquare()
        {
            transform.localScale = Vector3.one;
            
            return spriteHolder.DOScale(Vector3.one, 0.25f).SetEase(Ease.Linear);
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

        public void RemoveSquare()
        {
            if (CheckDiamondIsPlaced())
            {
                boardEventSo.InvokeOnDiamondCollected(_placedDiamond);
                SetPlacedDiamond(null);
            }
            
            foreach (var baseLine in _lineList)
            {
                if(!CheckLineIsPartOfSquare(baseLine))
                    continue;
                
                baseLine.RemovePlacedShape();
            }

            foreach (var standardEdge in _edgeList)
            {
                standardEdge.ClearConnectedEdges(_edgeList);
            }
            
            transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        private bool CheckLineIsPartOfSquare(BaseLine line)
        {
            var connectingEdges= line.GetConnectingEdges();
            
            foreach (var edge in connectingEdges)
            {
                if (!_edgeList.Contains(edge))
                    return false;
            }
            
            return true;
        }

        public void OpenFilledEdges()
        {
            foreach (var edge in _edgeList)
            {
                edge.OpenFilledEdge();
            }
        }
    }
}