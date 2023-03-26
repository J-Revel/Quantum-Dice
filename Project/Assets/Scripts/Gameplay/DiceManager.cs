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
    public List<int> possibleValues;
}

public enum IntricationMode
{
    Gregarious, // same value
    Opposite, // 2 dice only => Opposite values : result sum = 7 (1-6, 2-5, 3-4)
    Selfish, // All values are different
    None,
}

[System.Serializable]
public struct IntricationGroup
{
    public IntricationMode mode;
    public int[] diceIndex;
}


public class GraphNode
{
    //mettre DiceState ou index
    public DiceState state; 
    public List<(GraphNode, IntricationMode)> neighbors;

    public GraphNode(DiceState state)
    {
        this.state = state;
        neighbors = new List<(GraphNode, IntricationMode)>();
    }

    public void Reset()
    {
        //initializer la dé par default, 
        state.value = 0;
        state.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

        removeAllIntrication(); //A ajouter ?
    }
    public void removeAllIntrication()
    {
        // Elle enleves toute ses intrication 
        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbors[i].Item1.neighbors.Remove((this, IntricationMode.Gregarious));
            neighbors[i].Item1.neighbors.Remove((this, IntricationMode.Opposite));
            neighbors[i].Item1.neighbors.Remove((this, IntricationMode.Selfish));
        }

        neighbors = new List<(GraphNode, IntricationMode)>();
    }
}

public class Graph
{
    public List<GraphNode> nodes;

    public Graph()
    {
        nodes = new List<GraphNode>();
        /*        nodes = new List<GraphNode>();
                List<int> initializeList = new List<int> { 1, 2, 3, 4, 5, 6 };
                DiceState des0 = new DiceState();
                des0.value = 0; // non lancé
                des0.id = 0; // non lancé
                des0.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des1 = new DiceState();
                des1.value = 0; // non lancé
                des1.id = 1; // non lancé
                des1.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des2 = new DiceState();
                des2.id = 2; // non lancé
                des2.value = 0; // non lancé
                des2.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des3 = new DiceState();
                des3.id = 3; // non lancé
                des3.value = 0; // non lancé
                des3.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };


                DiceState des4 = new DiceState();
                des4.id = 4; // non lancé
                des4.value = 0; // non lancé
                des4.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                GraphNode node0 = new GraphNode(des0);
                GraphNode node1 = new GraphNode(des1);
                GraphNode node2 = new GraphNode(des2);
                GraphNode node3 = new GraphNode(des3);
                GraphNode node4 = new GraphNode(des4);

                // Ajouter les voisins de chaque noeud
                node0.neighbors.Add((node1, IntricationMode.Selfish));
                node0.neighbors.Add((node2, IntricationMode.Selfish));
                node0.neighbors.Add((node2, IntricationMode.Opposite));

                node1.neighbors.Add((node0, IntricationMode.Selfish));
                node1.neighbors.Add((node2, IntricationMode.Selfish));

                node2.neighbors.Add((node0, IntricationMode.Selfish));
                node2.neighbors.Add((node0, IntricationMode.Opposite));
                node2.neighbors.Add((node1, IntricationMode.Selfish));

                node3.neighbors.Add((node4, IntricationMode.Gregarious));

                node4.neighbors.Add((node3, IntricationMode.Gregarious));

                // Ajouter les nœuds à la liste de nœuds du graphe
                nodes.Add(node0);
                nodes.Add(node1);
                nodes.Add(node2);
                nodes.Add(node3);
                nodes.Add(node4);

                //Debug.Log(nodes);*/
    }
    public void PrintGraph()
    {
        foreach (GraphNode node in nodes)
        {
            Debug.Log(node.state.id+" - Node state: dé" + node.state.id+" value = "+ node.state.value);
            string Res = node.state.id.ToString() + " - Ses valeurs possible: ";
            foreach (var item in node.state.possibleValues)
            {
                Res += item.ToString() + ", ";
            }
            Debug.Log(Res);
            Debug.Log(node.state.id+" - Neighbors: ");
            foreach ((GraphNode neighbor, IntricationMode mode) in node.neighbors)
            {
                Debug.Log(node.state.id + " -->((dé "+neighbor.state.id + " = "+ neighbor.state.value+ ") with mode " + mode.ToString()+")");
            }
            Debug.Log(node.state.id + " ---------------------------------------");
        }
    }

    public void AddNode(DiceState state)
    {
        GraphNode node = new GraphNode(state);
        nodes.Add(node);
    }

    public void Reset()
    {
        //
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Reset();
        }
        BreadthFirstSearch(nodes[0], 0);

    }
    //1 un lien avec un sommet avec un sommet voisin gregarious on ne peut pas ajouter un lien S ou O avec le meme voisin

    //2 un lien avec un sommet avec un sommet voisin gregarious on ne peut pas avoir un lien S avec aucun autre voisin

    //3 un sommet peut au plus avoir 5 voisin S.

    //4 un lien avec un sommet avec un sommet voisin S ne peut pas avoir 2 lien avec des voisins de type 0
    //un sommet qq ne peut avoir 2 voisin O
    public void AddEdge(DiceState state1, DiceState state2, IntricationMode mode)
    {
        // Recherche des noeuds correspondants aux DiceStates
        GraphNode node1 = nodes.Find(n => n.state.id == state1.id);
        GraphNode node2 = nodes.Find(n => n.state.id == state2.id);

        // Vérification si les noeuds existent
        if (node1 == null)
        {
            node1 = new GraphNode(state1);
            nodes.Add(node1);
        }
        if (node2 == null)
        {
            node2 = new GraphNode(state2);
            nodes.Add(node2);
        }

        // Ajout de l'arête entre les noeuds
        node1.neighbors.Add((node2, mode));
        node2.neighbors.Add((node1, mode));
    }

    public void AddEdgesFromGroups(IntricationGroup[] groups)
    {
        foreach (IntricationGroup group in groups)
        {
            for (int i = 0; i < group.diceIndex.Length; i++)
            {
                GraphNode node1 = nodes[group.diceIndex[i]];
                for (int j = i + 1; j < group.diceIndex.Length; j++)
                {
                    GraphNode node2 = nodes[group.diceIndex[j]];
                    node1.neighbors.Add((node2, group.mode));
                    node2.neighbors.Add((node1, group.mode));
                }
            }
        }
    }

    GraphNode getNodes(DiceState state)
    {
        foreach (GraphNode gn in nodes)
        {
            if (gn.state.Equals(state))
            {
                return gn;
            }
        }
        return null;
    }

    public void UpdateForSelfish()
    {
        GraphNode graphNode = nodes[0];
        foreach (GraphNode node in nodes){
            if(node.state.value == 0 && node.state.possibleValues.Count == 1)
            {
                foreach((GraphNode neighbord, IntricationMode mode) in node.neighbors){
                    if (mode.Equals(IntricationMode.Selfish))
                    {
                        neighbord.state.possibleValues.Remove(node.state.possibleValues[0]);
                    }
                }
            }
        }
    }

    public void BreadthFirstSearch( GraphNode startNode, int newValue)
    {
        // Initialisation des variables
        Queue<GraphNode> queue = new Queue<GraphNode>();
        HashSet<GraphNode> visited = new HashSet<GraphNode>();

        // Ajouter le noeud de départ dans la queue
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            // Obtenir le premier noeud de la queue
            GraphNode currentNode = queue.Dequeue();

            // Mise à jour de la valeur du DiceState
            if (currentNode.state.Equals(startNode.state))
            {
                currentNode.state.value = newValue;
                //currentNode.state.possibleValues = new List<int> { newValue };
                //changer list valueur possible a lui meme
            }

            // Parcourir tous les voisins du noeud courant
            foreach ((GraphNode neighbor, IntricationMode mode) in currentNode.neighbors)
            {
                // Vérifier si le voisin n'a pas déjà été visité
                if (!visited.Contains(neighbor))
                {
                    if (mode.Equals(IntricationMode.Gregarious))
                    {
                        neighbor.state.possibleValues = new List<int> { currentNode.state.value};
                    }
                    if (mode.Equals(IntricationMode.Selfish))
                    {
                        neighbor.state.possibleValues.Remove(currentNode.state.value);   
                    }
                    if (mode.Equals(IntricationMode.Opposite))
                    {
                        neighbor.state.possibleValues = new List<int> { 7-currentNode.state.value };
                    }
                    // Ajouter le voisin dans la queue
                    queue.Enqueue(neighbor);

                    // Marquer le voisin comme visité
                    //visited.Add(neighbor);

                }
            }
            visited.Add(currentNode);

        }

        UpdateForSelfish();
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
        foreach(DiceState ds in dice)
        {
            graphDice.AddNode(ds);
        }
        graphDice.AddEdgesFromGroups(intricationGroups);

    }

    public void ValidateGraph()
    {
        graphDice.AddEdgesFromGroups(intricationGroups);
    }

    public void Awake()
    {
        instance = this;
        dice = new DiceState[config.diceCount];
        for(int i=0; i<config.diceCount; i++)
        {
            dice[i].value = 0;
            dice[i].id = i;
            dice[i].possibleValues = new List<int> { 1,2,3,4,5,6};
        }
    }

    public void RollDice(int diceIndex)
    {
        Debug.Log($"Lancer du dés {diceIndex} !");

        System.Random random = new System.Random();
        Debug.Log(graphDice.nodes[diceIndex].state.possibleValues);
        int index = random.Next(graphDice.nodes[diceIndex].state.possibleValues.Count);

        graphDice.BreadthFirstSearch(graphDice.nodes[diceIndex], graphDice.nodes[diceIndex].state.possibleValues[index]);
        for(int i = 0; i < dice.Length; i++)
        {
            dice[i] = graphDice.nodes[i].state;
        }
        graphDice.PrintGraph();
        Debug.Log("#############################################");
        diceRollDelegate?.Invoke();
    }

    public void AddToIntricationGroup(int groupIndex, int dieIndex)
    {
        int[] oldIndex = intricationGroups[groupIndex].diceIndex;
        int newIndexLength = 1;
        if(oldIndex != null)
            newIndexLength = oldIndex.Length+1;
        int[] newIndex = new int[newIndexLength];
        if(oldIndex != null)
        {
            for(int i=0; i<oldIndex.Length; i++)
            {
                newIndex[i] = oldIndex[i];
            }
        }
        newIndex[newIndexLength - 1] = dieIndex;
        intricationGroups[groupIndex].diceIndex = newIndex;
    }

    public void RemoveFromIntricationGroup(int groupIndex, int dieIndex)
    {
        int[] oldIndex = intricationGroups[groupIndex].diceIndex;
        int[] newIndex = new int[oldIndex.Length-1];
        int cursor = 0;
        for(int i=0; i<oldIndex.Length; i++)
        {
            if(oldIndex[i] != dieIndex)
            {
                newIndex[cursor] = oldIndex[i];
                cursor++;
            }
        }
        intricationGroups[groupIndex].diceIndex = newIndex;
    }
}
