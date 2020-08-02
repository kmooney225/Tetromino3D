using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tetromino : MonoBehaviour
{

    float prevTime;
    float fallTime = 1.4f;
    
    void Start()
    {
        ButtonInputs.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidMove())
        {
            GameManager.instance.SetGameIsOver();
            FindObjectOfType<AudioManager>().Play("GameOver");
        }
    }
    void Update()
    {
        if(Time.time - prevTime > fallTime)
        {
            transform.position += Vector3.down;

            
            if (!CheckValidMove())
            {
                transform.position += Vector3.up;
                //DELETE LAYER IF POSSIBLE
                Playfield.instance.DeleteLayer();
                enabled = false;
                //CREATE A NEW TETRIS BLOCK

                if (!GameManager.instance.ReadGameIsOver())
                {
                    FindObjectOfType<AudioManager>().Play("Impact");
                    Playfield.instance.SpawnNewBlock();

                }
            }
            else
            {
                //UPDATE THE GRID
                Playfield.instance.UpdateGrid(this);
            }
            prevTime = Time.time;
        }

        //Movement with buttons-----------------------
        if (Input.GetKeyDown(KeyCode.A))
        {
            setInput(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            setInput(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            setInput(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            setInput(Vector3.back);
        }


        //X Rotation----------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setRotationInput(new Vector3(90, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setRotationInput(new Vector3(-90, 0, 0));
        }


        //Y Rotation-----------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setRotationInput(new Vector3(0, 90, 0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            setRotationInput(new Vector3(0, -90, 0));
        }


        //Z Rotation--------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            setRotationInput(new Vector3(0, 0, 90));
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            setRotationInput(new Vector3(0, 0, -90));
        }
    }

    public void setInput(Vector3 direction)
    {
        transform.position += direction;
        if(!CheckValidMove())
        {
            transform.position -= direction;
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
            FindObjectOfType<AudioManager>().Play("Turn");
        }
    }

    public void setRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation,Space.World);
        if (!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
            FindObjectOfType<AudioManager>().Play("Turn");
        }
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            if(!Playfield.instance.CheckInsideGrid(pos))
            {
                return false;
            }
        }

        foreach(Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            Transform t = Playfield.instance.GetTransformOnGridPos(pos);
            if(t!=null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
