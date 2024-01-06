using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GiftScript : MonoBehaviour
{
    public GameObject currentTag;
    public GameObject correctTag;
    [HideInInspector] public Material giftMaterial;
    [HideInInspector] public Material bowMaterial;
    [HideInInspector] public string color;

    [SerializeField] List<GameObject> GiftMaterialZones;
    [SerializeField] List<GameObject> BowMaterialZones;


    private Animator myAnim;
    private AudioSource myAudio;
    

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    public void LoadColor()
    {
        foreach (GameObject zone in GiftMaterialZones)
        {
            zone.GetComponent<MeshRenderer>().material = giftMaterial;
        }
        foreach (GameObject zone in BowMaterialZones)
        {
            zone.GetComponent<MeshRenderer>().material = bowMaterial;
        }
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
