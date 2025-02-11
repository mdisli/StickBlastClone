using System;
using System.Collections.Generic;
using _Workspace.Scripts.Level_Scripts;
using _Workspace.Scripts.SO_Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.Scripts.Shape_Scripts
{
    public class ShapeManager : MonoBehaviour
    {
        #region Variables
        
        [Header("So References")]
        [SerializeField] private BoardEventSO boardEventSo;
        
        [Header("Shape Data")]
        [SerializeField] private List<BaseShape> shapeList = new List<BaseShape>();

        [Header("Spawning")] 
        [SerializeField] private Transform shapeHolderTransform;
        [SerializeField] private List<Transform> shapeSpawnPoints;

        [Header("Data")] 
        [SerializeField] private List<LevelShapeData> shapeData;
        
        private List<BaseShape> _generatedShapes = new List<BaseShape>();
        private List<BaseShape> _usingShapes = new List<BaseShape>();
        
        #endregion

        #region Unity Funcs

        private void OnEnable()
        {
            boardEventSo.OnShapePlaced += BoardEventSo_OnShapePlaced;
        }

        private void OnDisable()
        {
            boardEventSo.OnShapePlaced -= BoardEventSo_OnShapePlaced;
        }

        #endregion

        #region Data

        public void SetShapeData(List<LevelShapeData> data)
        {
            shapeData = data;
            
            GenerateShapes();
        }

        #endregion

        #region Generate Shapes

        private async void GenerateShapes()
        {
            int shapeIndex = 0;
            foreach (var levelShapeData in shapeData)
            {
                for (int i = 0; i < 3; i++)
                {
                    var shapeId = levelShapeData.shapeIdList[i];
                    BaseShape shape = Instantiate(shapeList[shapeId], shapeHolderTransform);
                    shape.SetShapeIndex(shapeIndex);
                    shapeIndex++;
                    shape.transform.localPosition = new Vector3(15, 0, 0);
                    
                    _generatedShapes.Add(shape);
                }
            }

            await UniTask.Delay(250);
            
            GetShapesToSpawnPoint();
        }

        private void GetShapesToSpawnPoint()
        {
            for (int i = 0; i < 3; i++)
            {
                var shape = _generatedShapes[0];
                _usingShapes.Add(shape);
                shape.MoveToSpawnPoint(shapeSpawnPoints[i]);
                _generatedShapes.RemoveAt(0);
            }
        }

        #endregion

        #region Event Callbacks

        private void BoardEventSo_OnShapePlaced(BaseShape placedShape)
        {
            _usingShapes.Remove(placedShape);
            
            if(_usingShapes.Count == 0)
                GetShapesToSpawnPoint();
        }

        #endregion
    }
}