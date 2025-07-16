using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 30f;
    [SerializeField] private float _borderThickness = 20f; // � ��������, ������ ���� � ���� ������
    [SerializeField] private Vector2 _minPosition; //  x, z ���������� �����
    [SerializeField] private Vector2 _maxPosition; //  x, z ���������� �����

    private Vector3 _cursorPosition;

    void Update()
    {
        _cursorPosition = transform.position;

        Vector3 mouse = Input.mousePosition;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (mouse.x <= _borderThickness)
            _cursorPosition.x -= _moveSpeed * Time.deltaTime;

        if (mouse.x >= screenWidth - _borderThickness)
            _cursorPosition.x += _moveSpeed * Time.deltaTime;

        if (mouse.y <= _borderThickness)
            _cursorPosition.z -= _moveSpeed * Time.deltaTime;

        if (mouse.y >= screenHeight - _borderThickness)
            _cursorPosition.z += _moveSpeed * Time.deltaTime;

        _cursorPosition.x = Mathf.Clamp(_cursorPosition.x, _minPosition.x, _maxPosition.x);
        _cursorPosition.z = Mathf.Clamp(_cursorPosition.z, _minPosition.y, _maxPosition.y);

        transform.position = _cursorPosition;
    }
}