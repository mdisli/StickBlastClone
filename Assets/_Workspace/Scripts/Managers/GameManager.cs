using System;
using _Workspace.Scripts.Level_Scripts;
using _Workspace.Scripts.SO_Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [Header("So References")]
        [SerializeField] private LevelEventSO levelEventSO;
        [SerializeField] private BoardEventSO boardEventSo;

        [Header("Particle Prefabs")] 
        [SerializeField] private ParticleSystem[] confetties;

        #endregion
        
        #region Unity Funcs

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            levelEventSO.OnLevelCompleted += LevelEventSo_OnLevelCompleted;
        }

        private void OnDisable()
        {
            levelEventSO.OnLevelCompleted -= LevelEventSo_OnLevelCompleted;
        }

        private async void LevelEventSo_OnLevelCompleted(int arg0)
        {
            foreach (var confetty in confetties)
            {
                confetty.Play();
                await UniTask.Delay(150);
            }
        }

        #endregion
    }
}