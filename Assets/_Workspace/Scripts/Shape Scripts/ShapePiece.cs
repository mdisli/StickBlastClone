using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Workspace.Scripts.Shape_Scripts
{
    public class ShapePiece : MonoBehaviour
    {
        #region Variables

        [Header("Shape Piece Settings")]
        [SerializeField] private LineDirection _shapePieceDirection;
        
        [Header("Raycast Variables")]
        [SerializeField] private LayerMask _lineLayerMask;
        [SerializeField] private List<Transform> _rayPointList =new List<Transform>();
        
        public BaseLine placaebleLine;

        #endregion

        public bool CheckForPlacement()
        {
            bool status = false;
            placaebleLine = null;
            foreach (var transform1 in _rayPointList)
            {
                if (!Physics.Raycast(transform1.position, Vector3.down, out var hit, 1f, _lineLayerMask)) continue;
                
                var line = hit.collider.GetComponent<BaseLine>();
                
                if (line == null || !line._isAvailable) continue;
                
                if(_shapePieceDirection != line._lineDirection) continue;
                
                placaebleLine = line;
                status = true;
                break;
            }
            
            return status;
        }
    }
}