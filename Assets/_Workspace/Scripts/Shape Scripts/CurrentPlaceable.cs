using System;
using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;

namespace _Workspace.Scripts.Shape_Scripts
{
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
            for (int i = 0; i < _shapePieces.Count; i++)
            {
                var piece = _shapePieces[i];
                if(piece == _primaryShapePiece)
                    piece.currentPlaceable.Place(placedShape,true);
                else
                    piece.currentPlaceable.Place(placedShape);
            }
            
            OnShapePlaced();
        }

        private void OnShapePlaced()
        {
            
        }
    }
}