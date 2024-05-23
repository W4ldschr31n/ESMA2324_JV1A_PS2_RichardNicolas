using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScreen : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.Play("FadeInScreen");
    }

    public void FadeOut()
    {
        animator.Play("FadeOutScreen");
    }
}
