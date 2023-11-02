using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BoardManager : MonoBehaviour 
{
	public static BoardManager instance;
	public List<Sprite> characters = new List<Sprite>();
    public Sprite bomb;
    public GameObject tile;
	public int xSize, ySize;


    private GameObject[,] tiles;

	public bool IsShifting { get; set; }

	void Start () 
	{
		instance = GetComponent<BoardManager>();

		Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);

        for (int x = xSize - 1; x >= 0; x--)
        {
            tiles[x, ySize - 1].SetActive(false);
        }
    }

    private void CreateBoard (float xOffset, float yOffset) 
	{
		tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
		float startY = transform.position.y;

		Sprite[] previousLeft = new Sprite[ySize];
		Sprite previousBelow = null; 

		for (int x = 0; x < xSize; x++) 
		{
			for (int y = 0; y < ySize; y++) 
			{
				GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
				tiles[x, y] = newTile;
				newTile.transform.parent = transform; 

				List<Sprite> possibleCharacters = new List<Sprite>();
				possibleCharacters.AddRange(characters);

				possibleCharacters.Remove(previousLeft[y]);
				possibleCharacters.Remove(previousBelow);

				Sprite newSprite;

                if (Container.isRandom == false && Random.Range(0, 60) == 0)
				{
					newSprite = bomb;

				}
				else
				{
                    newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                }
				newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
				previousLeft[y] = newSprite;
				previousBelow = newSprite;
			}
        }
    }

	public IEnumerator FindNullTiles() 
	{
		for(int i =  0; i < 2; i++)
		{
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                    {
                        yield return StartCoroutine(ShiftTilesDown(x, y));
						break;
                    }
                }
            }

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    tiles[x, y].GetComponent<Tile>().ClearAllMatches();
                }
            }
        }
    }


    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .03f) 
	{
		IsShifting = true;
		List<SpriteRenderer> renders = new List<SpriteRenderer>();
		int nullCount = 0;

		for (int y = yStart; y < ySize; y++) 
		{
			SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
			if (render.sprite == null) 
			{
				nullCount++;
			}
			renders.Add(render);
		}

		for (int i = 0; i < nullCount; i++) 
		{
			GUIManager.instance.Score += 50;
			yield return new WaitForSeconds(shiftDelay);
			for (int k = 0; k < renders.Count - 1; k++) 
			{
				renders[k].sprite = renders[k + 1].sprite;
                if (Container.isRandom == false && Random.Range(0, 40) == 0)
                {
                    renders[k + 1].sprite = bomb;

                }
                else
                {
                    renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
                }
			}
		}

		IsShifting = false;
	}

	private Sprite GetNewSprite(int x, int y) 
	{
		List<Sprite> possibleCharacters = new List<Sprite>();
		possibleCharacters.AddRange(characters);

		if (x > 0) 
		{
			possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (x < xSize - 1) 
		{
			possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (y > 0) 
		{
			possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
		}
        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
	}
}
