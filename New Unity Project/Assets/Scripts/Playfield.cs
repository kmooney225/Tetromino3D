using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class Playfield : MonoBehaviour
{

    public static Playfield instance;


    public int gridSizeX, gridSizeY, gridSizeZ;
    [Header("Tetriminos")]
    public GameObject[] blockList;
    public GameObject[] ghostList;

    [Header("Playfield Visuals")]
    public GameObject bottomPlane;
    public GameObject N, S, W, E;

    int randomIndex;

    public Transform[,,] theGrid;


    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        SpawnNewBlock();
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x),
                            Mathf.RoundToInt(vec.y),
                             Mathf.RoundToInt(vec.z));
    }

    public bool CheckInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX &&
                (int)pos.z >= 0 && (int)pos.z < gridSizeZ &&
                (int)pos.y >= 0); 
    }

    public void UpdateGrid(Tetromino block)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for(int y = 0; y < gridSizeY; y++)
                {
                    if(theGrid[x,y,z]!=null)
                    {
                        if (theGrid[x, y, z].parent == block.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        foreach(Transform child in block.transform)
        {
            Vector3 pos = Round(child.position);
            if(pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }
    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if(pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public void SpawnNewBlock()
    {
        Vector3 spawnPoint = new Vector3((int)(transform.position.x + (float)gridSizeX / 2),
                                         (int)transform.position.y + gridSizeY,
                                         (int)(transform.position.z + (float)gridSizeZ / 2));


        //Spawn The Block
        GameObject newBlock = Instantiate(blockList[randomIndex], spawnPoint, Quaternion.identity)as GameObject;
        //Ghost
        GameObject newGhost = Instantiate(ghostList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;
        newGhost.GetComponent<Ghost>().setParent(newBlock);

        CalculatePreview();
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);
        Previewer.instance.ShowPreview(randomIndex);
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for(int y = gridSizeY-1; y >= 0; y--)
        {
            //Check full Layer
            if (CheckFullLayer(y))
            {
                layersCleared++;
                //Delete some blocks
                DeleteLayerAt(y);
                MoveAllLayerDown(y);
                //Move all Down By 1
            }
        }
        if(layersCleared > 0)
        {
            GameManager.instance.LayersCleared(layersCleared);
        }
    }

    bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(theGrid[x, y, z].gameObject);
                FindObjectOfType<AudioManager>().Play("ClearLayer");

                theGrid[x, y, z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] != null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if(bottomPlane != null)
        {
            //RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float)gridSizeX/10, 1, (float)gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;

            //REPOSITION
            bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX/2,
                                                         transform.position.y,
                                                         transform.position.z + (float)gridSizeZ/2);

            //RETILE MATERIAL
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }

        if (N != null)
        {
            //RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            N.transform.localScale = scaler;

            //REPOSITION
            N.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y +(float)gridSizeY/2,
                                                         transform.position.z + gridSizeZ);

            //RETILE MATERIAL
            N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (S != null)
        {
            //RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            S.transform.localScale = scaler;

            //REPOSITION
            S.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z);

            //RETILE MATERIAL
            //S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }


        if (E != null)
        {
            //RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            E.transform.localScale = scaler;

            //REPOSITION
            E.transform.position = new Vector3(transform.position.x + gridSizeX,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + (float)gridSizeZ/2);

            //RETILE MATERIAL
            E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if (W != null)
        {
            //RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            W.transform.localScale = scaler;

            //REPOSITION
            W.transform.position = new Vector3(transform.position.x,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + (float)gridSizeZ / 2);

            //RETILE MATERIAL
            //W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

    }
}
