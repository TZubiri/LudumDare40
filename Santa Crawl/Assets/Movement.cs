using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


    float speed()
    {
        return 8;
    }
	// Use this for initialization
	void Start () {
		
	}
	bool upInput()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
    }
    bool downInput()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }
    bool rightInput()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    }
    bool leftInput()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }
    // Update is called once per frame
    void Update () {
        if (upInput() && !downInput())
        {
            transform.position  = new Vector3(transform.position.x, transform.position.y + this.speed() * Time.deltaTime);
        }
        else if (!upInput() && downInput())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - this.speed() * Time.deltaTime);
        }

        if (rightInput() && !leftInput())
        {
            transform.position = new Vector3(transform.position.x + this.speed() * Time.deltaTime,transform.position.y);
        }
        else if (!rightInput() && leftInput() )
        {
            transform.position = new Vector3(transform.position.x - this.speed() * Time.deltaTime,transform.position.y);
        }

    }
}
