using _Workspace.Scripts.Level_Scripts;
using _Workspace.Scripts.SO_Scripts;
using UnityEngine;

namespace _Workspace.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [Header("So References")]
        [SerializeField] private LevelEventSO levelEventSO;
        [SerializeField] private BoardEventSO boardEventSo;

        #endregion
        
        #region Unity Funcs

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        #endregion
    }
}