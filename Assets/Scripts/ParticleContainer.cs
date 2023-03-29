using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleContainer : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _moneyParticle;
    [SerializeField] private List<ParticleSystem> _loseMoneyParticle;
    private ParticleSystem chosenParticle;

    /// <summary>
    /// Choose a particle name and play it on desired pos
    /// </summary>
    public static UnityEvent<ParticleNames, Vector3> PlayParticlesAtPositionEvent = new UnityEvent<ParticleNames, Vector3>();

    private void OnEnable()
    {
        PlayParticlesAtPositionEvent.AddListener(PlayParticleAtPosition);
    }

    private void OnDisable()
    {
        PlayParticlesAtPositionEvent.RemoveListener(PlayParticleAtPosition);
    }

    private void PlayParticleAtPosition(ParticleNames particleName, Vector3 desiredPos)
    {
        ChooseParticle(particleName);
        chosenParticle.transform.position = desiredPos;
        chosenParticle.Stop();
        chosenParticle.Play();
    }

    /// <summary>
    /// Choose a particle list by ParticleNames enum
    /// </summary>
    /// <param name="particleName"></param>
    private void ChooseParticle(ParticleNames particleName)
    {
        switch(particleName)
        {
            case ParticleNames.MoneyParticle:
                SelectAvailableParticleFromList(_moneyParticle);
                break;

            case ParticleNames.LoseMoneyParticle:
                SelectAvailableParticleFromList(_loseMoneyParticle);
                break;
        }
    }

    /// <summary>
    /// Choose a particle from list which is available
    /// </summary>
    /// <param name="list"></param>
    private void SelectAvailableParticleFromList(List<ParticleSystem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].isPlaying)
            {
                chosenParticle = list[i];
                return;
            }
        }
    }
}

public enum ParticleNames
{
    MoneyParticle,
    LoseMoneyParticle
    //We can add lots of particle here
}
