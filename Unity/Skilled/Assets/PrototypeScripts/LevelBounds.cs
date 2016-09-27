﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelBounds : MonoBehaviour {

    Bounds bounds;
    GameObject[] players;
    List<GameObject> objects;
    bool _requestFindPlayers = false;
    List<WrapClones> cloneList;


    class WrapClones
    {
        GameObject[] clones;
        GameObject original;
        Bounds levelBounds;
        public WrapClones(GameObject original, Bounds levelBounds)
        {
            this.original = original;
            this.levelBounds = levelBounds;
            clones = new GameObject[3]; //max 3 clones for each corner +original
            for (int i = 0; i < clones.Length; i++)
            {
                GameObject clone = GameObject.Instantiate(this.original);
                GameObject.Destroy(clone.GetComponent<LoopOutLevel>());
                GameObject.Destroy(clone.GetComponent<Rigidbody2D>());
                GameObject.Destroy(clone.GetComponent<PlayerMovement>());
                GameObject.Destroy(clone.GetComponent<PowerupUser>());
                GameObject.Destroy(clone.GetComponent<PlayerHit>());    //TODO KEEP THIS           
                clones[i] = clone;
            }
            foreach (GameObject clone in clones)
            {
                clone.transform.parent = original.transform;
            }
        }
        public void UpdateClones()
        {
            Vector2 diff = (Vector2)original.transform.position - (Vector2)levelBounds.center;

                float x0 =  diff.x < 0? levelBounds.size.x : -levelBounds.size.x;
                clones[0].transform.localPosition = new Vector2(x0, 0);

                float x1 = diff.x < 0 ? levelBounds.size.x : -levelBounds.size.x;
                float y1 = diff.y < 0 ? levelBounds.size.y : -levelBounds.size.y;
                clones[1].transform.localPosition = new Vector2(x1, y1);

            float x2 = 0;
            float y2 = diff.y < 0 ? levelBounds.size.y : -levelBounds.size.y;
            clones[2].transform.localPosition = new Vector2(x2, y2);



        }
    }

    public static LevelBounds Instance { private set; get; }
    public void RegisterObject(GameObject g)
    {
        objects.Add(g);
        cloneList.Add(new WrapClones(g, bounds));
    }
    public void UnRegisterObject(GameObject g)
    {
        //TODO REMOVE CLONES
        objects.Remove(g);
    }

    bool CheckBounds(GameObject obj, bool checkonly = false)
    {
        //List<GameObject> newTemps = new List<GameObject>();
        bool inbounds = false;
        //Bounds objBounds = obj.GetComponent<SpriteRenderer>().bounds;
        //float halfY = objBounds.size.y / 2f;
        //float halfX = objBounds.size.x / 2f;
        /*
        if (obj.transform.position.y - halfY > bounds.center.y - bounds.size.y / 2f &&
            obj.transform.position.y + halfY < bounds.center.y + bounds.size.y / 2f &&

            obj.transform.position.x - halfX > bounds.center.x - bounds.size.x / 2f &&
            obj.transform.position.x + halfX < bounds.center.y + bounds.size.x / 2f)
            inbounds = true;*/
        //below bounds
        //newTemps.Add(obj);
        if (obj.transform.position.y <= bounds.center.y - bounds.size.y / 2f)
        {
            if (!checkonly)
            {
                //GameObject obj2 = (GameObject)GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
                //GameObject.Destroy(obj2.GetComponent<LoopOutLevel>());
                //newTemps.Add(obj2);
                obj.transform.position += new Vector3(0, bounds.size.y, 0);
            }
        }
        //above bounds
        if (obj.transform.position.y >= bounds.center.y + bounds.size.y / 2f)
        {
            if (!checkonly)
            {
               // GameObject obj2 = (GameObject)GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
               // GameObject.Destroy(obj2.GetComponent<LoopOutLevel>());
                //newTemps.Add(obj2);
                obj.transform.position += new Vector3(0, -bounds.size.y, 0);        
            }
        }

        //left of bounds
        if (obj.transform.position.x <= bounds.center.x - bounds.size.x / 2f)
        {
            if (!checkonly)
            {
               // GameObject obj2 = (GameObject)GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
               // GameObject.Destroy(obj2.GetComponent<LoopOutLevel>());
                //newTemps.Add(obj2);
                obj.transform.position += new Vector3(bounds.size.x, 0, 0);
            }
        }
        //right of bounds
        if (obj.transform.position.x >= bounds.center.x + bounds.size.x / 2f)
        {
            if (!checkonly)
            {
               // GameObject obj2 = (GameObject)GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
               // GameObject.Destroy(obj2.GetComponent<LoopOutLevel>());
                //newTemps.Add(obj2);
                obj.transform.position += new Vector3(-bounds.size.x, 0, 0);
            }
        }
        //if(newTemps.Count > 1)
       //     tempObjects.Add(newTemps);
        return inbounds;
    }
    void Update()
    {
        //TODO OPTIMIZE THIS
        foreach(GameObject obj in objects)
        {
            CheckBounds(obj);
        }
        foreach(WrapClones clones in cloneList)
        {
            clones.UpdateClones();
        }
        /*
        foreach(List<GameObject> temp in tempObjects)
        {
            if(CheckBounds(temp[0], true))
            {
                for (int i = temp.Count-1; i > 0; i--)
                {
                    GameObject.Destroy(temp[i]);
                }
            }
        }*/
        /*
        foreach(GameObject player in players)
        {
            if (_requestFindPlayers) FindPlayers();
            if(player == null)
            {
                _requestFindPlayers = true;
                continue;
            }

            //below bounds
            if(player.transform.position.y <= bounds.center.y - bounds.size.y/2f)
            {
                player.transform.position += new Vector3(0, bounds.size.y, 0);
            }
            //above bounds
            if (player.transform.position.y >= bounds.center.y + bounds.size.y / 2f)
            {
                player.transform.position += new Vector3(0, -bounds.size.y, 0);
            }

            //left of bounds
            if (player.transform.position.x <= bounds.center.x - bounds.size.x / 2f)
            {
                player.transform.position += new Vector3(bounds.size.x, 0, 0);
            }
            //right of bounds
            if (player.transform.position.x >= bounds.center.x + bounds.size.x / 2f)
            {
                player.transform.position += new Vector3(-bounds.size.x, 0, 0);
            }
        }*/
    }

    public void FindPlayers()
    {
        PlayerHit[] hits = GameObject.FindObjectsOfType<PlayerHit>();
        players = new GameObject[hits.GetLength(0)];
        for (int i = 0; i < hits.GetLength(0); i++)
        {
            players[i] = hits[i].gameObject;
        }
        _requestFindPlayers = false;
    }

    void Awake()
    {
        //FindPlayers();
        bounds = GetComponent<BoxCollider2D>().bounds;
        objects = new List<GameObject>();
        cloneList = new List<WrapClones>();
        Instance = this;
    }
	
}
