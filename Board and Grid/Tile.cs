using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{
	public static Tile instance;
	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	private static Tile previousSelected = null;

	private SpriteRenderer render;
	private bool isSelected = false;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	private bool isExp = false;// Эта переменная должна разрешать взрываться, что-бы толькочто заспавненные бомбы не взрывались

	void Awake() 
	{
		render = GetComponent<SpriteRenderer>();
    }

    private void Select() 
	{
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();
		SFXManager.instance.PlaySFX(Clip.Select);
	}

	private void Deselect() 
	{
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}

	void OnMouseDown() 
	{
		if (render.sprite == null || BoardManager.instance.IsShifting) {
			return;
		}

		if (isSelected) 
		{

            if (render.sprite == BoardManager.instance.bomb)
            {
                isExp = true;
                previousSelected.ClearAllMatches();
                previousSelected.Deselect();
                ClearAllMatches();
                GUIManager.instance.MoveCounter--;
            }
			else
			{
                Deselect();
            }
		} 
		else 
		{
			if (previousSelected == null)
			{
				Select();
			}
            else
			{
                if (GetAllAdjacentTiles().Contains(previousSelected.gameObject))
				{
					if (render.sprite == BoardManager.instance.bomb || previousSelected.gameObject.GetComponent<SpriteRenderer>().sprite == BoardManager.instance.bomb)
						isExp = true;
					else if (render.sprite == BoardManager.instance.bomb)
						isExp = true;

                    SwapSprite(previousSelected.render);
					previousSelected.ClearAllMatches();
					previousSelected.Deselect(); Debug.Log("zxc");
                    ClearAllMatches();
                }
                else
				{
					previousSelected.GetComponent<Tile>().Deselect();
					Select();
				}
			}
		}
	}

	public void SwapSprite(SpriteRenderer render2) 
	{
		if (render.sprite == render2.sprite) 
		{
			return;
		}

		Sprite tempSprite = render2.sprite;
		render2.sprite = render.sprite;
		render.sprite = tempSprite;
		SFXManager.instance.PlaySFX(Clip.Swap);
		GUIManager.instance.MoveCounter--; 
	}

	private GameObject GetAdjacent(Vector2 castDir) 
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		if (hit.collider != null) 
		{
			return hit.collider.gameObject;
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles() 
	{
		List<GameObject> adjacentTiles = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++) 
		{
			adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
		}
		return adjacentTiles;
	}

	private List<GameObject> FindMatch(Vector2 castDir) 
	{
		List<GameObject> matchingTiles = new List<GameObject>();
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite) 
		{
			matchingTiles.Add(hit.collider.gameObject);
			hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
		}
		return matchingTiles;
	}

	private void ClearMatch(Vector2[] paths) 
	{
		List<GameObject> matchingTiles = new List<GameObject>();
		/*for (int i = 0; i < paths.Length; i++)
        {
            if (render.sprite == BoardManager.instance.bomb)
            {
				matchingTiles.AddRange(Exploide(paths[i]));

            }else
			{
                matchingTiles.AddRange(FindMatch(paths[i]));
            }
		}*/
        if (matchingTiles.Count >= 2) 
		{
			for (int i = 0; i < matchingTiles.Count; i++) 
			{
				if(Container.isRandom == true)
				{
                    Container.sprite = matchingTiles[i].GetComponent<SpriteRenderer>().sprite;
                    Container.spriteCount = matchingTiles.Count + 1;
                    RandomManager.instance.Counter();

                }
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
			}
			matchFound = true;
		}
	}

	private bool matchFound = false;
	public void ClearAllMatches() 
	{
		if (render.sprite == null)
			return;

		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if (matchFound) 
		{
			render.sprite = null;
			matchFound = false;
            isExp = false;
            StopCoroutine(BoardManager.instance.FindNullTiles());
			StartCoroutine(BoardManager.instance.FindNullTiles());
			SFXManager.instance.PlaySFX(Clip.Clear);
        }
	}

    private List<GameObject> Exploide(Vector2 boom)
    {
		if(isExp)
		{
            List<GameObject> matchingTiles = new List<GameObject>();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, boom);
            while (hit.collider != null)
            {
                matchingTiles.Add(hit.collider.gameObject);
                hit = Physics2D.Raycast(hit.collider.transform.position, boom);
            }
            return matchingTiles;
        }
		else { return null; }
    }

}