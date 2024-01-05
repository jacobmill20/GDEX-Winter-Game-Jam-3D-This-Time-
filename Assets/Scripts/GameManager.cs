using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gifts;
    public List<Material> giftMaterials;
    public List<Material> bowMaterials;

    // Start is called before the first frame update
    void Start()
    {
        /*
            Begin Random Generation Coroutine
            Generate the random and sets values in place before the game starts
            Possibly do black screen for a couple seconds to allow the game to process and begin
         */
        StartCoroutine(GeneratePresents());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GeneratePresents()
    {
        /*
         Use random to pick a tag from the list
         Also use random to pick correct color for gift from list

         **I DONT KNOW WHAT THE LOGIC IS FOR HOW THE GIFTS ARE ASSIGNED FROM THE KIDS LISTS OR HOW THAT'S GOING TO BE DISPLAYED**
         */
        
        //Assign Tag to Present

        //Start Game
        yield return null;
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
