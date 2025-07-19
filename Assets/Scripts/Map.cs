using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Map : MonoBehaviour
{
    private Renderer _renderer;
    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    private void Awake()
    {
        CalculateBounds();
        _renderer = GetComponent<Renderer>();
    }

    private void CalculateBounds()
    {
        if (_renderer != null)
        {
            Bounds bounds = _renderer.bounds;
            _minBounds = new Vector2(bounds.min.x, bounds.min.z);
            _maxBounds = new Vector2(bounds.max.x, bounds.max.z);
        }
    }

    private bool IsPointInside(Vector3 point)
    {
        return point.x >= _minBounds.x && point.x <= _maxBounds.x &&
               point.z >= _minBounds.y && point.z <= _maxBounds.y;
    }
}