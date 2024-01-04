using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gifts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
