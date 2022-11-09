using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private ParticleController _damageDust;

    private Vector3 _currentDirection;
    private Vector3 _directionAfterCollision;
    private Vector3 _collisionNormal;
    
    private float _speed;
    private bool _collision;
    
    private float MaxDistanceDelta => _speed * Time.deltaTime;
    
    private void Update()
    {
        if (!_collision)
            SetDirectionFromInput(-_input.Direction);

        Move();
    }

    public void CalculateForce()
    {
        _speed = 3 * _input.Distance;
        
        if (_speed > 15)
        {
            var dust = Instantiate(_damageDust, transform.position, transform.rotation);
            dust.PlayParticles();
        }
        _collision = false;
    }
    
    private void Move()
    {
        Vector3 targetPosition = Vector3.zero;
        
        if (_collision)
        {
            var _directionReflect = Vector3.Reflect(_currentDirection.normalized, _collisionNormal);
            _directionAfterCollision = _directionReflect;
            targetPosition = transform.position + _directionAfterCollision;
        }
        else
        {
            targetPosition = transform.position + _currentDirection;
        }
        
        _speed = Mathf.Lerp(_speed, 0, Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MaxDistanceDelta);
        Rotate();
    }

    private void SetDirectionFromInput(Vector3 direction)
    {
        _currentDirection = direction;
        _directionAfterCollision = Vector3.zero;
    }
    
    private void Rotate()
    {
        float circumference = Mathf.PI;
        float angleDelta = MaxDistanceDelta / circumference * 360;
        Vector3 rotationAxis = Vector3.zero;
       
        if(_directionAfterCollision == Vector3.zero)
            rotationAxis = Quaternion.LookRotation(_currentDirection.normalized) * Vector3.right;
        else
            rotationAxis = Quaternion.LookRotation(_directionAfterCollision.normalized) * Vector3.right;

        transform.Rotate(rotationAxis, angleDelta, Space.World);
    }
    
    private void DeformBall(Vector3 contactPoint)
    {
        var mesh = GetComponent<MeshRenderer>();
        MeshDeformer deformer = GetComponent<MeshDeformer>();
        
        deformer.AddDeformingForce(contactPoint + _collisionNormal, 100);
        mesh.material.color = Color.red;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var wall = collision.gameObject.GetComponent<Wall>();
        
        if (wall != null)
        {
            var contact = collision.contacts[0];
            _collisionNormal = (contact.normal);
            _collision = true;
            
            if (_directionAfterCollision != Vector3.zero)
                _currentDirection = _directionAfterCollision;
            
            if (_speed > 10)
                DeformBall(contact.point);
            
            Instantiate(_damageDust,contact.point, collision.transform.rotation);
            _damageDust.PlayParticles();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.white;
    }

}