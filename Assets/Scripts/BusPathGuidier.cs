using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BusPathGuidier : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform guide;
    public Transform firstBusStation;
    Vector3 pointPosition;

    private void Start()
    {
        meshAgent.speed = 0;
        SetDestination(firstBusStation.position);
    }

    private void LateUpdate()
    {
        //When bus has a path it calculates navmesh path and look to next pos on path to guide 
        if (meshAgent.hasPath)
        {
            if (meshAgent.path.corners.Length > 0)
                pointPosition = meshAgent.path.corners[1];
            else
                pointPosition = meshAgent.pathEndPosition;

            guide.DOLookAt(pointPosition, 0.1f);
            guide.transform.eulerAngles = Vector3.up * guide.eulerAngles.y;
        }
    }

    public void SetDestination(Vector3 target)
    {
        meshAgent.SetDestination(target);
    }
}
