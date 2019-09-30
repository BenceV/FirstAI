using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creatures : MonoBehaviour {
    private float speed = 3f;
    public bool notLiving;
    private float x;
    public Material livingMaterial;
    public Material notMovingMaterial;
    public Material reallyDeadMaterial;
    private float rb;
    private float rrb;
    private float lb;
    private float llb;
    private float mb;
    private float[] outPut;
    private float[] inPut;
    public float distanceCovered;
    public  float timeAlive;
    private NeuralNetwork neuralNetwork;
    private int ID;
    private float timeNotLiving;
    public bool realyDead;
    public float utility;
    public Rigidbody rigidBody;
    public int finish_point;
    private int next_point;
	// Use this for initialization
    public NeuralNetwork GetNeuralNetwork()
    {
        return neuralNetwork;
    }
    
	void Start () {
        
        notLiving = false;
        GetComponent<Renderer>().material = livingMaterial;
        rigidBody = GetComponent<Rigidbody>();
        next_point = 0;
        gameObject.name = ID.ToString();
    }

    // Update is called once per frame
    void Update () {
        GiveInput();
        inPut = new float[] {speed, rb, rrb, mb, llb, lb};
        outPut = new float[3];
        outPut = neuralNetwork.FeedForward(inPut);
        
        if (!notLiving)
        {
            timeNotLiving = 0;
            timeAlive += Time.deltaTime;
            distanceCovered += Time.deltaTime * speed;
            GetComponent<Renderer>().material = livingMaterial;
            if (speed < 0)
            {
                speed = 0;
            }
            if (speed <= 4f) {
                speed += 0.05f;
            }


            if (outPut[0]>=0.5f)
            {
                
                speed -= 0.5f;
            }

            transform.Translate(transform.right * speed*Time.deltaTime, Space.World);

            if (outPut[1]>=0.5f) {
                transform.Rotate(transform.up, 20 * Time.deltaTime,Space.World);
            }
            if (outPut[2]>=0.5f)
            {
                transform.Rotate(transform.up, -20 * Time.deltaTime, Space.World);
            }
            
        }
        else
        {
            GetComponent<Renderer>().material = notMovingMaterial;
            timeNotLiving += Time.deltaTime;
            if (timeNotLiving>2f)
            {
                realyDead = true;
                GetComponent<Renderer>().material = reallyDeadMaterial;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        notLiving = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==10)
        {

            int index = int.Parse(other.name);
            if (next_point == index && next_point != finish_point)
            {
                next_point++;
                utility++;
            }
            else if (next_point == index && next_point == finish_point)
           
 {
                next_point = 0;
                utility++;
            }
            else if(index != next_point-1 ){
                notLiving = true;
            }


        }
    }
    private void GiveInput()
    {
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, 1), out hit, 400f, layerMask))
        {
            rb = hit.distance;
        }
        else
        {
            rb = 0;    
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, 2), out hit, 400f, layerMask))
        {
            rrb = hit.distance;
        }
        else
        {
            rrb = 0;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, -1), out hit, 400f, layerMask))
        {
            lb = hit.distance;
        }
        else
        {
            lb = 0;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, -2), out hit, 400f, layerMask))
        {
            llb = hit.distance;
        }
        else
        {
            llb = 0;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, 400f, layerMask))
        {
            
            mb = hit.distance;
        }
        else {
            mb = 0;
        }


    }
    public void InitCreature(NeuralNetwork neuralNetwork, int ID,int finish_point)
    {
        this.finish_point = finish_point;
        this.neuralNetwork = neuralNetwork;
        this.ID = ID;
    }
}
