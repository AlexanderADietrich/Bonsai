using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSceneNode : SceneNode
{
    public Material nodeMaterial = null;
    public Shader nodeShader = null;
    int growthRate = 150;
    int current = 0;
    int gen = 0;
    static int MAX_GEN = 7;
    GameObject child1;
    GameObject child2;

    // Update is called once per frame
    void Update()
    {
        current++;
        if (current >= growthRate)
            if (child1 == null && child2 == null && gen < MAX_GEN)
            {
                current = 0;
                grow();
            }
    }

    void grow()
    {
        child1 = createChild(true);
        child2 = createChild(false);
    }

    public static float decayRate = 0.9f;
    GameObject createChild(bool negate)
    {
        //initialize node
        GameObject c1 = new GameObject();
        c1.name = "Child";
        c1.AddComponent<GrowingSceneNode>().PrimitiveList = new List<NodePrimitive>();
        c1.GetComponent<GrowingSceneNode>().nodeMaterial = nodeMaterial;
        c1.GetComponent<GrowingSceneNode>().nodeShader = nodeShader;
        c1.GetComponent<GrowingSceneNode>().gen = gen + 1;
        c1.transform.parent = this.transform;
        //initialize game object
        GameObject c1geom = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        c1geom.name = "Geometry";
        c1geom.AddComponent<NodePrimitive>().Pivot = new Vector3(0, -1, 0);
        //connect node and game obect
        GameObject geom = new GameObject();
        geom.name = "Geom";
        geom.transform.parent = c1.transform;
        c1geom.transform.parent = geom.transform;
        c1.GetComponent<GrowingSceneNode>().PrimitiveList.Add(c1geom.GetComponent<NodePrimitive>());
        //change scale/rotation/material/location/origin position/shader
        c1.transform.localRotation = Quaternion.Euler(0, 0, (negate) ? -45 : 45);
        c1.transform.localScale = transform.localScale * decayRate;
        c1.transform.localPosition = transform.localPosition;
        c1.GetComponent<GrowingSceneNode>().NodeOrigin = new Vector3(((negate) ? 1 : -1) * Mathf.Sqrt(2) * 0.5f * decayRate, transform.localScale.y + Mathf.Sqrt(2) * 0.5f * decayRate, 0);
        nodeMaterial.shader = nodeShader;
        c1geom.GetComponent<Renderer>().material = nodeMaterial;

        return c1;
    }
}
