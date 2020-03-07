using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{

    public GameObject eye;

    private void Start()
    {
        LeanTween.scaleX(gameObject, 0.9f, 0.5f).setEaseInOutSine().setLoopPingPong();
        LeanTween.scaleY(eye, 0.1f, 0.5f).setEaseInOutSine().setLoopPingPong();
    }
}
