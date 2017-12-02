using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiMove
{
    public int frameDuration;
    public int framesElapsed;
    public System.Action<float> action;
    public System.Func<float, float> smoothingFunction;
    public float start;
    public float end;

    public MultiMove(System.Action<float> action, int frameDuration, System.Func<float, float> smoothingFunction, float start, float end)
    {
        this.action = action;
        this.frameDuration = frameDuration;
        this.smoothingFunction = smoothingFunction;
        this.start = start;
        this.end = end;
    }

    public bool update()
    {
        float percentCompleted = this.framesElapsed / this.frameDuration;
        if (percentCompleted <= 1f)
        {
            action(smoothingFunction(start + percentCompleted * (end- start)));

        }
        else
        {
            return false;
        }
        framesElapsed++;
        return true;
    }

    public static List<MultiMove> executeMultiMoves(ref List<MultiMove> multiMoves)
    {
        List<MultiMove> returnList = new List<MultiMove>();
        foreach (MultiMove move in multiMoves)
        {
            if(move.update() == true)
            {
                returnList.Add(move);
            }
        }
        return returnList;
    }
}
public class Movement : MonoBehaviour {

    public float inertia;
    public float inertiaDuration;
    public Vector3 currentVelocity;
    public float weight;
    public float debugWeightPenalty;
    public float debugBagSpeed;
    public float debugSpeed;
    public float debugDecaySpeed;
    public List<MultiMove> multiMoves = new List<MultiMove>();
    private int framesSinceSwingBag = 999;
    private int swingBagDuration = 30;


    float weightPenalty()
    {
        this.debugWeightPenalty = Mathf.Pow(Mathf.Min(1, this.weight / 20f), 2f); ;
        return this.debugWeightPenalty;
    }

    float bagSpeed()
    {
        debugBagSpeed = 8f + weightPenalty() * 200f;
        return debugBagSpeed;
    }

    float speed()
    {
        this.debugSpeed = 0.5f + 4f * (1 - weightPenalty());
        return this.debugSpeed;
    }

    Vector3 getCurrentVelocity()
    {
        return this.currentVelocity;
    }
    // Use this for initialization
    void Start() {

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

        if (upInput()) {
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

        if (x == 1 && y == 1)
        {
            x = Mathf.Sqrt(2) / 2;
            y = Mathf.Sqrt(2) / 2;
        }

        return new Vector3(x, y);
    }

    void swingBag(float smoothing)
    {
        Vector3 direction;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        direction = this.transform.position - ray.origin;
        direction.Set(direction.x, direction.y, 0f);
        this.currentVelocity = direction.normalized * bagSpeed() * -1 * smoothing;
        framesSinceSwingBag = 0;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            multiMoves.Add(new MultiMove(swingBag, 30, Mathf.Sin, 0f, Mathf.PI));
        }
        else
        {
            this.currentVelocity += inputVector();
        }
    }


    void FixedUpdate()
    {
        lockZ();
        MultiMove.executeMultiMoves(ref multiMoves);
        this.currentVelocity = this.currentVelocity / 1.5f;
        if (Mathf.Abs(this.currentVelocity.x) < 0.05f && Mathf.Abs(this.currentVelocity.y) < 0.05f)
        {
            this.currentVelocity = new Vector3(0, 0);
        }
        transform.position = transform.position + this.getCurrentVelocity() * Time.deltaTime;
    }

    private void lockZ()
    {
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "item") {
            coll.gameObject.SetActive(false);
            this.weight++;
        }
    }
}
