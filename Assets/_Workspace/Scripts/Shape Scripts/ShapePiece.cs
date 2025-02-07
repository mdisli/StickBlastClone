using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using UnityEngine;

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
        
        public IPlaceable currentPlaceable;
        
        public LineDirection GetDirection()
        {
            return _shapePieceDirection;
        }

        #endregion

        public bool CheckForPlacement()
        {
            bool status = false;
            currentPlaceable = null;
            foreach (var transform1 in _rayPointList)
            {
                if (!Physics.Raycast(transform1.position, Vector3.down, out var hit, 5f, _lineLayerMask)) continue;
                
                var line = hit.collider.GetComponent<IPlaceable>();

                if(line == null) continue;
                
                bool status2 = line.CheckShapePieceCanPlace(this);
                
                if(!status2) continue;
                
                currentPlaceable = line;
                status = true;
                break;
            }
            
            return status;
        }
    }
}