using UnityEngine;

public class SpawnArea
{
    [SerializeField] private Vector2 _min = new (-40, -40);
    [SerializeField] private Vector2 _max = new (40, 40);
    [SerializeField] private float _fixedY = 1.5f;

    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(_min.x, _max.x);
        float z = Random.Range(_min.y, _max.y);
        float y = _fixedY;

        return new Vector3(x, y, z);
    }
}