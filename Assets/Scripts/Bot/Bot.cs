using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _handHolder;
    private Box _box;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Base _homeBase;
    private Coroutine currentRoutine;
    private bool _isLifted;
    private bool _isLifting;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Init(Vector3 startPosition, Base basePoint)
    {
        transform.SetPositionAndRotation(startPosition, Quaternion.identity);
        _homeBase = basePoint;
    }

    public void BringBox(Box box)
    {
        if (IsBusy) return;
        _box = box;
        IsBusy = true;
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ProcessBringBox());
    }

    private IEnumerator ProcessBringBox()
    {
        _isLifted = false;
        _isLifting = false;

        yield return MoveToTarget(_box.GetSpotForLift());

        yield return SmoothLookAtTarget(_box.transform);

        _animator.SetTrigger("Lift");

        yield return new WaitUntil(() => _isLifting);

        LiftBox();

        yield return new WaitUntil(() => _isLifted);

        yield return GoToBase();

        Drop(_box);

        OnTaskCompleted();
    }

    private IEnumerator GoToBase()
    {
        _animator.SetTrigger("RunWith");
        _agent.SetDestination(_homeBase.transform.position);

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            yield return null;
    }

    private IEnumerator MoveToTarget(Vector3 destination)
    {
        _agent.SetDestination(destination);
        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance || _agent.velocity.sqrMagnitude > 0f)
            yield return null;
    }

    private IEnumerator SmoothLookAtTarget(Transform target)
    {
        float duration = 0.5f;

        _agent.isStopped = true;
        _agent.updateRotation = false;

        Quaternion startRot = transform.rotation;
        Vector3 toTarget = (target.position - transform.position).normalized;

        if (toTarget == Vector3.zero)
        {
            toTarget = transform.forward;
        }

        Quaternion endRot = Quaternion.LookRotation(toTarget);
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / duration);
            yield return null;
        }

        transform.rotation = endRot;
        _agent.updateRotation = true;
        _agent.isStopped = false;
    }

    //גûחûגאועסÿ ג  "Lift"
    private void OnLifting()
    {
        _isLifting = true;       
    }

    //גûחûגאועסÿ ג Lift"
    private void OnLifted()
    {
        _isLifted = true;
    }

    private void LiftBox()
    {
        _box.IsTaken = true;
        Rigidbody boxRb = _box.GetComponent<Rigidbody>();
        boxRb.isKinematic = true;
        _box.transform.SetParent(_handHolder);
        _box.transform.localPosition = Vector3.zero;
        var obstacle = _box.GetComponent<NavMeshObstacle>();

        if (obstacle)
        {
            obstacle.enabled = false;
        }
    }

    private void Drop(Box box)
    {
        box.IsTaken = false;
        box.transform.SetParent(null);
        Rigidbody boxRb = box.GetComponent<Rigidbody>();
        boxRb.isKinematic = false;
        _homeBase.TakeBox(box);
    }

    private void OnTaskCompleted()
    {
        _homeBase.TakeBot(this);
        IsBusy = false;
        currentRoutine = null;
    }
}