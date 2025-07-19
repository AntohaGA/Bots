using System;
using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;

    public bool isBusy = false;

    public event Action Released;

    public void Init(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    public void GoForBox(Box box, Base homeBase)
    {
        isBusy = true;
        StartCoroutine(MoveToResource(box, homeBase));
    }

    private IEnumerator MoveToResource(Box box, Base homeBase)
    {
        while (box && Vector3.Distance(transform.position, box.transform.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, box.transform.position, _speed * Time.deltaTime);

            yield return null;
        }

        Take(box);

        while (Vector3.Distance(transform.position, homeBase.transform.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, homeBase.transform.position, _speed * Time.deltaTime);

            yield return null;
        }

        Drop(box);

        homeBase.TakeBox(this, box);
        Released?.Invoke();
        isBusy = false;
    }

    public void Take(Box box)
    {
        box.IsTaken = true;
        box.GetComponent<Rigidbody>().isKinematic = true;
        box.transform.SetParent(transform);
    }

    public void Drop(Box box)
    {
        box.IsTaken = false;
        box.transform.SetParent(null);
        box.GetComponent<Rigidbody>().isKinematic = false;
    }
}