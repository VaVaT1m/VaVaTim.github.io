using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomManager : MonoBehaviour
{
    public static RandomManager instance;

    public int mode;
    int toWin = 0;

    public GameObject win, lose, board, random, steps, pause;

    public bool isWin = false;

    public GameObject[] spawners;
    public Text[] goalTextCounter;
    private int[] goal;
    private Vector3 posSpawn;
    public Sprite[] characters;
    private GameObject[] gg;
    private int[] variblesTile; // Я забыл то это, но походу это надо для рандома тайла цели, но это нахуй не надо, можно просто переменную въебать


    public GameObject goalTile;
    private Transform tr;

    private void Awake()
    {
        instance = GetComponent<RandomManager>();

        tr = GetComponent<Transform>();

        mode = Container.mode;

        Difficult(mode);
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < goal.Length; i++)
        {
            if (goal[i] == 0)
                toWin += 1;
        }
        if (toWin == mode + 1)
        {
            Win();
        }
        else
        {
            toWin = 0;
        }
    }

    private void Difficult(int diff)
    {
        variblesTile = new int[diff + 1];
        goal = new int[diff + 1];
        gg = new GameObject[diff + 1];
        for (int i = 0; i <= diff; i++) 
        {
            int hui = Container.move;// Число ходов зарандомленное с гуиманагера
            int b = Random.Range(10, 25); // Это надо для задачи рандомного числа в цели
            goal[i] = b;
            switch(i)
            {
                case 0:
                    posSpawn = new Vector3(tr.position.x, tr.position.y, tr.position.z);
                    goalTextCounter[0].gameObject.SetActive(true);
                    goalTextCounter[0].text = "x" + System.Convert.ToString(b);
                    break;
                case 1:
                    posSpawn = new Vector3(spawners[0].transform.position.x, spawners[0].transform.position.y, spawners[0].transform.position.z);
                    goalTextCounter[1].gameObject.SetActive(true);
                    goalTextCounter[1].text = "x" + System.Convert.ToString(b);
                    break;
                case 2:
                    posSpawn = new Vector3(spawners[1].transform.position.x, spawners[1].transform.position.y, spawners[1].transform.position.z);
                    goalTextCounter[2].gameObject.SetActive(true);
                    goalTextCounter[2].text = "x" + System.Convert.ToString(b);
                    break;
            }
            
            GameObject forGoal = Instantiate(goalTile, posSpawn, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
            forGoal.transform.parent = transform;
            gg[i] = forGoal;
            switch(i)
            {
                case 0:
                    variblesTile[i] = Random.Range(0, characters.Length);
                    break;
                case 1:
                    do
                    {
                        variblesTile[i] = Random.Range(0, characters.Length);
                    } while (variblesTile[i] == variblesTile[i-1]);
                    break;
                case 2:
                    do
                    {
                        variblesTile[i] = Random.Range(0, characters.Length);
                    } while (variblesTile[i] == variblesTile[i - 1] || variblesTile[i] == variblesTile[i - 2]);
                    break;
            }
            goalTile.GetComponent<SpriteRenderer>().sprite = characters[variblesTile[i]];
        }
    }
    public void Counter()
    {
        for (int i = 0; i <= gg.Length-1; i++)
        {
                if (Container.sprite == gg[i].GetComponent<SpriteRenderer>().sprite)
                {
                    for (int j = 0; Container.sprite != null; j++)
                    {
                        if (goal[i] - Container.spriteCount >= 0 && goal[i] != 0)
                        {
                            goal[i] -= Container.spriteCount;
                        }
                        else
                        {
                            goal[i] = 0;
                        }

                        Container.spriteCount = 0;
                        Container.sprite = null;
                    }
                }
            goalTextCounter[i].text = "x" + System.Convert.ToString(goal[i]);
        }
    }

    void Win()
    {
        isWin = true;
        Close();
        win.SetActive(true);
    }

    public void Close()
    {
        Container.isRandom = false;
        board.SetActive(false);
        random.SetActive(false);
        steps.SetActive(false);
        pause.SetActive(false);
    }
}
