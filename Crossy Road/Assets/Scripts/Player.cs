using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    private int minZPos;
    private int extent;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    public AudioSource audioSource;
    public AudioClip crackSound;
    public bool IsDie { get => !this.enabled; }

    public void SetUp(int minZPos, int extent){
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    void Update()
    {
        var moveDir= Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow)) moveDir += new Vector3(0,0,1);
        if(Input.GetKey(KeyCode.DownArrow)) moveDir += new Vector3(0,0,-1);
        if(Input.GetKey(KeyCode.LeftArrow)) moveDir += new Vector3(-1,0,0);
        if(Input.GetKey(KeyCode.RightArrow)) moveDir += new Vector3(1,0,0);
        if(moveDir != Vector3.zero && !IsJumping()) Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection){
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));
        if(targetPosition.z <= backBoundary || 
            targetPosition.x <= leftBoundary || 
            targetPosition.x >= rightBoundary ||
            Tree.allPositions.Contains(targetPosition)) return;
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration).OnComplete(UpdateTravel);
    }

    private void UpdateTravel(){
        this.currentTravel = (int) this.transform.position.z;
        if(currentTravel > maxTravel) maxTravel = currentTravel;
        stepText.text = "Score : "+ maxTravel.ToString();
    }

    public bool IsJumping(){
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other){
        if(this.enabled) if(other.tag == "Car") AnimateCrash();
    }

    private void AnimateCrash()
    {
        audioSource.PlayOneShot(crackSound);
        transform.DOScaleY(0.01f,0.2f);
        transform.DOScaleX(3,0.2f);
        transform.DOScaleZ(2,0.2f);
        this.enabled = false;
        dieParticles.Play();
    }
}
