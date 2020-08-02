using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs instance;

    public GameObject[] rotateCanvases;
    public GameObject moveCanvas;

    GameObject activeBlock;
    Tetromino activeTetris;

    bool moveIsOn = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetInputs();
    }

    void RepositionToActiveBlock()
    {
        if(activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, Tetromino tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }

    void Update()
    {
        RepositionToActiveBlock();
    }

    public void MoveBlock(string direction)
    {
        if(activeBlock != null)
        {
            if(direction == "left")
            {
                activeTetris.setInput(Vector3.left);
            }

            if (direction == "right")
            {
                activeTetris.setInput(Vector3.right);
            }

            if (direction == "forward")
            {
                activeTetris.setInput(Vector3.forward);
            }

            if (direction == "back")
            {
                activeTetris.setInput(Vector3.back);
            }
        }
    }

    public void RotateBlock(string rotation)
    {
        if (activeBlock != null)
        {
            //X Rotation
            if(rotation == "posX")
            {
                activeTetris.setRotationInput(new Vector3(90, 0, 0));
            }
            if (rotation == "negX")
            {
                activeTetris.setRotationInput(new Vector3(-90, 0, 0));
            }

            //Y Rotation
            if (rotation == "posY")
            {
                activeTetris.setRotationInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY")
            {
                activeTetris.setRotationInput(new Vector3(0, -90, 0));
            }

            //Z Rotation
            if (rotation == "posZ")
            {
                activeTetris.setRotationInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ")
            {
                activeTetris.setRotationInput(new Vector3(0, 0, -90));
            }
        }
    }

    public void SwitchInputs()
    {
        moveIsOn = !moveIsOn;
        SetInputs();
    }

    void SetInputs()
    {
        moveCanvas.SetActive(moveIsOn);
        foreach (GameObject c in rotateCanvases)
        {
            c.SetActive(!moveIsOn);
        }
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}
