using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct DiceState
{
    public int id; // a supprimer
    public int value; // 0 = not rolled, 1->6 = face visible
    
    List<int> valuePossible;
    List<int> indexNeighbor;
    
}

public enum IntricationMode
{
    Gregarious, // same value
    Opposite, // 2 dice only => Opposite values : result sum = 7 (1-6, 2-5, 3-4)
    Selfish, // All values are different
}

[System.Serializable]
public struct IntricationGroup
{
    public IntricationMode mode;
    public int[] diceIndex;
}


public class GraphNode
{
    public DiceState state;
    public List<(GraphNode, IntricationMode)> neighbors;

    public GraphNode(DiceState state)
    {
        this.state = state;
        neighbors = new List<(GraphNode, IntricationMode)>();
    }
}

public class Graph
{
    public List<GraphNode> nodes;

    public Graph()
    {
        nodes = new List<GraphNode>();

        DiceState des0 = new DiceState();
        des0.value = 0; // non lanc�
        des0.id = 0; // non lanc�

        DiceState des1 = new DiceState();
        des1.value = 0; // non lanc�
        des1.id = 1; // non lanc�

        DiceState des2 = new DiceState();
        des2.id = 2; // non lanc�
        des2.value = 0; // non lanc�

        DiceState des3 = new DiceState();
        des3.id = 3; // non lanc�
        des3.value = 0; // non lanc�
        
        GraphNode node0 = new GraphNode(des0);
        GraphNode node1 = new GraphNode(des1);
        GraphNode node2 = new GraphNode(des2);
        GraphNode node3 = new GraphNode(des3);

        // Ajouter les voisins de chaque noeud
        node0.neighbors.Add((node1, IntricationMode.Selfish));
        node0.neighbors.Add((node2, IntricationMode.Selfish));
        node0.neighbors.Add((node2, IntricationMode.Opposite));

        node1.neighbors.Add((node0, IntricationMode.Selfish));
        node1.neighbors.Add((node2, IntricationMode.Selfish));
        node1.neighbors.Add((node3, IntricationMode.Gregarious));

        node2.neighbors.Add((node0, IntricationMode.Selfish));
        node2.neighbors.Add((node0, IntricationMode.Opposite));
        node2.neighbors.Add((node1, IntricationMode.Selfish));

        node3.neighbors.Add((node1, IntricationMode.Gregarious));

        // Ajouter les n�uds � la liste de n�uds du graphe
        nodes.Add(node0);
        nodes.Add(node1);
        nodes.Add(node2);
        nodes.Add(node3);

        //Debug.Log(nodes);
    }
    public void PrintGraph()
    {
        foreach (GraphNode node in nodes)
        {
            Debug.Log("Node state: d�" + node.state.id);
            Debug.Log("Neighbors: ");
            foreach ((GraphNode neighbor, IntricationMode mode) in node.neighbors)
            {
                Debug.Log("->(d� "+neighbor.state.id + " with mode " + mode.ToString()+")");
            }
            Debug.Log("--------------------------------------");
        }
    }
}





public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public LevelConfig config;
    public DiceState[] dice;
    public IntricationGroup[] intricationGroups;
    public System.Action diceRollDelegate;
    Graph graphDice;

    public void Start()
    {
        graphDice = new Graph();
        //PrintGraph(graphDice);
        graphDice.PrintGraph();
        Debug.Log($"GapheDice.nodes.Count() = {graphDice.nodes.Count()}");

    }

    public void Awake()
    {
        instance = this;
        dice = new DiceState[config.diceCount];
        for(int i=0; i<config.diceCount; i++)
            dice[i].value = 0;
    }

    public void RollDice(int diceIndex)
    {
        List<int> valuesPossible = new List<int> { 1, 2, 3, 4, 5, 6 };
        Debug.Log($"Lancer du d�s {diceIndex} !");
    
        for (int i = 0; i < intricationGroups.Length; i++ )
        {
            int[] dicesIndex = intricationGroups[i].diceIndex;

            //verifier s'il est contenue dans la liste diceIndex
            Debug.Log($"Groupe {i} !");

            string dicesIndexRes = "List dicesIndex before: ";
            foreach (var item in dicesIndex)
            {
                dicesIndexRes += item.ToString() + ", ";
            }
            Debug.Log(dicesIndexRes);

            if (Array.IndexOf(dicesIndex, diceIndex) != -1)
            {               
                int[] dicesIndexCopy = dicesIndex.Except(new int[] { diceIndex }).ToArray();

                string result1 = "List valuesPossible before: ";
                foreach (var item in valuesPossible)
                {
                    result1 += item.ToString() + ", ";
                }
                Debug.Log(result1);

                if (intricationGroups[i].mode == IntricationMode.Selfish)
                {
                    Debug.Log("selfish");
                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        valuesPossible.Remove(dice[dicesIndexCopy[k]].value);
                    }
                }
                if (intricationGroups[i].mode == IntricationMode.Gregarious)
                {
                    Debug.Log($"Greagrious");
                   
                    for (int k = 0; k < dicesIndexCopy.Length; k++) // a achanger pour une boucle while
                    {
                        if(dice[dicesIndexCopy[k]].value != 0)
                        {
                            valuesPossible = new List<int> { dice[dicesIndexCopy[k]].value };
                        }
                    }
                }
                if (intricationGroups[i].mode == IntricationMode.Opposite)
                {
                    Debug.Log($"Opposite");

                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        if (dice[dicesIndexCopy[k]].value != 0)
                        {
                            int tmp = 7 - dice[dicesIndexCopy[k]].value;
                            valuesPossible = new List<int> { tmp };
                        }
                    }
                }
            }
        }
        System.Random random = new System.Random();
        int index = random.Next(valuesPossible.Count);

        dice[diceIndex].value = valuesPossible[index];

        diceRollDelegate?.Invoke();
    }
}
