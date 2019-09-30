using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_SOLO : MonoBehaviour
{
    public GameObject prefab;
    private int[] layers = { 3, 5, 5, 3 };
    private int curGenN = 0;
    private float timeLeft = 30f;
    public int finish_point;
    private GameObject creature;
    // Use this for initialization
    void Start () {
        SetUp();

    }
	
	// Update is called once per frame
	void Update ()
    {
        timeLeft -= Time.deltaTime;
        print(timeLeft.ToString());
        if (timeLeft<0)
        {
            Kill();
            Start();
        }
        if (timeLeft>29f && timeLeft<29.5f)
        {
            creature.GetComponent<Creatures>().notLiving = false;
        }
    }
    private void SetUp()
    {
        timeLeft = 30f;
        curGenN++;
        creature = (GameObject)Instantiate<GameObject>(prefab,transform.position,transform.rotation);
        NeuralNetwork neuralNetwork = new NeuralNetwork(layers);
        creature.GetComponent<Creatures>().InitCreature(neuralNetwork,curGenN, finish_point);
    }
    private void Kill()
    {
        GameObject.Destroy(creature);
    }
}
