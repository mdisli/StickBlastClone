using UnityEngine;
using UnityEngine.Events;

namespace _Workspace.Scripts.SO_Scripts
{
    [CreateAssetMenu(fileName = "BoardEventSo", menuName = "SO/Event SO/Board Event SO", order = 0)]
    public class BoardEventSO : ScriptableObject
    {
        #region Events

        public UnityAction OnShapePlaced;

        #endregion

        #region Invoking

        public void InvokeOnShapePlaced()
        {
            OnShapePlaced?.Invoke();
        }

        #endregion
    }
}