using _Workspace.Scripts.Board_Scripts;
using _Workspace.Scripts.Shape_Scripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace _Workspace.Scripts.Level_Scripts
{
    public class LevelController : MonoBehaviour
    {
        #region Variables
        
        [Header("Level References")]
        [SerializeField] private BoardController boardController;
        [SerializeField] private ShapeManager shapeManager;

        #endregion

        public void SetLevelData(LevelSO levelData)
        {
            shapeManager.SetShapeData(levelData.levelShapeDataList);
            boardController.GenerateBoard();
        }
    }
}