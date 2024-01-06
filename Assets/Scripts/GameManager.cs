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

         **I DONT KNOW WHAT THE LOGIC IS FOR HOW THE GIFTS ARE ASSIGNED FROM THE KIDS LISTS OR HOW THAT'S GOING TO BE DISPLAYED**
         */
        RandomizeGifts();


        //Assign Tag to Present

        //Start Game
    }

    private void RandomizeGifts()
    {
        randomizationGiftList = new List<GameObject>(gifts);
        int rand;
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
