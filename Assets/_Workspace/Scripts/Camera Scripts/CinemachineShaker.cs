using System;
using _Workspace.Scripts.SO_Scripts;
using Cinemachine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Camera_Scripts
{
    public class CinemachineShaker : MonoBehaviour
    {
        #region Variables

        [Header("SO References")]
        [SerializeField] private BoardEventSO boardEventSO;
        
        [Header("Shake Settings")]
        [SerializeField] private float shakeDuration = 0.3f;
        [SerializeField] private float shakeAmplitude = 1.2f;
        
        [Header("Genereal References")]
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _virtualCamera;

        #endregion

        #region Unity Funcs

        private void OnEnable()
        {
            boardEventSO.OnRowColumnFilled += BoardEventSo_OnRowColumnFilled;
        }

        private void OnDisable()
        {
            boardEventSO.OnRowColumnFilled -= BoardEventSo_OnRowColumnFilled;
        }

        #endregion
        
        #region Shaking

        [Button]
        private async void ShakeCamera()
        {
            CinemachineBasicMultiChannelPerlin perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            perlin.m_AmplitudeGain = shakeAmplitude;

            await UniTask.Delay((int) (shakeDuration * 1000));
            
            perlin.m_AmplitudeGain = 0;
        }

        #endregion

        #region Callbacks
        
        private void BoardEventSo_OnRowColumnFilled(int arg0)
        {
            ShakeCamera();
        }

        #endregion
    }
}