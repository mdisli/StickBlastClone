using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Board_Scripts
{
    public class FilledSquare : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Vector2 _coordinate; 

        #endregion

        #region Getter & Setter

        public Vector2 GetCoordinate()
        {
            return _coordinate;
        }

        private void SetCoordinate(Vector2 coordinate)
        {
            _coordinate = coordinate;
        }

        #endregion

        #region Generating

        public void GenerateSquare(Vector3 worldPosition, Vector2 coordinate)
        {
            SetCoordinate(coordinate);
            transform.localPosition = worldPosition;
            transform.localScale = Vector3.zero;
            OpenSquare();
        }
        
        public void OpenSquare()
        {
            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
        }

        #endregion
    }
}