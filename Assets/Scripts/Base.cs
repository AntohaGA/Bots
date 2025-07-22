using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Bot _bot;
    [SerializeField] private PoolBoxes _poolResources;
    [SerializeField] private PoolBots _poolBots;
    [SerializeField] private float scanInterval;

    private int _countMaxBots = 3;
    private int _countBoxes = 0;

    private void Awake()
    {
        _poolBots.Init(_bot);
    }

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(scanInterval);

            Box box = FindNearestBox();

            if (box != null)
            {
                AssignBots(box);
            }
        }
    }

    public void TakeBox(Bot bot, Box box)
    {
        _countBoxes++;
        Debug.Log("box count" + _countBoxes);
        _poolResources.ReturnInstance(box);
        _poolBots.ReturnInstance(bot);
    }

    private void AssignBots(Box box)
    {
        Bot bot = GetBot();

        if (bot != null)
        {
            bot.Init(transform.position, this);
            box.IsTaken = true;
            bot.BringBox(box);

            return;
        }
    }

    private Box FindNearestBox()
    {
        Box closestBox = null;
        float minDist = float.MaxValue;

        foreach (Box box in _poolResources)
        {
            if (box.IsTaken == false)
            {
                float distant = Vector3.Distance(transform.position, box.transform.position);

                if (distant < minDist)
                {
                    closestBox = box;
                    minDist = distant;
                }
            }
        }

        return closestBox;
    }

    private Bot GetBot()
    {
        if (_poolBots.ActiveCount < _countMaxBots)
        {
            return _poolBots.GetInstance();
        }

        return null;
    }
}