using System;
using _Workspace.Scripts.Level_Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.Scripts.Managers.Input_Manager
{
    public class InputManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private LayerMask clickableLayer;
        
        [SerializeField] private LevelEventSO levelEventSO;
        
        private bool OnMouseDown => Input.GetMouseButtonDown(0);
        private bool OnMouseUp => Input.GetMouseButtonUp(0);
        
        private bool _isDragging;
        
        private Camera _mainCamera;
        
        private IClickable _currentClickable;

        private bool _canClick;

        #endregion

        #region Unity Funcs

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            levelEventSO.OnLevelStarted += LevelEventSo_OnLevelStarted;
            levelEventSO.OnLevelCompleted += LevelEventSo_OnLevelCompleted;
            levelEventSO.OnLevelFailed += LevelEventSo_OnLevelFailed;
        }

        private void OnDisable()
        {
            levelEventSO.OnLevelStarted -= LevelEventSo_OnLevelStarted;
            levelEventSO.OnLevelCompleted -= LevelEventSo_OnLevelCompleted;
            levelEventSO.OnLevelFailed -= LevelEventSo_OnLevelFailed;
        }

        private void LevelEventSo_OnLevelFailed(int arg0)
        {
            _canClick = false;
        }

        private void LevelEventSo_OnLevelCompleted(int arg0)
        {
            _canClick = false;
        }

        private void LevelEventSo_OnLevelStarted()
        {
            _canClick = true;
        }

        private void Update()
        {
            if(!_canClick)
                return;
            
            if (OnMouseDown && CheckForClickable())
                OnClickDown();

            if (OnMouseUp)
                OnClickUp();
            
            if(_isDragging)
                OnDrag();
            
        }

        #endregion

        #region Vector Converting

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.y = 0;
            
            return worldPosition;
        }

        #endregion

        #region Clicking Funcs
        
        private bool CheckForClickable()
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            IClickable clickable = null;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {
                clickable = hit.collider.GetComponent<IClickable>();
                _currentClickable = clickable;
            }
            
            return clickable != null;
        }

        private async void OnClickDown()
        {
            _currentClickable.OnClickDown(GetMouseWorldPosition());

            await UniTask.Delay(50);
            
            _isDragging = true;
        }
        
        private void OnClickUp()
        {
            if(_isDragging)
                _currentClickable.OnClickUp();
                
            _isDragging = false;
        }
        
        private void OnDrag()
        {
            _currentClickable.OnDrag(GetMouseWorldPosition());
        }
        
        #endregion
        
    }
}