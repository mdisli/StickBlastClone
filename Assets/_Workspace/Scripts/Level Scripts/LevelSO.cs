using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Workspace.Scripts.Level_Scripts
{
    [CreateAssetMenu(fileName = "Level So", menuName = "SO/Level So", order = 0)]
    public class LevelSO : ScriptableObject
    {
        public int levelId;
        [FormerlySerializedAs("pointPerSquare")] public int pointPerRowColumn;
        public float pointMultiplierOnDoubleSquare;
        public int targetPoint;
        public List<LevelShapeData> levelShapeDataList = new List<LevelShapeData>();
        
    }
    
    [Serializable]
    public class LevelShapeData
    {
        [Range(0,2)]public List<int> shapeIdList = new List<int>(3);
    }
}