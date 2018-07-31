using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GeneticAlgorithm : MonoBehaviour
{

    public int popSize;
    public int chromSize;
    public bool useDefaults;
    public int seed;
    public int randomFillPercent;
    public String chromo;

    MapGenerator maze1;
    /*MapGenerator maze2;
	MapGenerator maze3;
	MapGenerator maze4;
	MapGenerator maze5;
	MapGenerator maze6;
	MapGenerator maze7;
	MapGenerator maze8;
	MapGenerator maze9;*/

    int[,] population;
    int[,] parents;

    
    // Use this for initialization
    void Start()
    {
        if (useDefaults)
        {
            popSize = 1;
            chromSize = 418;
            seed = 1337;
            randomFillPercent = 50;
        }
        population = new int[1, 418];// { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1} };
        parents = new int[popSize, chromSize];

        maze1 = GetComponentInChildren<MapGenerator>();
        /*maze2 = GetComponent<MapGenerator> ();
		maze3 = GetComponent<MapGenerator> ();
		maze4 = GetComponent<MapGenerator> ();
		maze5 = GetComponent<MapGenerator> ();
		maze6 = GetComponent<MapGenerator> ();
		maze7 = GetComponent<MapGenerator> ();
		maze8 = GetComponent<MapGenerator> ();
		maze9 = GetComponent<MapGenerator> ();*/

        initPop();
        sendChroms();
    }

  /*  void initPop()
    {

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < popSize; x++)
        {
            for (int y = 0; y < chromSize; y++)
            {
                if (y < chromSize - 18)
                {
                    if (y / 20 < 1 || y / 20 > 18 || y % 20 < 1 || y % 20 > 18)
                    {
                        population[x, y] = 1;
                    }
                    else
                    {
                        population[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
                else
                {
                    population[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
      
    }*/
    void initPop()
    {
        //Debug.Log("print chromo");
        
        for (int x = 0; x < popSize; x++)
        {
            for (int y = 0; y < chromSize; y++)
            {
               // print((int)chromo[y]);
                population[x, y] = (int)(chromo[y] - '0');
            }
        }

    }

    void Update()
    {
        if (false)
        {//next gen button pressed
         //get fitness of current gen
         //pick parents
         //cross over
         //mutate
         //send chroms to level generators
         //set button pressed to false if necessary
        }
    }

    void sendChroms()
    {
        //send chromasomes to the maze generators
        int width, height;
        for (int x = 0; x < chromSize; x++)
        {
            if (x < chromSize - 18)
            {
                width = x / MapGenerator.width;
                height = x % MapGenerator.height;
                maze1.map[width, height] = population[0, x];
            }
            else
            {
                maze1.rules[(x - chromSize) + 18] = population[0, x];
            }
        }
        maze1.newChrom = true;
    }

    void getFit()
    {
        //get the fitness of each maze
    }

    void selectParents()
    {
        //select the parents for crossover
    }

    void crossover(int parent1, int parent2, int child1, int child2)
    {
        //crossover parents to make the next generation of mazes
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        int splitMaze = pseudoRandom.Next(0, chromSize - 19);
        int splitRules = pseudoRandom.Next(chromSize - 18, chromSize - 1);

        if (pseudoRandom.Next(0, 100) < 70)
        {
            for (int x = 0; x < chromSize; x++)
            {
                if (x < splitMaze)
                {
                }
                else if (x < chromSize - 19)
                {
                }
                else if (x < splitRules)
                {
                }
                else
                {
                }
            }
        }
    }

    void mutate()
    {
        //pick a random spot in the selected chromasome to mutate randomly
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        int x = pseudoRandom.Next(0, popSize - 1);
        int y = pseudoRandom.Next(0, chromSize - 1);

        if (pseudoRandom.Next(0, 1000) < 1)
        {
            population[x, y] += 1 - population[x, y];
        }
    }
}