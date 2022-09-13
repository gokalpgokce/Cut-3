using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    private Board _board;
    public GameObject backGround;
    public float column, row; 
    private float _startPosX = 0f, _startPosY = 0f;

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        // Create Board
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                _startPosX = -(column - 1) / 2f;
                _startPosY = -(row - 1) / 2f;
                GameObject tilePos = Instantiate(backGround, new Vector3(_startPosX, _startPosY), Quaternion.identity);
                Vector3 newPos = new Vector3(_startPosX + j, _startPosY + i);
                tilePos.transform.position = newPos;
                tilePos.name = ("Tile " + i + "," + j);
            }
        }
    }
}
