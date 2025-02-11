using System;
using _Workspace.Scripts.Board_Scripts;
using _Workspace.Scripts.Diamond;
using _Workspace.Scripts.Managers;
using _Workspace.Scripts.Shape_Scripts;
using _Workspace.Scripts.SO_Scripts;
using _Workspace.Scripts.UI_Scripts;
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

        private int _targetDiamond;
        
        #endregion

        #region Unity Funcs

        private void OnEnable()
        {
            boardEventSo.OnDiamondCollected += BoardEventSo_OnDiamondCollected;
        }
        
        private void OnDisable()
        {
            boardEventSo.OnDiamondCollected -= BoardEventSo_OnDiamondCollected;
        }

        private void BoardEventSo_OnDiamondCollected(DiamondController arg0)
        {
            _targetDiamond--;
            if(_targetDiamond <= 0)
                levelEventSo.InvokeOnLevelCompleted(PlayerPrefsManager.GetCurrentLevel());
        }

        #endregion

        public void SetLevelData(LevelSO levelData)
        {
            _levelData = levelData;
            _targetDiamond = levelData.targetDiamond;
            shapeManager.SetShapeData(levelData.levelShapeDataList);
            boardController.GenerateBoard();
        }
    }
}