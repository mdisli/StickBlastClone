using System.Security.Cryptography.X509Certificates;

namespace _Workspace.Scripts.Managers
{
    public static class PlayerPrefsManager
    {
        #region Keys

        private static readonly string CurrentLevelKey = "CurrentLevel";
        

        #endregion

        #region Get & Set

        public static int GetCurrentLevel()
        {
            return UnityEngine.PlayerPrefs.GetInt(CurrentLevelKey, 0);
        }
        
        public static void SetCurrentLevel(int level)
        {
            UnityEngine.PlayerPrefs.SetInt(CurrentLevelKey, level);
        }

        public static void IncreaseLevel()
        {
            int currentLevel = GetCurrentLevel();
            SetCurrentLevel(currentLevel + 1);
        }

        #endregion
    }
}