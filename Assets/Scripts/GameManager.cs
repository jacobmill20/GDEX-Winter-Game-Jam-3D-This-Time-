using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;

    public List<GameObject> gifts;
    //I was gonna use a dictionary for this but apparently they cant be serialized
    public List<GiftMaterials> giftMaterials;
    public List<Material> bowMaterials;

    public List<TagStuct> tags;

    private List<GameObject> randomizationGiftList = new List<GameObject>();
    private List<GameObject> finalGiftList = new List<GameObject>();

    private List<Material> selectedColors = new List<Material>();
    private List<Material> selectedBows = new List<Material>();

    private List<TagStuct> randomizationTagList = new List<TagStuct>();
    private List<TagStuct> finalTagList = new List<TagStuct>();

    private GameObject[] giftClones = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        /*
            Begin Random Generation Coroutine
            Generate the random and sets values in place before the game starts
            Possibly do black screen for a couple seconds to allow the game to process and begin
         */
        GeneratePresents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GeneratePresents()
    {
        /*
         Use random to pick a tag from the list
         Also use random to pick correct color for gift from list
         */

        RandomizeGifts();
        AssignTags();
        PlaceGifts();


        //Start Game

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
            //Randomize color
            randColor = UnityEngine.Random.Range(0, giftMaterials.Count);
            randBow = UnityEngine.Random.Range(0, bowMaterials.Count);
            if (!selectedColors.Contains(giftMaterials[randColor].material)) //we never put anything in selected colors so why the if? this will always be true. Even if it were false it would just skip assigning color entirely for that gift
            {
                gift.GetComponent<GiftScript>().color = giftMaterials[randColor].color;
                gift.GetComponent<GiftScript>().giftMaterial = giftMaterials[randColor].material;
            }
            if (!selectedBows.Contains(bowMaterials[randBow])) //same goes here
            {
                gift.GetComponent<GiftScript>().bowMaterial = bowMaterials[randBow];
            }
            gift.GetComponent<GiftScript>().LoadColor();
        }
    }

    private void PlaceGifts()
    {
        //finalGiftList[0].transform.position = new Vector3(-20f, 0f, -3f);
        //finalGiftList[1].transform.position = new Vector3(0f, 0f, -3f);
        //finalGiftList[2].transform.position = new Vector3(20f, 0f, -3f);
        Shuffle();


        giftClones[0] = Instantiate(finalGiftList[0], new Vector3(-20f, 0f, -3f), Quaternion.identity);
        giftClones[1] = Instantiate(finalGiftList[1], new Vector3(0f, 0f, -3f), Quaternion.identity);
        giftClones[2] = Instantiate(finalGiftList[2], new Vector3(20f, 0f, -3f), Quaternion.identity);

        //I know this part is scuf
        //fixing placement for certain items
        /*if(finalGiftList[0] == gifts[4])
        {
            finalGiftList[0].transform.position = new Vector3(-17f, 0f, -3f);
        }
        foreach(GameObject gift in finalGiftList)
        {
            if(gift == gifts[1])
            {
                gift.transform.Translate(new Vector3(0f, 1.5f, 0f));
            }
        }*/

        //I think if we push one gift down closer to the player we can avoid having to move them around hopefully
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
            GameObject clone = Instantiate(finalTagList[i].tag, new Vector3(-20f + 20f * i, 0f, -10f), Quaternion.Euler(0f, 180f, 0f));
            finalGiftList[i].GetComponent<GiftScript>().correctTag = clone;
        }
    }

    private void RandomizeLetter(int idx)
    {
        //Instantiate
        GameObject letter = Instantiate(finalTagList[idx].letter, canvas.transform);

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
        //Check each gift to see if it is correct
        foreach (GameObject gift in giftClones)
        {
            if (!gift.GetComponent<GiftScript>().IsCorrect())
            {
                Bad();
                return;
            }

        }

        Good();
    }

    private void Bad()
    {
        Debug.Log("Incorrect");
    }

    private void Good()
    {
        Debug.Log("Correct");
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
