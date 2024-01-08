using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GiftScript : MonoBehaviour
{
    public GameObject currentTag;
    public GameObject correctTag;
    public bool isColorGift;
    public string giftName;
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

    public void ShakeAnim()
    {
        myAnim.SetTrigger("Shake");
        myAudio.Play();
    }

    public void MakeLikeATree()
    {
        myAnim.SetTrigger("End");
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
