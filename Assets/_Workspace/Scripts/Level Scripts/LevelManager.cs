using System.Collections.Generic;
using _Workspace.Scripts.Managers;
using UnityEngine;

namespace _Workspace.Scripts.Level_Scripts
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables

        [Header("Event SO References")]
        [SerializeField] private LevelEventSO levelEventSO;
        
        [Header("Level So Data")]
        public List<LevelSO> levelList = new List<LevelSO>();
        
        [Header("Prefabs")]
        public LevelController levelControllerPrefab;

        #endregion

        #region Unity Functions

        private void Start()
        {
            int level = PlayerPrefsManager.GetCurrentLevel();
            
            GenerateLevel(level);
        }

        #endregion

        #region Generate Level

        private void GenerateLevel(int level)
        {
            LevelController levelController = Instantiate(levelControllerPrefab);
            
            LevelSO levelData = levelList[level];
            
            levelEventSO.InvokeOnLevelSelected(levelData);
            
            levelController.SetLevelData(levelData);
            
        }

        #endregion
    }
}