using _Workspace.Scripts.Board_Scripts;
using _Workspace.Scripts.Diamond;
using _Workspace.Scripts.Shape_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace _Workspace.Scripts.SO_Scripts
{
    [CreateAssetMenu(fileName = "BoardEventSo", menuName = "SO/Event SO/Board Event SO", order = 0)]
    public class BoardEventSO : ScriptableObject
    {
        #region Events

        public UnityAction<BaseShape> OnShapePlaced;
        public UnityAction<int> OnSquareFilled;
        public UnityAction<int> OnRowColumnFilled;
        public UnityAction<DiamondController> OnDiamondCollected;

        #endregion

        #region Invoking

        public void InvokeOnShapePlaced(BaseShape placedShape)
        {
            OnShapePlaced?.Invoke(placedShape);
        }
        
        public void InvokeOnSquareFilled(int squareCount)
        {
            OnSquareFilled?.Invoke(squareCount);
        }
        
        public void InvokeOnRowColumnFilled(int rowColumnCount)
        {
            OnRowColumnFilled?.Invoke(rowColumnCount);
        }
        
        public void InvokeOnDiamondCollected(DiamondController collectedDiamond)
        {
            OnDiamondCollected?.Invoke(collectedDiamond);
        }

        #endregion
    }
}