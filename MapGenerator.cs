using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

	public int popSize;
	public int maxGen;

	public int width;
	public int height;

	public string seed;

	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;

	void Start()
	{
		GenerateMap ();
	}

	void GenerateMap()
	{
		map = new int[width*5+20, height*4+40];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			smoothMap ();
		}
	}

	void RandomFillMap()
	{
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

		for(int layer = 0; layer < popSize/5; layer++){
			for (int pop = 0; pop < 5; pop++) {
				for (int x = (5*pop)+width*(pop); x < (5*pop)+width*(pop+1); x++) {
					for (int y = (10*layer)+(height)*(layer); y < (10*layer)+height*(layer+1); y++) {
						if (x == ((5*pop)+width*(pop)) || x == ((5*pop)+width*(pop+1)) - 1 || y == ((10*layer)+(height)*(layer)) || y == ((10*layer)+height*(layer+1)) - 1) {
							map [x, y] = 1;
						} else {
							map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
						}
					}
				}
			}
		}
	}

	void smoothMap(){
		for(int layer = 0; layer < popSize/5; layer++){
			for (int pop = 0; pop < 5; pop++) {
				for (int x = 1 + ((5 * pop) + width * (pop)); x < ((5 * pop) + width * (pop + 1)) - 1; x++) {
					for (int y = 1 + (10 * layer) + (height) * (layer); y < (10 * layer) + height * (layer + 1) - 1; y++) {
						int neighbourWallTiles = GetSurroundingWallCount (x, y, pop, layer);

						if (neighbourWallTiles == 3) {
							map [x, y] = 1;
						} else if (neighbourWallTiles > 4) {
							map [x, y] = 0;
						}
					}
				}
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY,int pop, int layer){
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (neighbourX >= ((5*pop)+width*(pop)) && neighbourX < ((5 * pop) + width * (pop + 1)) && neighbourY >= ((10*layer)+(height)*(layer)) && neighbourY < (10 * layer) + height * (layer + 1)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map [neighbourX, neighbourY];
					}
				} else {
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	void OnDrawGizmos() {
		if (map != null) {
			for(int layer = 0; layer < popSize/5; layer++){
				for (int pop = 0; pop < 5; pop++) {
					for (int x = (5*pop)+width*(pop); x < (5*pop)+width*(pop+1); x++) {
						for (int y = (10 * layer) + (height) * (layer); y < (10 * layer) + height * (layer + 1); y++) {
							Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
							Vector3 pos = new Vector3 (-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
							Gizmos.DrawCube (pos, Vector3.one);
						}
					}
				}
			}
		}
	}
}

