using UnityEngine;
using DG.Tweening;

public class Passenger : MonoBehaviour
{
    private readonly string animHash_Walk = "Walking";
    private readonly string animHash_Idle = "Idle";
    [SerializeField] private GameObject _mesh;
    [SerializeField] private Animator _anim;

    public PassengersDatas datas;
    public bool IsAvailable { get; private set; } = true;

    public void Spawn(Transform waitPos)
    {
        //Spawn on wait pos
        transform.position = waitPos.position;
        transform.rotation = waitPos.rotation;
        IsAvailable = false;
        IdleAnim();
    }

    public void GetOnTheBus(Transform entrancePoint)
    {
        //Look to bus door and walk through it
        transform.DOLookAt(entrancePoint.position, 0.1f,AxisConstraint.Y);
        transform.DOMove(entrancePoint.position, datas.Speed).SetEase(Ease.Linear).OnComplete(() => _mesh.SetActive(false));
        WalkAnim();
    }

    public void GetOffTheBus(Transform exitPoint,float time)
    {
        //If is there any tweens which is not done yet
        transform.DOKill();
        //Set mesh active and walk through forward
        _mesh.SetActive(true);
        WalkAnim();
        transform.position = exitPoint.position;
        //To make spawnable again
        IsAvailable = true;
        transform.DOLookAt(exitPoint.position + exitPoint.right * 5,0.1f,AxisConstraint.Y);
        transform.DOMove(exitPoint.position + exitPoint.right * 5, datas.Speed).SetEase(Ease.Linear).OnComplete(()=> gameObject.SetActive(false));
        GiveMoneyToPlayer(time);
    }

    /// <summary>
    /// If player arrive to station at time players get a prize else player pays a cost
    /// </summary>
    /// <param name="time"></param>
    private void GiveMoneyToPlayer(float time)
    {
        if (time <= 0)
        {
            PlayerData.MoneyChangeEvent.Invoke(datas.Cost);
            ParticleContainer.PlayParticlesAtPositionEvent.Invoke(ParticleNames.LoseMoneyParticle, transform.position);
        }

        else
        {
            PlayerData.MoneyChangeEvent.Invoke(datas.Prize);
            ParticleContainer.PlayParticlesAtPositionEvent.Invoke(ParticleNames.MoneyParticle, transform.position);
        }
    }

    //Anim functions
    private void IdleAnim()
    {
        _anim.SetBool(animHash_Walk, false);
        _anim.SetBool(animHash_Idle, true);
    }
    private void WalkAnim()
    {
        _anim.SetBool(animHash_Walk, true);
        _anim.SetBool(animHash_Idle, false);
    }
}
