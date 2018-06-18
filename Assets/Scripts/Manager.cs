using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public GameObject prefab;
    private int numberOfCreatures = 10;
    private NeuralNetwork[] cnetworks;
    private NeuralNetwork[] pnetworks;
    private int[] layers = {3,5,5,3};
    public GameObject[] creatures;
    private int curGenN;
    private float timeLeft = 30f;


    void Start () {
        creatures = new GameObject[numberOfCreatures];
        cnetworks = new NeuralNetwork[numberOfCreatures];
        pnetworks = new NeuralNetwork[numberOfCreatures];
        
        curGenN = 0;
        SetUpFirstGeneration();


	}
    public void SetUpFirstGeneration()
    {
        for (int i = 0; i < numberOfCreatures; i++)
        {
            NeuralNetwork neuralNetwork = new NeuralNetwork(layers);
            GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature.GetComponent<Creatures>().InitCreature(neuralNetwork, i);
            cnetworks[i] = new NeuralNetwork(neuralNetwork);
            creatures[i] = creature;
        }
    }


    public void SetUpNextGeneration(int gen,NeuralNetwork[] previousNetworks)
    {
        
        for (int i = 0; i < numberOfCreatures; i++)
        {
            NeuralNetwork neuralNetwork = new NeuralNetwork(previousNetworks[i]);
            GameObject creature = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
            creature.GetComponent<Creatures>().InitCreature(neuralNetwork,curGenN*10+i);
            cnetworks[i] = new NeuralNetwork(neuralNetwork);
            creatures[i] = creature;
            
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        //print(timeLeft);
        if (timeLeft<0)
        {
            KillAll();
            AssignFittness();
            curGenN++;
            timeLeft = 30f;
            SetUpNextGeneration(curGenN, SelectWhoLivesAndWhoDies());
        }
	}
    
    private void KillAll()
    {
        for (int i = 0; i < numberOfCreatures; i++)
        {
            creatures[i].GetComponent<Creatures>().notLiving = true;
        }
    }
    private void AssignFittness()
    {
        for (int i = 0; i < numberOfCreatures; i++)
        {
            Creatures c = creatures[i].GetComponent<Creatures>();
            cnetworks[i].SetFitness((c.distanceCovered/c.timeAlive)+c.distanceCovered);
        }
    }
    private NeuralNetwork[] SelectWhoLivesAndWhoDies()
    {
        NeuralNetwork[] anetworks = new NeuralNetwork[numberOfCreatures];
        List<NeuralNetwork> rnetworks = new List<NeuralNetwork>();
        for (int i = 0; i < numberOfCreatures; i++)
        {
            NeuralNetwork n = new NeuralNetwork(cnetworks[i]);
            rnetworks.Add(n);
        }
        rnetworks.Sort((p1,p2)=>p2.CompareTo(p1));

        for (int i = 0; i < numberOfCreatures-6; i++)
        {
            NeuralNetwork n = new NeuralNetwork(rnetworks[i]);
            n.Mutate();
            anetworks[i] = new NeuralNetwork(n);
            
        }
        for (int i = numberOfCreatures-6; i < numberOfCreatures; i++)
        {
            anetworks[i] = new NeuralNetwork(layers);
        }
        for (int i = 0; i < numberOfCreatures; i++)
        {
            GameObject.Destroy(creatures[i]);
        }
        
        return anetworks;
    }
}
