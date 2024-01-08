using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public Canvas canvas;
    public AudioClip correct;
    public AudioClip incorrect;
    public AudioClip pageTurn;

    public float gameTime;
    public float reward;
    public float penalty;
    public Slider timeSlider;

    public List<GameObject> gifts;
    public List<GiftMaterials> giftMaterials;
    public List<Material> bowMaterials;

    public List<TagStuct> tags;

    public Button butt1;
    public Button butt2;

    public GameObject DuringGameUI;
    public GameObject EndGameUI;
    public TMP_Text EndGameText;

    private List<GameObject> randomizationGiftList = new List<GameObject>();
    private List<GameObject> finalGiftList = new List<GameObject>();

    private List<GiftMaterials> randomizationColorList = new List<GiftMaterials>();

    private List<TagStuct> randomizationTagList = new List<TagStuct>();
    private List<TagStuct> finalTagList = new List<TagStuct>();

    private GameObject[] giftClones = new GameObject[3];
    private GameObject[] tagClones = new GameObject[3];
    private GameObject[] letterClones = new GameObject[3];

    private AudioSource myAudio;
    private int pageIdx;

    private float timeLeft;
    private int score;
    private int totalGifts;
    private void Awake()
    {
        if(instance == null)
            instance = this;

        myAudio = GetComponent<AudioSource>();
        DuringGameUI.SetActive(true);
        EndGameUI.SetActive(false);
    }
    void Start()
    {
        /*
            Begin Random Generation Coroutine
            Generate the random and sets values in place before the game starts
            Possibly do black screen for a couple seconds to allow the game to process and begin
         */
        timeLeft = gameTime;

        StartCoroutine("GeneratePresents");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        timeLeft -= Time.deltaTime;
        timeSlider.value = timeLeft / gameTime;

        if(timeLeft <= 0)
        {
            //Lose game
            EndGame();
        }
    }

    IEnumerator GeneratePresents()
    {
        ClearLists();
        RandomizeGifts();
        AssignTags();
        PlaceGifts();
        yield return null;
    }

    private void ClearLists()
    {
        randomizationGiftList.Clear();
        finalGiftList.Clear();
        randomizationTagList.Clear();
        finalTagList.Clear();
        randomizationColorList.Clear();
    }

    private void RandomizeGifts()
    {
        randomizationGiftList = new List<GameObject>(gifts);
        int rand;
        int randColor;
        int randBow;
        //Select 3 random gifts
        for(int i = 0; i < 3; i++ )
        {
            rand = UnityEngine.Random.Range(0, randomizationGiftList.Count);
            //Add to final gift list and remove from randomization list
            finalGiftList.Add(randomizationGiftList[rand]);
            randomizationGiftList.RemoveAt(rand);
        }
        foreach(GameObject gift in finalGiftList)
        {
            randomizationColorList = new List<GiftMaterials>(giftMaterials);
            
            //Randomize color
            randColor = UnityEngine.Random.Range(0, randomizationColorList.Count);
            gift.GetComponent<GiftScript>().color = randomizationColorList[randColor].color;
            gift.GetComponent<GiftScript>().giftMaterial = randomizationColorList[randColor].material;
            randomizationColorList.RemoveAt(randColor);

            //Randomize bow
            randBow = UnityEngine.Random.Range(0, bowMaterials.Count);
            gift.GetComponent<GiftScript>().bowMaterial = bowMaterials[randBow];
            gift.GetComponent<GiftScript>().LoadColor();
        }
    }

    private void PlaceGifts()
    {
        Shuffle();
        giftClones[0] = Instantiate(finalGiftList[0], new Vector3(-11.51f, 0f, -0.76f), Quaternion.identity);
        giftClones[1] = Instantiate(finalGiftList[1], new Vector3(9.09f, 0f, 0.42f), Quaternion.identity);
        giftClones[2] = Instantiate(finalGiftList[2], new Vector3(-9.57f, 0f, -13.97f), Quaternion.identity);

        totalGifts += 3;
    }

    private void AssignTags()
    {
        randomizationTagList = new List<TagStuct>(tags);
        int rand;

        //Select 3 random tags
        for (int i = 0; i < 3; i++)
        {
            rand = UnityEngine.Random.Range(0, randomizationTagList.Count);
            //Add to final gift list and remove from randomization list
            finalTagList.Add(randomizationTagList[rand]);
            randomizationTagList.RemoveAt(rand);

            RandomizeLetter(i);

            //Assign correct tag. Must instantiate first becase prefab =/= clone of prefab
            GameObject clone = Instantiate(finalTagList[i].tag, new Vector3(1f + 7f * i, 0f, -13f), Quaternion.Euler(0f, 180f, 0f));
            finalGiftList[i].GetComponent<GiftScript>().correctTag = clone;
            tagClones[i] = clone;
        }

        pageIdx = 0;
        butt1.interactable = false;
        butt2.interactable = true;
    }

    private void RandomizeLetter(int idx)
    {
        //Instantiate
        GameObject letter = Instantiate(finalTagList[idx].letter, canvas.transform);
        letterClones[idx] = letter;
        if(idx > 0)
            letter.SetActive(false);

        //Get text children
        Text[] list = letter.GetComponentsInChildren<Text>();

        //Randomize which line of letter is correct
        int correct = UnityEngine.Random.Range(0, list.Length);

        GiftScript giftScript = finalGiftList[idx].GetComponent<GiftScript>();
        //Loop through list and add gift names
        for (int i = 0;i < list.Length;i++)
        {
            
            if (giftScript.isColorGift)
            {
                if(i == correct)
                {
                    list[i].text = giftScript.color + " " + giftScript.giftName;
                }
                else
                {
                    GiftScript randomGift = GetRandomLeftoverGift();
                    list[i].text = giftScript.color + " " + randomGift.giftName;
                }

                //Change color of text
                list[i].color = giftScript.giftMaterial.color;
            }
            else
            {
                if (i == correct)
                {
                    list[i].text = giftScript.giftName;
                }
                else
                {
                    GiftScript randomGift = GetRandomLeftoverGift();
                    list[i].text = randomGift.giftName;
                }
            }
        }
    }

    private GiftScript GetRandomLeftoverGift()
    {
        //Return a random gift script then delete it from randomization list
        int rand = UnityEngine.Random.Range(0, randomizationGiftList.Count);
        GiftScript giftScript = randomizationGiftList[rand].GetComponent<GiftScript>();
        randomizationGiftList.RemoveAt(rand);
        return giftScript;
    }

    private void Shuffle()
    {
        //Found this online
        int n = finalGiftList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject value = finalGiftList[k];
            finalGiftList[k] = finalGiftList[n];
            finalGiftList[n] = value;
        }
    }

    public void CheckCorrect()
    {
        StartCoroutine("EndRound");
    }

    public void DestroyTags()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(tagClones[i]);
            Destroy(letterClones[i]);
        }
    }

    IEnumerator EndRound()
    {
        //Check each gift to see if it is correct
        foreach (GameObject gift in giftClones)
        {
            gift.GetComponent<Animator>().SetTrigger("Shake");
            if (gift.GetComponent<GiftScript>().IsCorrect())
            {
                myAudio.clip = correct;
                timeLeft += reward;
                score++;
            }
            else
            {
                myAudio.clip = incorrect;
                timeLeft -= penalty;
            }
            myAudio.Play();
            yield return new WaitForSeconds(0.5f);
        }

        //gifts go bye bye
        for(int i = 0;i < giftClones.Length;i++)
            giftClones[i].GetComponent<GiftScript>().MakeLikeATree();

        yield return new WaitForSeconds(2);

        DestroyTags();

        //Start again
        StartCoroutine(GeneratePresents());
    }

    public void ProgressPage()
    {
            letterClones[pageIdx].SetActive(false);
            letterClones[++pageIdx].SetActive(true);

            if (pageIdx + 2 > letterClones.Length)
                butt2.interactable = false;
            else
                butt1.interactable = true;

            myAudio.clip = pageTurn;
            myAudio.Play();
    }

    public void RegressPage()
    {
            letterClones[pageIdx].SetActive(false);
            letterClones[--pageIdx].SetActive(true);

            if (pageIdx - 1 < 0)
                butt1.interactable = false;
            else
                butt2.interactable = true;

            myAudio.clip = pageTurn;
        myAudio.Play();
    }

    private void EndGame()
    {
        string endText = "Congratulations! You helped Santa sort " + score + " / " + totalGifts + " presents correctly";
        DuringGameUI.SetActive(false);
        EndGameUI.SetActive(true);
        EndGameText.SetText(endText);
    }
    
}

//Creating this since we cant serialize Dictonaries for some reason
[Serializable]
public class GiftMaterials
{
    public string color;
    public Material material;

}

[Serializable]
public class TagStuct
{
    public GameObject tag;
    public GameObject letter;
}
