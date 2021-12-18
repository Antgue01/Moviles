using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoxAnimController : MonoBehaviour
{
    public void grow()
    {
        _animator.Play("Grow");
    }
    public void growAndShrink()
    {
        _animator.Play("GrowAndShrink");
    }

    [SerializeField]
    Animator _animator;
}
