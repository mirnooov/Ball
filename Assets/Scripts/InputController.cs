using UnityEngine;

public class InputController : MonoBehaviour
{
    
    [SerializeField] private Ball _ball;
    [SerializeField] private LineRenderer _lineRenderer;
    
    [SerializeField] private float minDistanse = 1;
    [SerializeField] private float maxDistanse = 10;
    
    private float _distance;
    private bool _needCalculateDirection;
    
    private Vector3 _direction;
    private Vector3 _tempDirection;
    private Vector3 _startMousePosition;

    private Vector3 CurrentMousePosition => (Input.mousePosition);
    public Vector3 Direction => new Vector3(_direction.x, 0, _direction.y);
    public float Distance => _distance; 
    
    private void Start()
    {
        _lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        CheckInput();

        if (_needCalculateDirection)
        {
            _tempDirection = (CurrentMousePosition - _startMousePosition).normalized;
            _distance = Vector3.Distance(CurrentMousePosition, _startMousePosition) / 100f;
        }

        var startLinePosition = _ball.transform.position;
        var endLinePosition = new Vector3(startLinePosition.x, startLinePosition.z, 0) - _tempDirection * _distance;
        
        DrawLine(startLinePosition, endLinePosition);

    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _needCalculateDirection = true;
            _startMousePosition = Input.mousePosition;
            _lineRenderer.enabled = true;
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.enabled = false;

            if (_distance > minDistanse && _distance < maxDistanse)
            {
                _needCalculateDirection = false;
            }

            if (_distance > maxDistanse)
            {
                _distance = maxDistanse;
                _needCalculateDirection = false;
            }

            _ball.CalculateForce();
            
            _direction = _tempDirection;
            _tempDirection = Vector3.zero;
        }
    }

    private void DrawLine(Vector3 startLinePosition, Vector3 endLinePosition)
    {
        _lineRenderer.SetPosition(0, new Vector3(startLinePosition.x, 0, startLinePosition.z));
        _lineRenderer.SetPosition(1, new Vector3(endLinePosition.x, 0, endLinePosition.y));
    }

}
    