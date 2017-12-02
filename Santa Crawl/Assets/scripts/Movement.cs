using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float inertia;
    public float inertiaDuration;
    public Vector3 currentVelocity;
    public float weight;

    float weightPenalty()
    {
        return Mathf.Min(1,1.05f-( this.weight/20f));
    }
    float maxBagSpeed()
    {
        return 4f + this.weight;
    }

    float speed()
    {
        return 4f * weightPenalty();
    }
    float decaySpeed()
    {
        return speed() / 2f;
    }
    Vector3 getCurrentVelocity()
    {
        return this.currentVelocity;
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

    Vector3 inputVector() {
        float x = 0;
        float y = 0;

        if (upInput()){
            y += speed();
        }
        if (downInput())
        {
            y -= speed();
        }
        if (rightInput())
        {
            x += speed();
        }
        if (leftInput())
        {
            x -= speed();
        }
        
        if(x == 1 && y == 1)
        {
            x = Mathf.Sqrt(2) / 2;
            y = Mathf.Sqrt(2) / 2;
        }
        
        return new Vector3(x, y);
    }
    // Update is called once per frame
    void Update() {
        bool velSign = this.currentVelocity.x < 0f;
        bool inpSign = this.inputVector().x < 0f;
        if ((this.currentVelocity.x != 0 && this.inputVector().x != 0  ) && (velSign != inpSign))
        {
            this.currentVelocity.x = Mathf.Min(maxBagSpeed(), this.currentVelocity.x * -18f);
            Debug.Log(maxBagSpeed());
            Debug.Log(this.currentVelocity.x);
        }
        else
        {
            this.currentVelocity += inputVector();
        }
    }

    void FixedUpdate()
    {

        this.currentVelocity = this.currentVelocity / 1.5f;
        if(Mathf.Abs(this.currentVelocity.x) < 0.05f && Mathf.Abs(this.currentVelocity.y) < 0.05f)
        {
            this.currentVelocity = new Vector3(0,0);
        }
        transform.position = transform.position + this.getCurrentVelocity() * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "item") {
            coll.gameObject.SetActive(false);
            this.weight++;
        }
    }
}
