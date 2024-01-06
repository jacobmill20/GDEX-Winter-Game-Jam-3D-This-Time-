using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TagScript : MonoBehaviour
{
    public float carryHeight;
    
    private bool pickedUp;
    private bool placed;
    private Transform giftPos;
    private GiftScript gift;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTag();
    }

    public void PickupTag()
    {
        pickedUp = true;

        //Remove tag from gift
        if (placed)
        {
            gift.currentTag = null;
            gift = null;
            placed = false;
        }
    }

    public void PutDownTag()
    {
        pickedUp = false;
        PlaceTag();
    }

    private void MoveTag()
    {
        if (pickedUp)
        {
            //Cast a ray from mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create invisible plane denoting the height that the tag will be carried
            Plane hPlane = new Plane(Vector3.up, new Vector3(0f, carryHeight, 0f));

            // if the ray hits the plane...
            float distance = 0;
            if (hPlane.Raycast(ray, out distance))
            {
                //move tag to hit point
                transform.position = ray.GetPoint(distance);
                transform.rotation = Quaternion.Euler(-90f, 180f, 0f);
            }
        }
        else if (placed)
        {
            //Make the tag move with the gift
            transform.position = giftPos.position;
            transform.rotation = giftPos.localRotation;
        }
        else
        {
            //If not picked up or placed, move back
            transform.localPosition = Vector3.zero;
        }
    }

    private void PlaceTag()
    {
        //Check if over a gift
        //Cast a ray from mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //check if collided with gift
        LayerMask mask = new LayerMask();
        mask |= (1 << 3);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, mask))
        {
            //find the tag point of the gift
            giftPos = hitInfo.collider.gameObject.transform.GetChild(0).Find("Tag Point").transform;

            //Set tag as gift's current tag
            gift = hitInfo.collider.gameObject.GetComponent<GiftScript>();
            gift.currentTag = gameObject.transform.parent.gameObject;

            //Set Placed
            placed = true;
        }
    }
}
