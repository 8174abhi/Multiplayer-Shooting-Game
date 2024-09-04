using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public Animator animator;
    int horizontalvalue;
    int Verticalvalue;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        horizontalvalue = Animator.StringToHash("Horizontal");
        Verticalvalue = Animator.StringToHash("Vertical");


    }
    public void ChangeAnimatorValues(float Horizontalmovement, float Verticalmovement, bool issprinting)
    {
        float snappedhorizontal;
        float snappedvertical;
        #region Snapped_Horizontal
        if (Horizontalmovement > 0 && Horizontalmovement < .55f)
        {
            snappedhorizontal = .5f;
        }
        else if (Horizontalmovement > .55f)
        {
            snappedhorizontal = 1f;
        }
        else if (Horizontalmovement < 0 && Horizontalmovement > -.55f)
        {
            snappedhorizontal = -.55f;
        }
        else if (Horizontalmovement < -0.55f)
        {
            snappedhorizontal = -1f;
        }
        else
        {
            snappedhorizontal = 0f;
        }


        #endregion

        #region Snapped_Vertical
        if (Verticalmovement > 0 && Verticalmovement < .55f)
        {
            snappedvertical = .5f;
        }

        else if (Verticalmovement > .55f)
        {
            snappedvertical = 1f;
        }
        else if (Verticalmovement < 0 && Verticalmovement > -.55f)
        {
            snappedvertical = -.55f;
        }
        else if (Verticalmovement < -0.55f)
        {
            snappedvertical = -1f;
        }
        else
        {
            snappedvertical = 0f;
        }


        #endregion
        if (issprinting)
        {
            snappedhorizontal = Horizontalmovement;
            snappedvertical = 2;
        }
        animator.SetFloat(horizontalvalue,/* Horizontalmovement*/snappedhorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(Verticalvalue, /*Verticalmovement*/snappedvertical, 0.1f, Time.deltaTime);
    }
    public void PlayTargetAnim(string Targetanim, bool isIntracting)
    {
        animator.SetBool("isIntracting", isIntracting);
        animator.CrossFade(Targetanim, 0.2f);
    }
}
