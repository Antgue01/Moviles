using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoxAnimController : MonoBehaviour
{
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void grow()
    {
        _animator.Play("Grow");
    }
    public void growAndShrink()
    {
        _animator.Play("GrowAndShrink");
    }
    Animator _animator;
}
