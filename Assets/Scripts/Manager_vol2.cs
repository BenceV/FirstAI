using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_vol2 : MonoBehaviour {
    public GameObject prefab;
    private int numberOfCreatures = 100;
    private NeuralNetwork[] cnetworks;
    private int[] layers = { 6, 7, 3 };
    public GameObject[] creatures;
    private GameObject bestCreature;
    private int curGenN = 0;
    private float timeLeft = 30f;
    private float standard_time = 120f;
    public int finish_point;
    public Text curGeneration;

    // Use this for initialization
    void Start () {
        KillAll();

        creatures = new GameObject[numberOfCreatures];
        cnetworks = new NeuralNetwork[numberOfCreatures];
        if (curGenN == 0)
        {
            SetUpGeneration();
        }
        else
        {
            SetUpEvolution();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        //print(creatures[0].GetComponent<Creatures>().deads);

        if (timeLeft<0)
        {
            Start();
        }
        else if (IsAllDead())
        {
            Start();
        }
            { }
        if (timeLeft > standard_time-1f && timeLeft < standard_time-0.5f)
        {
            for (int i = 0; i < numberOfCreatures; i++)
            {
                creatures[i].GetComponent<Creatures>().notLiving = false;
            }
            
        }
        
    }

    private void SetUpGeneration()
    {   
        timeLeft = standard_time;
        curGenN++;
        curGeneration.text = "Current Generation Number: "+ curGenN.ToString();
        for (int i = 0; i < numberOfCreatures; i++)
        {   
            NeuralNetwork neuralNetwork = new NeuralNetwork(layers);
            GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            var scr = creature.GetComponent<Creatures>();
            scr.InitCreature(neuralNetwork, i,finish_point);
            cnetworks[i] = new NeuralNetwork(neuralNetwork);
            creatures[i] = creature;
            //creature.transform.Rotate(Vector3.forward);
        }

    }
    private void SetUpEvolution()
    {
        timeLeft = standard_time;
        curGenN++;
        curGeneration.text= "Current Generation Number: " + curGenN.ToString();

        NeuralNetwork neuralNetwork = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
        GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
        creature.GetComponent<Creatures>().InitCreature(neuralNetwork, 0,finish_point);
        cnetworks[0] = new NeuralNetwork(neuralNetwork);
        creatures[0] = creature;
        //creature.transform.Rotate(Vector3.forward);

        for (int i = 1; i < 60; i++)
        {
            NeuralNetwork neuralNetwork1 = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
            neuralNetwork1.Mutate(0.1f);
            GameObject creature2 = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature2.GetComponent<Creatures>().InitCreature(neuralNetwork1, i, finish_point);
            cnetworks[i] = new NeuralNetwork(neuralNetwork1);
            creatures[i] = creature2;
            //creature2.transform.Rotate(Vector3.forward);
        }
        for (int i = 60; i < 80; i++)
        {
            NeuralNetwork neuralNetwork1 = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
            neuralNetwork1.Mutate(0.7f);
            GameObject creature2 = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature2.GetComponent<Creatures>().InitCreature(neuralNetwork1, i, finish_point);
            cnetworks[i] = new NeuralNetwork(neuralNetwork1);
            creatures[i] = creature2;
            //creature2.transform.Rotate(Vector3.forward);
        }
        for (int i = 80; i < numberOfCreatures; i++)
        {
            NeuralNetwork neuralNetwork1 = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
            neuralNetwork1.Mutate(0.9f);
            GameObject creature2 = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature2.GetComponent<Creatures>().InitCreature(neuralNetwork1, i, finish_point);
            cnetworks[i] = new NeuralNetwork(neuralNetwork1);
            creatures[i] = creature2;
            //creature2.transform.Rotate(Vector3.forward);
        }

    }
    public void KillAll()
    {
        if (creatures.Length!=0)
        {
            float[] utilities = new float[numberOfCreatures];
            for (int i = 0; i < numberOfCreatures; i++)
            {
                var ut = creatures[i].GetComponent<Creatures>().utility;
                print(i + ": Utility " + ut);
                utilities[i] =ut;
            }
            int bestIndex = FindIndexMax(utilities);
            bestCreature = creatures[bestIndex];

            for (int i = 0; i < numberOfCreatures; i++)
            {
                creatures[i].GetComponent<Creatures>().notLiving = false;
                GameObject.Destroy(creatures[i]);
                
            }
        }
    }
    private bool IsAllDead()
    {
        bool t = true;
        for (int i = 0; i < numberOfCreatures; i++)
        {
            t = t && creatures[i].GetComponent<Creatures>().realyDead;
        }
        return t;
    }

    private int FindIndexMax(float[] utilities)
    {
        int index = 0;
        float value = utilities[0];
        for (int i = 0; i < numberOfCreatures; i++)
        {
            if (utilities[i]>value)
            {
                index = i;
                value = utilities[i];
            }
        }
        return index;
    }
}
