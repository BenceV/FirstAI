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
    private int rb;
    private int lb;
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
    public bool[] points;
	// Use this for initialization
    public NeuralNetwork GetNeuralNetwork()
    {
        return neuralNetwork;
    }
    
	void Start () {
        
        notLiving = false;
        GetComponent<Renderer>().material = livingMaterial;
        rigidBody = GetComponent<Rigidbody>();
        points = new bool[] {false, false, false, false, false, false, false, false};
        gameObject.name = ID.ToString();
    }

    // Update is called once per frame
    void Update () {
        GiveInput();
        inPut = new float[] {speed,rb,lb};
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

            speed += 0.05f;

            if (outPut[0]>=0.5f)
            {
                
                speed -= 0.3f;
            }

            transform.Translate(Vector3.forward * Time.deltaTime*speed);

            if (outPut[1]>=0.5f) { 
                x += Time.deltaTime * 30;
                transform.rotation = Quaternion.Euler(0, x, 0);
            }
            if (outPut[2]>=0.5f)
            {
                x -= Time.deltaTime * 30;
                transform.rotation = Quaternion.Euler(0, x, 0);
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
        if (other.gameObject.CompareTag("Points"))
        {

            int index = int.Parse(other.name);
            if (points[index] == false)
            {
                points[index] = true;
                utility++;
                print(ID + ": " + utility);
            }


        }
    }
    private void GiveInput()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, 1), 6f))
        {
            rb = 1;
        }
        else
        {
            rb = 0;    
        }


        if (Physics.Raycast(transform.position, transform.TransformDirection(1, 0, -1), 6f))
        {
            lb = 1;
        }
        else
        {
            lb = 0;
        }


    }
    public void InitCreature(NeuralNetwork neuralNetwork, int ID)
    {
        this.neuralNetwork = neuralNetwork;
        this.ID = ID;
    }
}
