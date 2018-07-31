using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class GeneticAlgorithm : MonoBehaviour
{

	public int popSize;
	public int chromSize;
	public bool useDefaults;
	public int seed;
	public int randomFillPercent;
	public int maxGen;
	public string fout;
	public bool hillclimber;

	MapGenerator maze1;
	MapGenerator maze2;
	MapGenerator maze3;
	MapGenerator maze4;
	MapGenerator maze5;
	MapGenerator maze6;
	MapGenerator maze7;
	MapGenerator maze8;
	MapGenerator maze9;

	Slider slider1;
	Slider slider2;
	Slider slider3;
	Slider slider4;
	Slider slider5;
	Slider slider6;
	Slider slider7;
	Slider slider8;
	Slider slider9;

	int[,] population;
	int[,] parents;
	int[] fitness;
	int[] popKeys;
	int[,] fullPopulation;

    System.Random pseudoRandom;


    int generation;

	// Use this for initialization
	void Start()
	{
		if (useDefaults)
		{
			popSize = 16;
			chromSize = 418;
			seed = 1337;
			randomFillPercent = 50;
			maxGen = 10;
			fout = "test.txt";
			hillclimber = false;
		}
		population = new int[9, chromSize];
		fullPopulation = new int[popSize,chromSize];
		parents = new int[popSize, chromSize];
		fitness = new int[popSize];
		popKeys = new int[9];
        pseudoRandom = new System.Random(seed.GetHashCode());
		generation = 0;

        maze1 = GameObject.Find ("Level1").GetComponentInChildren<MapGenerator> ();
		maze2 = GameObject.Find ("Level2").GetComponentInChildren<MapGenerator> ();
		maze3 = GameObject.Find ("Level3").GetComponentInChildren<MapGenerator> ();
		maze4 = GameObject.Find ("Level4").GetComponentInChildren<MapGenerator> ();
		maze5 = GameObject.Find ("Level5").GetComponentInChildren<MapGenerator> ();
		maze6 = GameObject.Find ("Level6").GetComponentInChildren<MapGenerator> ();
		maze7 = GameObject.Find ("Level7").GetComponentInChildren<MapGenerator> ();
		maze8 = GameObject.Find ("Level8").GetComponentInChildren<MapGenerator> ();
		maze9 = GameObject.Find ("Level9").GetComponentInChildren<MapGenerator> ();

		slider1 = GameObject.Find ("Slider1").GetComponent<Slider>();
		slider2 = GameObject.Find ("Slider2").GetComponent<Slider>();
		slider3 = GameObject.Find ("Slider3").GetComponent<Slider>();
		slider4 = GameObject.Find ("Slider4").GetComponent<Slider>();
		slider5 = GameObject.Find ("Slider5").GetComponent<Slider>();
		slider6 = GameObject.Find ("Slider6").GetComponent<Slider>();
		slider7 = GameObject.Find ("Slider7").GetComponent<Slider>();
		slider8 = GameObject.Find ("Slider8").GetComponent<Slider>();
		slider9 = GameObject.Find ("Slider9").GetComponent<Slider>();

		initPop();
		sendChroms();
	}

	void initPop()
	{
        for (int x = 0; x < popSize; x++)
		{
			for (int y = 0; y < chromSize; y++)
			{
				if (y < chromSize - 18)
				{
					if (y / 20 < 1 || y / 20 > 18 || y % 20 < 1 || y % 20 > 18)
					{
						fullPopulation[x, y] = 1;
					}
					else
					{
						fullPopulation[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
					}
				}
				else
				{
					fullPopulation[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
			//initialize fitness to minimum
			fitness [x] = 1;
			//select first nine non-duplicates
			findUniques();
		}
	}

	void findUniques(){
		int possible = 0;
		int check = 0;
		bool dup = true;
		for (int selected = 0; selected < 9; selected++) {
			while (dup && possible < popSize) {
				while (check < selected) {
					for (int iter = 0; iter < chromSize; iter++) {
						if (fullPopulation [possible, iter] != population [check, iter]) {
							dup = false;
						}
					}
					check++;
				}
				//add non-duplicate to population
				if (!dup || selected == 0) {
					for (int iter = 0; iter < chromSize; iter++) {
						population [selected, iter] = fullPopulation [possible, iter];
					}
					dup = false;
					popKeys [selected] = possible;
				}
				possible++;
				check = 0;
			}
			dup = true;
			//if population isn't full and only duplicates remain, add random duplicates to population
			if (possible >= popSize && selected < 9) {
				for (; selected < 9; selected++) {
					addRandom (selected);
				}
			}
		}
	}

	void addRandom(int select){
		//System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		int rand = pseudoRandom.Next (0, popSize);

		for (int iter = 0; iter < chromSize; iter++) {
			population [select, iter] = fullPopulation [rand, iter];
		}
	}

	public void NextGen()
	{
		//next gen button pressed
		//get fitness of current gen
		getFit();
        // Print highest fitness chromasome from either start, middle, or end generation to file
        if (generation == 0 || generation == maxGen/2 || generation == maxGen-1)
        {
            int maxfit = 0;
            for(int i = 0; i < popSize; i++)
            {
                if (fitness[i] > fitness[maxfit])
                {
                    maxfit = i;
                }
            }
			string chromozome = "";
            for(int x = 0; x < chromSize; x++)
            {
				chromozome = chromozome + population[maxfit, x];
				//chromozome = chromozome + ",";
            }
			chromozome = chromozome + "\n";
			System.IO.File.AppendAllText (fout, chromozome);
			System.IO.File.AppendAllText (fout, System.Environment.NewLine);
        }

		//pick parents
		selectParents();
		for(int child = 0; child < popSize;child += 2){
			//cross over
			crossover(child,child+1);
			//mutate
			mutate(child);
			mutate(child+1);
		}
		findUniques ();
		//send chroms to level generators
		sendChroms();
		//increment generation counter
		generation++;
        if (generation >= maxGen) {
            //end game
			Debug.Log("Quit");
			Application.Quit ();
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
				maze2.map[width, height] = population[1, x];
				maze3.map[width, height] = population[2, x];
				maze4.map[width, height] = population[3, x];
				maze5.map[width, height] = population[4, x];
				maze6.map[width, height] = population[5, x];
				maze7.map[width, height] = population[6, x];
				maze8.map[width, height] = population[7, x];
				maze9.map[width, height] = population[8, x];
			}
			else
			{
				maze1.rules[(x - chromSize) + 18] = population[0, x];
				maze2.rules[(x - chromSize) + 18] = population[1, x];
				maze3.rules[(x - chromSize) + 18] = population[2, x];
				maze4.rules[(x - chromSize) + 18] = population[3, x];
				maze5.rules[(x - chromSize) + 18] = population[4, x];
				maze6.rules[(x - chromSize) + 18] = population[5, x];
				maze7.rules[(x - chromSize) + 18] = population[6, x];
				maze8.rules[(x - chromSize) + 18] = population[7, x];
				maze9.rules[(x - chromSize) + 18] = population[8, x];
			}
		}
		maze1.newChrom = true;
		maze2.newChrom = true;
		maze3.newChrom = true;
		maze4.newChrom = true;
		maze5.newChrom = true;
		maze6.newChrom = true;
		maze7.newChrom = true;
		maze8.newChrom = true;
		maze9.newChrom = true;
	}

	public void getFit()
	{
		//get the fitness of each maze
		fitness [popKeys[0]] = (int)slider1.value;
		fitness [popKeys[1]] = (int)slider2.value;
		fitness [popKeys[2]] = (int)slider3.value;
		fitness [popKeys[3]] = (int)slider4.value;
		fitness [popKeys[4]] = (int)slider5.value;
		fitness [popKeys[5]] = (int)slider6.value;
		fitness [popKeys[6]] = (int)slider7.value;
		fitness [popKeys[7]] = (int)slider8.value;
		fitness [popKeys[8]] = (int)slider9.value;

        slider1.value = 1;
        slider2.value = 1;
        slider3.value = 1;
        slider4.value = 1;
        slider5.value = 1;
        slider6.value = 1;
        slider7.value = 1;
        slider8.value = 1;
        slider9.value = 1;

		missingFit ();
    }

	void missingFit(){
		bool unEvaled = true;
		for (int x = 0; x < popSize; x++) {
			for (int check = 0; check < 9; check++) {
				if (x == popKeys [check]) {
					unEvaled = false;
				}
			}

			if (unEvaled) {
				findClosest (x);
			}
			unEvaled = true;
		}
	}

	void findClosest(int chrom){
		int closest = 0;
		int sameBits = 0;
		int prevSame = 0;
        int diffBits = 0;
        int prevDiff = 0;

		for (int x = 0; x < 9; x++) {
			for (int check = 0; check < chromSize; check++) {
				if (fullPopulation [chrom, check] == fullPopulation [popKeys [x], check]) {
					sameBits++;
				} else
                {
                    diffBits++;
                }
			}
			if (sameBits > prevSame) {
				closest = x;
				prevSame = sameBits;
                prevDiff = diffBits;
			}
			sameBits = 0;
            diffBits = 0;
		}

        fitness[chrom] = fitness[popKeys[closest]];// * (1-(prevDiff/(prevSame+prevDiff)));
	}

	void selectParents()
	{
		//select the parents for crossover
		int totalFit = getTotFit();
		//System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		int selected = pseudoRandom.Next(0,totalFit);
		int selectTotal = 0;
		int fit = 0;
		int iter = 0;

		while (selectTotal < popSize) {
			
			fit = fitness [iter];
			while(selected >= fit){
				iter++;
				fit += fitness [iter];
			}
			copyParent (selectTotal, iter);
			selectTotal++;
			iter = 0;
            selected = pseudoRandom.Next(0, totalFit);
        }
	}

	void copyParent (int target, int source){
		for (int x = 0; x < chromSize; x++) {
			parents [target, x] = fullPopulation [source, x];
		}
	}

	int getTotFit (){
		int total = 0;

		for (int iter = 0; iter < popSize; iter++) {
			total += fitness [iter];
    	}
        //Debug.Log("total: "); print(total);
        return total;
	}

	void crossover(int target1, int target2)
	{
		//crossover parents to make the next generation of mazes
		int splitMaze = pseudoRandom.Next(0, chromSize - 18);
		int splitRules = pseudoRandom.Next(chromSize - 18, chromSize);

		if (pseudoRandom.Next(0, 100) < 75)
		{
			for (int x = 0; x < chromSize; x++)
			{
				if (x < splitMaze)
				{
					fullPopulation [target1, x] = parents [target1, x];
					fullPopulation [target2, x] = parents [target2, x];
				}
				else if (x < chromSize - 18)
				{
					fullPopulation [target2, x] = parents [target1, x];
					fullPopulation [target1, x] = parents [target2, x];
				}
				else if (x < splitRules)
				{
					fullPopulation [target1, x] = parents [target1, x];
					fullPopulation [target2, x] = parents [target2, x];
				}
				else
				{
					fullPopulation [target2, x] = parents [target1, x];
					fullPopulation [target1, x] = parents [target2, x];
				}
			}
		}
	}

	void mutate(int tar)
	{
		//pick a random spot in the selected chromasome to mutate randomly
		//System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        //int y = pseudoRandom.Next(0, chromSize);
        for (int y = 0; y < chromSize; y++)
        {
            if (pseudoRandom.Next(0, 1000) < 2)
            {
                //Debug.Log("Mutate");
				fullPopulation[tar, y] = 1 - fullPopulation[tar, y];
            }
        }
	}
}
