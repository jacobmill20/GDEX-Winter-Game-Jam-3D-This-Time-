using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gifts;
    //I was gonna use a dictionary for this but apparently they cant be serialized
    public List<GiftMaterials> giftMaterials;
    public List<Material> bowMaterials;

    private List<GameObject> randomizationGiftList = new List<GameObject>();
    private List<GameObject> finalGiftList = new List<GameObject>();

    private List<Material> selectedColors = new List<Material>();
    private List<Material> selectedBows = new List<Material>();

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
        PlaceGifts();

        //Assign Tag to Gift

        //Start Game
        
    }

    private void RandomizeGifts()
    {
        randomizationGiftList = new List<GameObject>(gifts);
        int rand;
        int randColor;
        int randBow;
        //Loop through remaining gifts in the list until the final list has 3
        while(finalGiftList.Count < 3)
        {
            rand = UnityEngine.Random.Range(0, randomizationGiftList.Count);
            if (!finalGiftList.Contains(randomizationGiftList[rand]))
            {
                //Add to final gift list and remove from randomization list
                finalGiftList.Add(randomizationGiftList[rand]);
                randomizationGiftList.RemoveAt(rand);
            }
        }
        foreach(GameObject gift in finalGiftList)
        {
            //Randomize color
            randColor = UnityEngine.Random.Range(0, giftMaterials.Count);
            randBow = UnityEngine.Random.Range(0, bowMaterials.Count);
            if (!selectedColors.Contains(giftMaterials[randColor].material))
            {
                gift.GetComponent<GirftScript>().color = giftMaterials[randColor].color;
                gift.GetComponent<GirftScript>().giftMaterial = giftMaterials[randColor].material;
            }
            if (!selectedBows.Contains(bowMaterials[randBow]))
            {
                gift.GetComponent<GirftScript>().bowMaterial = bowMaterials[randBow];
            }
            gift.GetComponent<GirftScript>().LoadColor();
        }
    }

    private void PlaceGifts()
    {
        finalGiftList[0].transform.position = new Vector3(-20f, 0f, -3f);
        finalGiftList[1].transform.position = new Vector3(0f, 0f, -3f);
        finalGiftList[2].transform.position = new Vector3(20f, 0f, -3f);

        //I know this part is scuf
        //fixing placement for certain items
        if(finalGiftList[0] == gifts[4])
        {
            finalGiftList[0].transform.position = new Vector3(-17f, 0f, -3f);
        }
        foreach(GameObject gift in finalGiftList)
        {
            if(gift == gifts[1])
            {
                gift.transform.Translate(new Vector3(0f, 1.5f, 0f));
            }
        }
    }

    public void CheckCorrect()
    {
        //Check each gift to see if it is correct
        foreach (GameObject gift in gifts)
        {
            if (!gift.GetComponent<GirftScript>().IsCorrect())
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
