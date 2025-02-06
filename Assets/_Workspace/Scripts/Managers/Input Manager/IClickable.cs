using UnityEngine;

namespace _Workspace.Scripts.Managers.Input_Manager
{
    public interface IClickable
    {
        public void OnClickDown(Vector3 worldPosition);
        public void OnClickUp();
        public void OnDrag(Vector3 worldPosition);
    }
}