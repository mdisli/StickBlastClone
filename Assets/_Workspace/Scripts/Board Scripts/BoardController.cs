using System.Collections.Generic;
using System.Linq;
using _Workspace.Scripts.Line___Edge_Scripts;
using _Workspace.Scripts.SO_Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _Workspace.Scripts.Board_Scripts
{
    public class BoardController : MonoBehaviour
    {
        #region Variables

        [Header("SO References")] [SerializeField]
        private BoardEventSO _boardEventSO;

        [Header("Prefabs")] 
        [SerializeField] private BaseLine linePrefab;
        [SerializeField] private StandardEdge edgePrefab;
        [SerializeField] private FilledSquare squarePrefab;

        [Header("Grid Settings")] [SerializeField]
        private int _edgeXCount;

        [SerializeField] private int _edgeZCount;
        [SerializeField] private float _edgeSpacing;
        [SerializeField] private Transform _edgeParent;

        [Header("Lists")] 
        [SerializeField] private List<StandardEdge> _edgeList = new List<StandardEdge>();
        [SerializeField] private List<BaseLine> _lineList = new List<BaseLine>();
        [SerializeField] private List<FilledSquare> _squareList = new List<FilledSquare>();

        #endregion

        #region Unity Funcs

        private void OnEnable()
        {
            _boardEventSO.OnShapePlaced += BoardEventSo_OnShapePlaced;
        }

        private void OnDisable()
        {
            _boardEventSO.OnShapePlaced -= BoardEventSo_OnShapePlaced;
        }

        #endregion

        #region Callbakcs

        private void BoardEventSo_OnShapePlaced()
        {
            CheckForCompletedSquare();
        }

        #endregion

        #region Edge Funcs

        private StandardEdge GetEdgeWithCoordinate(Vector2Int coordinate)
        {
            return _edgeList.FirstOrDefault(edge => edge.GetCoordinate() == coordinate);
        }

        #endregion

        #region Square Detection

        private void CheckForCompletedSquare()
        {
            // Each edge needs to check if it forms part of a square
            foreach (var startEdge in _edgeList)
            {
                foreach (var connectedEdge in startEdge.connectedEdgesList)
                {
                    // Get the coordinates of both edges
                    Vector2Int pos1 = startEdge.GetCoordinate();
                    Vector2Int pos2 = connectedEdge.GetCoordinate();

                    // Determine if this is a horizontal or vertical connection
                    bool isHorizontal = pos1.y == pos2.y;

                    if (isHorizontal)
                    {
                        // Check for square above
                        Vector2Int topLeft = new Vector2Int(Mathf.Min(pos1.x, pos2.x), pos1.y + 1);
                        Vector2Int topRight = new Vector2Int(Mathf.Max(pos1.x, pos2.x), pos1.y + 1);

                        if (DoesEdgeExistAt(topLeft) && 
                            DoesEdgeExistAt(topRight) &&
                            AreEdgesConnected(topLeft, topRight) && 
                            (
                            (AreEdgesConnected(pos1, topLeft) || AreEdgesConnected(pos1, topRight)) && 
                             (AreEdgesConnected(pos2, topLeft) || AreEdgesConnected(pos2, topRight))
                            )
                            )
                        {
                            FillSquare(pos1, pos2, topLeft, topRight);
                        }

                        // Check for square below
                        Vector2Int bottomLeft = new Vector2Int(Mathf.Min(pos1.x, pos2.x), pos1.y - 1);
                        Vector2Int bottomRight = new Vector2Int(Mathf.Max(pos1.x, pos2.x), pos1.y - 1);

                        if (DoesEdgeExistAt(bottomLeft) &&
                            DoesEdgeExistAt(bottomRight) &&
                            AreEdgesConnected(bottomLeft, bottomRight) && 
                            ((AreEdgesConnected(pos1, bottomLeft) || AreEdgesConnected(pos1, bottomRight)) && 
                             (AreEdgesConnected(pos2, bottomLeft) || AreEdgesConnected(pos2, bottomRight))))
                        {
                            FillSquare(pos1, pos2, bottomLeft, bottomRight);
                        }
                    }
                    else // Vertical connection
                    {
                        // Check for square to the right
                        Vector2Int topRight = new Vector2Int(pos1.x + 1, Mathf.Max(pos1.y, pos2.y));
                        Vector2Int bottomRight = new Vector2Int(pos1.x + 1, Mathf.Min(pos1.y, pos2.y));

                        if (DoesEdgeExistAt(topRight) && 
                            DoesEdgeExistAt(bottomRight) &&
                            AreEdgesConnected(topRight, bottomRight) &&
                            ((AreEdgesConnected(pos1, topRight) || AreEdgesConnected(pos1, bottomRight)) && 
                             (AreEdgesConnected(pos2, topRight) || AreEdgesConnected(pos2, bottomRight))))
                        {
                            FillSquare(pos1, pos2, topRight, bottomRight);
                        }

                        // Check for square to the left
                        Vector2Int topLeft = new Vector2Int(pos1.x - 1, Mathf.Max(pos1.y, pos2.y));
                        Vector2Int bottomLeft = new Vector2Int(pos1.x - 1, Mathf.Min(pos1.y, pos2.y));

                        if (DoesEdgeExistAt(topLeft) && 
                            DoesEdgeExistAt(bottomLeft) &&
                            AreEdgesConnected(topLeft, bottomLeft) &&
                            ((AreEdgesConnected(pos1, topLeft) || AreEdgesConnected(pos1, bottomLeft)) && 
                             (AreEdgesConnected(pos2, topLeft) || AreEdgesConnected(pos2, bottomLeft))))
                        {
                            FillSquare(pos1, pos2, topLeft, bottomLeft);
                        }
                    }
                }
            }
        }

        private bool DoesEdgeExistAt(Vector2Int position)
        {
            var edge = GetEdgeWithCoordinate(position);
            
            return edge != null;
        }

        private bool AreEdgesConnected(Vector2Int pos1, Vector2Int pos2)
        {
            var edge1 = GetEdgeWithCoordinate(pos1);
            var edge2 = GetEdgeWithCoordinate(pos2);
            
            return edge1.connectedEdgesList.Contains(edge2);
        }

        private void FillSquare(Vector2Int edge1, Vector2Int edge2, Vector2Int edge3, Vector2Int edge4)
        {
            var edge1Obj = GetEdgeWithCoordinate(edge1);
            var edge2Obj = GetEdgeWithCoordinate(edge2);
            var edge3Obj = GetEdgeWithCoordinate(edge3);
            var edge4Obj = GetEdgeWithCoordinate(edge4);

            List<StandardEdge> connectedEdges = new List<StandardEdge>
            {
                edge1Obj,
                edge2Obj,
                edge3Obj,
                edge4Obj
            };

            for (var i = 0; i < _squareList.Count; i++)
            {
                var sq = _squareList[i];
                if (sq.CompareEdges(connectedEdges))
                    return;
            }

            FilledSquare square = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity, _edgeParent);
            square.transform.localScale = Vector3.zero;
            
            float coordinateX = (edge1.x + edge2.x + edge3.x + edge4.x) / 4f;
            float coordinateY = (edge1.y + edge2.y + edge3.y + edge4.y) / 4f;
            Vector2 squareCoordinate = new Vector2(coordinateX, coordinateY);
            
            float wpX = (edge1Obj.transform.localPosition.x + edge2Obj.transform.localPosition.x + edge3Obj.transform.localPosition.x + edge4Obj.transform.localPosition.x) / 4f;
            float wpZ = (edge1Obj.transform.localPosition.z + edge2Obj.transform.localPosition.z + edge3Obj.transform.localPosition.z + edge4Obj.transform.localPosition.z) / 4f;
            
            Vector3 worldPosition = new Vector3(wpX, 0, wpZ);
            
            square.GenerateSquare(worldPosition, squareCoordinate);
            
            square.SetEdges(connectedEdges);
            
            _squareList.Add(square);
            
        }

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
            for (int x = -2; x < _edgeXCount - 2; x++)
            {
                for (int z = -2; z < _edgeZCount - 2; z++)
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

                if (rightEdge != null)
                    edge._neighbourEdges.Add(rightEdge);
                if (leftEdge != null)
                    edge._neighbourEdges.Add(leftEdge);
                if (topEdge != null)
                    edge._neighbourEdges.Add(topEdge);
                if (bottomEdge != null)
                    edge._neighbourEdges.Add(bottomEdge);
            }
        }

        private void GenerateLines()
        {
            int index = 0;
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
                            edgeCoordinate.y < neighbourEdgeCoordinate.y, index);
                        index++;
                    }

                    // Eğer aynı istikamette ise
                    if (edgeCoordinate.y == neighbourEdgeCoordinate.y)
                    {
                        GenerateLine(edge, neighbourEdge, LineDirection.Horizontal,
                            edgeCoordinate.x < neighbourEdgeCoordinate.x, index);
                        index++;
                    }

                    neighbourEdge._neighbourEdges.Remove(edge);
                }
            }
        }

        private void GenerateLine(StandardEdge edge1, StandardEdge edge2, LineDirection direction, bool isGreater,
            int index)
        {
            var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, _edgeParent);
            line.SetLineIndex(index);
            line._lineDirection = direction;

            if (direction == LineDirection.Horizontal)
            {
                line.transform.localEulerAngles = new Vector3(0, 90, 0);
            }

            line.transform.position = edge1.transform.position;

            if (isGreater)
            {
                if (direction == LineDirection.Horizontal)
                    line.transform.position += new Vector3(linePrefab._lineHeight, 0, 0);
                else
                    line.transform.position += new Vector3(0, 0, linePrefab._lineHeight);
            }
            else
            {
                if (direction == LineDirection.Horizontal)
                    line.transform.position -= new Vector3(linePrefab._lineHeight, 0, 0);
                else
                    line.transform.position -= new Vector3(0, 0, linePrefab._lineHeight);
            }

            line.SetEdges(new List<StandardEdge> { edge1, edge2 });
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