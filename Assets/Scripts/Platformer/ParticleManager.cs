using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(AudioSource))]
public class ParticleManager : MonoBehaviour
{
   
    [ReorderableList]
    public List<ParticleSystem> particleSystems;
    [ReorderableList]
    public List<AudioClip> audioEffects;

    private AudioSource audioSource;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAnEffect(PlayerState playerState) {
        switch (playerState)
        {
            case PlayerState.Walk:
                particleSystems[0].Play();
                break;
            case PlayerState.Jump:
                particleSystems[1].Play();
                break;
            case PlayerState.MidAir:
                particleSystems[2].Play();
                break;
            case PlayerState.Land:
                particleSystems[3].Play();
                break;
            default:
                break;
        }
    }

    public void InstatiateEffect(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Walk:
                Instantiate(particleSystems[0], transform.position, Quaternion.identity);
                break;
            case PlayerState.Jump:
                Instantiate(particleSystems[1], transform.position, Quaternion.identity);
                PlayAShot(1);
                break;
            case PlayerState.MidAir:
                Instantiate(particleSystems[2], transform.position, Quaternion.identity);
                break;
            case PlayerState.Land:
                Instantiate(particleSystems[3], transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    public void PlayAShot(int clipIndex) {
        audioSource.PlayOneShot(audioEffects[clipIndex]);
    }


}
