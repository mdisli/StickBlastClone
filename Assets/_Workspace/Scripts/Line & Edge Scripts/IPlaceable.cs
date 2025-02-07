using _Workspace.Scripts.Shape_Scripts;

namespace _Workspace.Scripts.Line___Edge_Scripts
{
    public interface IPlaceable
    {
        public bool CheckShapePieceCanPlace(ShapePiece shapeToPlace);
        public void Place(BaseShape placedShape);
        public BaseShape GetPlacedShape();
        public bool IsAvailable();
        public LineDirection GetDirection();

        public int GetLineIndex();
        public void OnPlaceableEnter();
        public void OnPlaceableExit();

    }
}