using System;
using _Workspace.Scripts.Board_Scripts;
using _Workspace.Scripts.Shape_Scripts;
using _Workspace.Scripts.SO_Scripts;
using UnityEngine;

namespace _Workspace.Scripts.Level_Scripts
{
    public class LevelController : MonoBehaviour
    {
        #region Variables
        
        [Header("So References")]
        [SerializeField] private BoardEventSO boardEventSo;
        [SerializeField] private LevelEventSO levelEventSo;
        
        [Header("Level References")]
        [SerializeField] private BoardController boardController;
        [SerializeField] private ShapeManager shapeManager;
        private LevelSO _levelData;

        private int _currentPoint;
        
        #endregion

        #region Unity Funcs

        private void OnEnable()
        {
            boardEventSo.OnRowColumnFilled += BoardEventSo_OnRowColumnFilled;
        }

        private void OnDisable()
        {
            boardEventSo.OnRowColumnFilled -= BoardEventSo_OnRowColumnFilled;
        }

        private void BoardEventSo_OnRowColumnFilled(int filledRowCount)
        {
            int point = filledRowCount * _levelData.pointPerRowColumn;
            
            _currentPoint += point;
            
            levelEventSo.InvokeOnPointGained(point);

            if (_currentPoint >= _levelData.targetPoint)
            {
                levelEventSo.InvokeOnLevelCompleted(_levelData.levelId);
            }
        }

        #endregion

        public void SetLevelData(LevelSO levelData)
        {
            _levelData = levelData;
            shapeManager.SetShapeData(levelData.levelShapeDataList);
            boardController.GenerateBoard();
        }
    }
}