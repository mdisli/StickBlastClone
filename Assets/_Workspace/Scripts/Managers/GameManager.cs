using System;
using UnityEngine;

namespace _Workspace.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Unity Funcs

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        #endregion
    }
}