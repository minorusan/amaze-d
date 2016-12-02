using UnityEngine;
using CnControls;

// This is merely an example, it's for an example purpose only
// Your game WILL require a custom controller scripts, there's just no generic character control systems,
// they at least depend on the animations

public class ThidPersonExampleController : MonoBehaviour
{
    public float MovementSpeed = 0.3f;
    private Animator _animator;
    private Transform _mainCameraTransform;
    private Transform _transform;
    private CharacterController _characterController;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _mainCameraTransform = Camera.main.GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
    }

    public void Update()
    {
        // Just use CnInputManager. instead of Input. and you're good to go
        var inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));
        Vector3 movementVector = Vector3.zero;

        // If we have some input
        _animator.SetFloat("Speed", 0.0f);
        if (inputVector.sqrMagnitude > 0.001f)
        {
            movementVector = _mainCameraTransform.TransformDirection(inputVector);
            movementVector *= MovementSpeed;
            movementVector.y = 0f;
            _animator.SetFloat("Speed", movementVector.sqrMagnitude * MovementSpeed);
            movementVector.Normalize();
            _transform.forward = Vector3.MoveTowards(movementVector, _transform.forward, Time.deltaTime);
        }

        if (transform.position.y > 0)
        {
            movementVector += Physics.gravity;
        }
       
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementVector, Time.deltaTime * MovementSpeed);
    }
}
