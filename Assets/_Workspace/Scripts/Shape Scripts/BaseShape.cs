using System;
using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using _Workspace.Scripts.Managers.Input_Manager;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Shape_Scripts
{
    public abstract class BaseShape : MonoBehaviour, IClickable
    {
        #region Variables

        [Header("Shape Settings")]
        [SerializeField] protected List<ShapePiece> _shapePieces = new List<ShapePiece>();
        [SerializeField] protected ShapePiece primaryPiece;
        private CurrentPlaceable _placeableController = new CurrentPlaceable();
        
        public Vector3 positionOffset;
        private Vector3 _scaleEffect = Vector3.one * 1.1f;

        #endregion

        #region Unity Funcs

        public virtual void Start()
        {
            _placeableController.SetPrimaryShape(primaryPiece);
            _placeableController.SetShapePieces(_shapePieces);
        }

        #endregion
        
        #region IClickable
        public void OnClickDown(Vector3 worldPosition)
        {
            ScaleShakeSequence();
            transform.DOMove(worldPosition + positionOffset, 0.05f).SetEase(Ease.Linear);
        }

        public void OnClickUp()
        {
            ScaleShakeSequence();
            _placeableController.Place(this);
        }

        public void OnDrag(Vector3 worldPosition)
        {
            transform.position = worldPosition + positionOffset;

            if (!_placeableController.CheckForPlacement())
            {
                _placeableController.OnPlaceableExit(true);
            }
            
            //if(_placeableController.CheckPlaceablesIsSame()) return;
            
            _placeableController.OnPlaceableEnter();
        }

        #endregion

        #region Tweens

        private Sequence ScaleShakeSequence()
        {
            DOTween.Kill(transform);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(transform.DOScale(_scaleEffect, 0.1f).SetEase(Ease.Linear))
                .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));

            return sequence;
        }

        #endregion
        
    }

    [Serializable]
    public class CurrentPlaceable
    {
        private List<ShapePiece> _shapePieces = new List<ShapePiece>();
        private ShapePiece _primaryShapePiece;
        
        private List<IPlaceable> _currentPlaceableList = new List<IPlaceable>();
        private List<IPlaceable> _prevPlaceableList = new List<IPlaceable>();

        #region Setters
        public void SetPrimaryShape(ShapePiece shapePiece)
        {
            _primaryShapePiece = shapePiece;
        }
        public void SetShapePieces(List<ShapePiece> shapePieces)
        {
            _shapePieces = shapePieces;
        }

        private void SetCurrentPlaceables()
        {
            _prevPlaceableList.Clear();
            _prevPlaceableList.AddRange(_currentPlaceableList);
            _currentPlaceableList.Clear();
            
            foreach (var shapePiece in _shapePieces)
            {
                if(shapePiece.currentPlaceable == null) continue;
                _currentPlaceableList.Add(shapePiece.currentPlaceable);
            }

            if (!CheckPlaceablesIsSame())
            {
                OnPlaceableExit();
            }
        }
        
        #endregion

        #region Check Funcs

        public bool CheckPlaceablesIsSame()
        {
            if(_currentPlaceableList.Count != _prevPlaceableList.Count) return false;
            
            for (int i = 0; i < _currentPlaceableList.Count; i++)
            {
                IPlaceable currentPlaceable = _currentPlaceableList[i];
                //IPlaceable prevPlaceable = _prevPlaceableList[i];
                
                if(!_prevPlaceableList.Contains(currentPlaceable))
                    return false;
                
                // if(currentPlaceable.GetLineIndex() != prevPlaceable.GetLineIndex())
                //     return false;
            }
            
            return true;
        }

         public bool CheckForPlacement()
        {
            bool status = true;
            
            foreach (var shapePiece in _shapePieces)
            {
                if (!shapePiece.CheckForPlacement())
                {
                    status = false;
                }
            }
            
            return status;
        }

        #endregion
        
        public void OnPlaceableEnter()
        {
            SetCurrentPlaceables();
            
            if(!CheckForPlacement())
                return;
            
            if(CheckPlaceablesIsSame()) return;
            
            foreach (var placeable in _shapePieces)
            {
                placeable.currentPlaceable?.OnPlaceableEnter();
            }
        }
        public void OnPlaceableExit(bool clearLists=false)
        {
            foreach (var placeable in _prevPlaceableList)
            {
                placeable.OnPlaceableExit();
            }

            if (!clearLists) return;
            
            _prevPlaceableList.Clear();
            _currentPlaceableList.Clear();

        }
        
        public void Place(BaseShape placedShape)
        {
            if(!CheckForPlacement()) return;
            
            _primaryShapePiece.currentPlaceable.Place(placedShape);
        }
        
        
    }
}
