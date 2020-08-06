using UnityEngine;

namespace XREngine.Core.Scripts.Common
{
    [RequireComponent(typeof(LineRenderer))]
    public class Bezier : MonoBehaviour
    {
        public Vector3 EndPoint { get; private set; }
        
        public bool endPointDetected;
        
        [SerializeField] private LayerMask teleportLayer;

        private Transform _handTransform;
        
        
        private Material _availableMaterial;
        private Material _unavailableMaterial;
        
        // private float _extensionFactor;
        private Vector3[] _controlPoints;
        private LineRenderer _lineRenderer;
        private float _extendStep;
        private const int SegmentCount = 50;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
        }

        private void Start()
        {
            _controlPoints = new Vector3[3];
            _extendStep = 5f;
        }

        private void Update()
        {
            if (!_handTransform) return;
            
            UpdateControlPoints();
            DrawCurve();
        }

        public void Initialize(Transform handTransform, Material availableMat, Material unavailableMat)
        {
            _availableMaterial = availableMat;
            _unavailableMaterial = unavailableMat;
            
            _handTransform = handTransform;
            
            AvailableMaterial();
        }

        public void ToggleDraw(bool draw)
        {
            if (!_lineRenderer) _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.enabled = draw;
        }

        public void AvailableMaterial()
        {
            _lineRenderer.material = _availableMaterial;
        }

        public void UnavailableMaterial()
        {
            _lineRenderer.material = _unavailableMaterial;
        }
        
        // The first control is the remote. The second is a forward projection. The third is a forward and downward projection.
        private void UpdateControlPoints()
        {
            _controlPoints[0] = _handTransform.position; // Get Hand Position
            
            var forward = _handTransform.forward;
            
            _controlPoints[1] = _controlPoints[0] + (forward * (_extendStep * 2f) / 5f);
            _controlPoints[2] = _controlPoints[1] + (forward * (_extendStep * 3f) / 5f) + Vector3.up * -1f;
        }


        // Draw the bezier curve.
        private void DrawCurve()
        {
            if (!_lineRenderer.enabled)
                return;
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, _controlPoints[0]);

            Vector3 prevPosition = _controlPoints[0];
            Vector3 nextPosition = prevPosition;
            for (int i = 1; i <= SegmentCount; i++)
            {
                float t = i / (float)SegmentCount;
                _lineRenderer.positionCount = i + 1;

                if (i == SegmentCount)
                { // For the last point, project out the curve two more meters.
                    Vector3 endDirection = Vector3.Normalize(prevPosition - _lineRenderer.GetPosition(i - 2));
                    nextPosition = prevPosition + endDirection * 2f;
                }
                else
                {
                    nextPosition = CalculateBezierPoint(t, _controlPoints[0], _controlPoints[1], _controlPoints[2]);
                }

                if (CheckColliderIntersection(prevPosition, nextPosition))
                { // If the segment intersects a surface, draw the point and return.
                    _lineRenderer.SetPosition(i, EndPoint);
                    endPointDetected = true;
                    return;
                }
                else
                { // If the point does not intersect, continue to draw the curve.
                    _lineRenderer.SetPosition(i, nextPosition);
                    endPointDetected = false;
                    prevPosition = nextPosition;
                }
            }
        }

        // Check if the line between start and end intersect a collider.
        private bool CheckColliderIntersection(Vector3 start, Vector3 end)
        {
            var r = new Ray(start, end - start);
            
            if (Physics.Raycast(r, out var hit, Vector3.Distance(start, end), teleportLayer))
            {
                EndPoint = hit.point;
                return true;
            }

            return false;
        }

        private static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            return
                Mathf.Pow((1f - t), 2) * p0 +
                2f * (1f - t) * t * p1 +
                Mathf.Pow(t, 2) * p2;
        }
    }
}