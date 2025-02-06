using System.Collections.Generic;
using _Workspace.Scripts.Line___Edge_Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Board_Scripts
{
    public class BoardController : MonoBehaviour
    {
        #region Variables

        [Header("Prefabs")] [SerializeField] private BaseLine linePrefab;
        [SerializeField] private StandardEdge edgePrefab;

        [Header("Grid Settings")] [SerializeField]
        private int _edgeXCount;

        [SerializeField] private int _edgeZCount;
        [SerializeField] private float _edgeSpacing;
        [SerializeField] private Transform _edgeParent;

        [Header("Lists")] [SerializeField] private List<StandardEdge> _edgeList = new List<StandardEdge>();
        [SerializeField] private List<BaseLine> _lineList = new List<BaseLine>();

        #endregion

        #region Generating Board

        [Button("Generate Board")]
        private void GenerateBoard()
        {
            GenerateEdges();
            GenerateLines();
        }

        #endregion

        #region Generations

        private void GenerateEdges()
        {
            for (int x = -2; x < _edgeXCount-2; x++)
            {
                for (int z = -2; z < _edgeZCount-2; z++)
                {
                    var edge = Instantiate(edgePrefab, new Vector3(x * _edgeSpacing, 0, z * _edgeSpacing),
                        Quaternion.identity, _edgeParent);
                    
                    edge.transform.name = $"Edge {x} {z}";
                    edge.SetCoordinate(new Vector2Int(x, z));
                    _edgeList.Add(edge);
                }
            }
            
            SetEdgesNeighbours();
        }

        private void SetEdgesNeighbours()
        {
            foreach (var edge in _edgeList)
            {
                var edgeCoordinate = edge.GetCoordinate();
                var rightEdge = _edgeList.Find(e => e.GetCoordinate() == edgeCoordinate + Vector2Int.right);
                var leftEdge = _edgeList.Find(e => e.GetCoordinate() == edgeCoordinate + Vector2Int.left);
                var topEdge = _edgeList.Find(e => e.GetCoordinate() == edgeCoordinate + Vector2Int.up);
                var bottomEdge = _edgeList.Find(e => e.GetCoordinate() == edgeCoordinate + Vector2Int.down);

                edge._edgeNeighbours = new EdgeNeighbours
                {
                    _rightEdge = rightEdge,
                    _leftEdge = leftEdge,
                    _topEdge = topEdge,
                    _bottomEdge = bottomEdge
                };
                
                if(rightEdge != null)
                    edge._neighbourEdges.Add(rightEdge);
                if(leftEdge != null)
                    edge._neighbourEdges.Add(leftEdge);
                if(topEdge != null)
                    edge._neighbourEdges.Add(topEdge);
                if(bottomEdge != null)
                    edge._neighbourEdges.Add(bottomEdge);
            }
        }

        private void GenerateLines()
        {
            foreach (var edge in _edgeList)
            {
                Vector2Int edgeCoordinate = edge.GetCoordinate();
                
                foreach (var neighbourEdge in edge._neighbourEdges)
                {
                    Vector2Int neighbourEdgeCoordinate = neighbourEdge.GetCoordinate();

                    // Eğer aynı hizada ise
                    if (edgeCoordinate.x == neighbourEdgeCoordinate.x)
                    {
                        GenerateLine(edge, neighbourEdge, LineDirection.Vertical,
                            edgeCoordinate.y < neighbourEdgeCoordinate.y);
                    }
                    
                    // Eğer aynı istikamette ise
                    if (edgeCoordinate.y == neighbourEdgeCoordinate.y)
                    {
                        GenerateLine(edge, neighbourEdge, LineDirection.Horizontal,
                            edgeCoordinate.x < neighbourEdgeCoordinate.x);
                    }
                    
                    neighbourEdge._neighbourEdges.Remove(edge);
                    
                }
            }
        }

        private void GenerateLine(StandardEdge edge1, StandardEdge edge2, LineDirection direction, bool isGreater)
        {
            var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, _edgeParent);
            line._lineDirection = direction;

            if (direction == LineDirection.Horizontal)
            {
                line.transform.localEulerAngles = new Vector3(0, 90, 0);
            }

            line.transform.position = edge1.transform.position;
            
            if (isGreater)
            {
                if(direction == LineDirection.Horizontal)
                    line.transform.position += new Vector3(linePrefab._lineHeight,0,0);
                else
                    line.transform.position += new Vector3(0,0,linePrefab._lineHeight);
            }
            else
            {
                if(direction == LineDirection.Horizontal)
                    line.transform.position -= new Vector3(linePrefab._lineHeight,0,0);
                else
                    line.transform.position -= new Vector3(0,0,linePrefab._lineHeight);
            }
            
            line.SetEdges(new List<StandardEdge> {edge1, edge2});
            _lineList.Add(line);
        }

        #endregion

        #region Destroying Board

        [Button("Destroy Board")]
        private void DestroyBoard()
        {
            DestroyEdges();
            DestroyLines();
        }

        private void DestroyEdges()
        {
            foreach (var edge in _edgeList)
            {
#if UNITY_EDITOR
                DestroyImmediate(edge.gameObject);
#else
                Destroy(edge.gameObject);
#endif
            }

            _edgeList.Clear();
        }

        private void DestroyLines()
        {
            foreach (var line in _lineList)
            {
#if UNITY_EDITOR
                DestroyImmediate(line.gameObject);
#else
                Destroy(line.gameObject);
#endif
            }

            _lineList.Clear();
        }

        #endregion
    }
}