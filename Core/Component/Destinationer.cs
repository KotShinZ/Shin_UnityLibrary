using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Destinationer : MonoBehaviour
{
    [TitleDescription] public string title = "�ړI�n�܂œ��B������";

    [Space]
    public Rigidbody _rigidbody;
    //public RaySensor raySensor;

    [Space]
    [Label("�ړI�n�Ɉړ����邩�ǂ���")]public bool isMove = false;
    [Label("�ړI�n��Transform(null����Vector3�ɂȂ�)")] public Transform destinationTransform;
    [Label("�ړI�n�̍��W(Transform���D��)")]public Vector3 destination;
    [Label("����")]public float speed;
    [Label("�ړ����n�߂�ŏ�����")] public float minDistance = -1;
    [Label("���̌��ɗ����Ȃ����ǂ���")] public bool isDetectFloor = false;
    [Label("�����ƌ��߂�ŏ�����")] public float stopThreshold = 1;

    [Space]
    [Readonly] public bool isArrivaled = false;
    [Readonly] public bool isStoping = false;

    private float stopTime;
    private Vector3 stopedPosition = default;
    

    private void Awake()
    {
        stopTime = 0;
    }

    void Update()
    {
        /*var vec = destination;
        vec.y = transform.position.y;
        transform.LookAt(vec);

        isArrivaled = isArrival();
        isStoping = isStopping();

        if (isMove && !isArrivaled)
        {
            if (raySensor != null && isDetectFloor && raySensor.DetectedObjects.Count != 0)
            {
                _rigidbody.MovePosition(transform.forward * speed * Time.deltaTime + transform.position);
            }
        }*/
    }

    public void SetDestination(Vector3 _destination)
    {
        destination = _destination;
        isMove = true;
    }
    public void SetDestination(Transform _destination)
    {
        destination = _destination.position;
        isMove = true;
    }

    /// <summary>
    /// �������Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    bool isArrival()
    {
        return Vector3.Distance(transform.position, destination) < 0.3f;
    }

    /// <summary>
    /// �~�܂��Ă��邩�ǂ���
    /// </summary>
    bool isStopping()
    {
        if(Vector3.Distance(stopedPosition, transform.position) < 0.3f) //�~�܂��Ă���Ȃ�
        {
            stopedPosition = transform.position;
            stopTime += Time.deltaTime;
        }
        else { stopTime = 0; }

        if(stopTime > stopThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �p�g���[������������
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask RamdomPatrolTask(Vector3 center , float range , CancellationToken token)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();

            SetDestinationRandom(center, range);

            await UniTask.WaitUntil(()=> isStoping == true);
        }
    }
    public async UniTask RamdomPatrolTask(List<Transform> points, CancellationToken token = default, bool isRandom = true)
    {
        var nowDestinationPoint = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();

            if(isRandom == true)
            {
                destination = points.GetRandomInList().position;
            }
            else
            {
                destination = points[nowDestinationPoint].position;
                nowDestinationPoint += 1;
                if(nowDestinationPoint >= points.Count) nowDestinationPoint = 0;
            }

            await UniTask.WaitUntil(()=> isStoping == true);
        }
    }

    /// <summary>
    /// �͈͓��̃����_���ȏꏊ�Ɉړ�
    /// </summary>
    void SetDestinationRandom(Vector3 center, float range)
    {
        destination = new Vector3(range, 0, range) * (Random.value - 0.5f) + center;
    }

    public void SetMoveActive(bool active)
    {
        isMove = active;
    }
}
