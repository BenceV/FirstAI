using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_vol2 : MonoBehaviour {
    public GameObject prefab;
    private int numberOfCreatures = 50;
    private NeuralNetwork[] cnetworks;
    private int[] layers = { 3, 5, 5 ,3 };
    public GameObject[] creatures;
    private GameObject bestCreature;
    private int curGenN = 0;
    private float timeLeft = 30f;
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
        if (timeLeft > 29f && timeLeft < 29.5f)
        {
            for (int i = 0; i < numberOfCreatures; i++)
            {
                creatures[i].GetComponent<Creatures>().notLiving = false;
            }
            
        }
        
    }

    private void SetUpGeneration()
    {   
        timeLeft = 30f;
        curGenN++;
        curGeneration.text = "Current Generation Number: "+ curGenN.ToString();
        for (int i = 0; i < numberOfCreatures; i++)
        {   
            NeuralNetwork neuralNetwork = new NeuralNetwork(layers);
            GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature.GetComponent<Creatures>().InitCreature(neuralNetwork, i);
            cnetworks[i] = new NeuralNetwork(neuralNetwork);
            creatures[i] = creature;
        }

    }
    private void SetUpEvolution()
    {
        timeLeft = 30f;
        curGenN++;
        curGeneration.text= "Current Generation Number: " + curGenN.ToString();

        NeuralNetwork neuralNetwork = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
        GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
        creature.GetComponent<Creatures>().InitCreature(neuralNetwork, 0);
        cnetworks[0] = new NeuralNetwork(neuralNetwork);
        creatures[0] = creature;

        for (int i = 1; i < numberOfCreatures; i++)
        {
            NeuralNetwork neuralNetwork1 = new NeuralNetwork(bestCreature.GetComponent<Creatures>().GetNeuralNetwork());
            neuralNetwork.Mutate();
            GameObject creature2 = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature2.GetComponent<Creatures>().InitCreature(neuralNetwork1, i);
            cnetworks[i] = new NeuralNetwork(neuralNetwork1);
            creatures[i] = creature2;
        }

    }
    public void KillAll()
    {
        if (creatures.Length!=0)
        {
            float[] utilities = new float[numberOfCreatures];
            for (int i = 0; i < numberOfCreatures; i++)
            {
                utilities[i] = creatures[i].GetComponent<Creatures>().utility;
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
