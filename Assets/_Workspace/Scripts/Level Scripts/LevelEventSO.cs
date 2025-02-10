using UnityEngine;
using UnityEngine.Events;

namespace _Workspace.Scripts.Level_Scripts
{
    [CreateAssetMenu(fileName = "Level Event SO", menuName = "SO/Event SO/Level Event SO", order = 0)]
    public class LevelEventSO : ScriptableObject
    {
        #region Events

        public UnityAction<LevelSO> OnLevelSelected;
        public UnityAction<int> OnLevelGenerated;
        public UnityAction OnLevelStarted;
        public UnityAction<int> OnLevelCompleted;
        public UnityAction<int> OnLevelFailed;

        #endregion

        #region Invoking

        public void InvokeOnLevelSelected(LevelSO levelData) => OnLevelSelected?.Invoke(levelData);
        public void InvokeOnLevelGenerated(int levelIndex) => OnLevelGenerated?.Invoke(levelIndex);
        
        public void InvokeOnLevelStarted() => OnLevelStarted?.Invoke();
        
        public void InvokeOnLevelCompleted(int levelIndex) => OnLevelCompleted?.Invoke(levelIndex);
        
        public void InvokeOnLevelFailed(int levelIndex) => OnLevelFailed?.Invoke(levelIndex);

        #endregion
    }
}