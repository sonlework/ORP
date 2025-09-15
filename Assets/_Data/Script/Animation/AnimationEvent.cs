using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace ActionPlatformerKit
{
    public class AnimationEvent : MonoBehaviour
    {
        public UnityEvent OnAnimationAction { get; set; } = new UnityEvent();
        public UnityEvent OnAnimationEnd { get; set; } = new UnityEvent();

        [SerializeField]
        public Animator animator;
        private bool isAnimationPaused = false;

        private float savedFrame;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
       
      
        // This method pauses the animation by setting the animator's speed to 0 if the animation is not already paused
        public void PauseAnimation()
        {
            if (!isAnimationPaused)
            {
                animator.speed = 0f;
                isAnimationPaused = true;
            }
        }

        // This method resumes the animation by setting the animator's speed to 1 if the animation is paused
        public void ResumeAnimation()
        {
            if (isAnimationPaused)
            {
                animator.speed = 1f;
                isAnimationPaused = false;
            }
        }

        // This method removes all listeners from the OnAnimationAction and OnAnimationEnd events
        public void ResetEvents()
        {
            OnAnimationAction.RemoveAllListeners();
            OnAnimationEnd.RemoveAllListeners();
        }

        // This method invokes the OnAnimationAction event if it is not null
        public void InvokeAnimationAction()
        {
            OnAnimationAction?.Invoke();
        }

        // This method invokes the OnAnimationEnd event, if it is not null
        public void InvokeAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
        // This method stops the animator component
        public void StopAnimation()
        {
            animator.enabled = false;
        }

        // This method starts the animator component
        public void StartAnimation()
        {
            animator.enabled = true;
        }

        // This method plays the animation with the given name and frame
        private void Play(string name, float frame)
        {
            animator.Play(name, -1, frame);
        }

        // This method returns the current frame of the animation
        public float GetCurrentFrame()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        // This method saves the current frame of the animation
        public void SaveCurrentFrame()
        {
            savedFrame = GetCurrentFrame();
        }

        // This method returns the saved frame of the animation
        public float GetSavedFrame()
        {
            return savedFrame;
        }

        // This method compares the given animation name with the current animation name
        public bool CompareWithCurrentAnimation(string animation)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(animation);
        }
       
    }

}
