using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GirftScript : MonoBehaviour
{
    public GameObject currentTag;

    [SerializeField] private GameObject correctTag;
    private Animator myAnim;
    private AudioSource myAudio;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsCorrect()
    {
        if(correctTag == currentTag)
        {
            return true;
        }
         return false;
    }

    public void PlayAudio()
    {
        myAudio.Play();
    }

    public void ShakeAnim()
    {
        myAnim.SetTrigger("Shake");
    }
}
