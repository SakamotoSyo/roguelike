using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorMap : MonoBehaviour
{
    [SerializeField] private int _mapHeight;
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _startX = 0;
    [SerializeField] private int _startY = 0;
    [SerializeField] private int _goalX = 0;
    [SerializeField] private int _goalY = 0;
    [SerializeField] private MyAster _ater;
    [SerializeField] private GameObject _spritePrefab;
    [SerializeField] private SpriteRenderer[,] _spriteArray;
    private int _currentTurn;
    private Color _colorFootwear;
    Layer2D _layer;
    void Start()
    {
        GenarationMapData();
        _ater.SetStartGoalPos(_startX, _startY, _goalX, _goalY);
        _ater.StartAlgorithm(_layer);
        _colorFootwear = _spriteArray[_startX, _startY].color;
        _spriteArray[_startX, _startY].color = Color.green;
        _spriteArray[_goalX, _goalY].color = Color.blue;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            NextPlayerTurn();
        }
    }

    public void GenarationMapData() 
    {
        _layer = new Layer2D(_mapWidth, _mapHeight);
        _spriteArray = new SpriteRenderer[_mapWidth, _mapHeight];

        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                var obj = Instantiate(_spritePrefab, new Vector2(x, y * -1), transform.rotation);
                _spriteArray[x, y] = obj.GetComponent<SpriteRenderer>();
                if ((x + 1) % (y + 1) != 0)
                {
                    _spriteArray[x, y].color = Color.white;
                    _layer.SetData(x, y, 1);
                }
                else
                {
                    _spriteArray[x, y].color = Color.black;
                    _layer.SetData(x, y, 2);
                }
            }
        }
    }

    public void NextPlayerTurn() 
    {
        var nodeList = _ater.ParentNodeList;
        _spriteArray[nodeList[_currentTurn].X, nodeList[_currentTurn].Y].color = _colorFootwear;
        _currentTurn++;
        _colorFootwear = _spriteArray[nodeList[_currentTurn].X, nodeList[_currentTurn].Y].color;
        _spriteArray[nodeList[_currentTurn].X, nodeList[_currentTurn].Y].color = Color.green;
    }

}
