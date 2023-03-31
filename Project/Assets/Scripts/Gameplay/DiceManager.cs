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
    None
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
        //initializer la d� par default, 
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
                des0.value = 0; // non lanc�
                des0.id = 0; // non lanc�
                des0.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des1 = new DiceState();
                des1.value = 0; // non lanc�
                des1.id = 1; // non lanc�
                des1.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des2 = new DiceState();
                des2.id = 2; // non lanc�
                des2.value = 0; // non lanc�
                des2.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

                DiceState des3 = new DiceState();
                des3.id = 3; // non lanc�
                des3.value = 0; // non lanc�
                des3.possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };


                DiceState des4 = new DiceState();
                des4.id = 4; // non lanc�
                des4.value = 0; // non lanc�
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

                // Ajouter les n�uds � la liste de n�uds du graphe
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
            Debug.Log(node.state.id+" - Node state: d�" + node.state.id+" value = "+ node.state.value);
            string Res = node.state.id.ToString() + " - Ses valeurs possible: ";
            foreach (var item in node.state.possibleValues)
            {
                Res += item.ToString() + ", ";
            }
            Debug.Log(Res);
            Debug.Log(node.state.id+" - Neighbors: ");
            foreach ((GraphNode neighbor, IntricationMode mode) in node.neighbors)
            {
                Debug.Log(node.state.id + " -->((d� "+neighbor.state.id + " = "+ neighbor.state.value+ ") with mode " + mode.ToString()+")");
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

        // V�rification si les noeuds existent
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

        // Ajout de l'ar�te entre les noeuds
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
                foreach((GraphNode neighbor, IntricationMode mode) in node.neighbors){
                    if (mode.Equals(IntricationMode.Selfish))
                    {
                        neighbor.state.possibleValues.Remove(node.state.possibleValues[0]);
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

        // Ajouter le noeud de d�part dans la queue
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            // Obtenir le premier noeud de la queue
            GraphNode currentNode = queue.Dequeue();

            // Mise � jour de la valeur du DiceState
            if (currentNode.state.Equals(startNode.state))
            {
                currentNode.state.value = newValue;
                //currentNode.state.possibleValues = new List<int> { newValue };
                //changer list valueur possible a lui meme
            }

            // Parcourir tous les voisins du noeud courant
            foreach ((GraphNode neighbor, IntricationMode mode) in currentNode.neighbors)
            {
                // V�rifier si le voisin n'a pas d�j� �t� visit�
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

                    // Marquer le voisin comme visit�
                    //visited.Add(neighbor);

                }
            }
            visited.Add(currentNode);

        }

        UpdateForSelfish();
    }

}

public class DiceLinkGraph
{
    public int[] availableValues; // for each die, an int representing a bit flag of all available values
    public int[] forcedValues;

    public Dictionary<int, int[]> entanglementGraph = new Dictionary<int, int[]>();

    public string ArrayToString<T>(T[] a)
    {
        string listContent = "[";
        for(int i=0; i<a.Length; i++)
        {
            if(i > 0)
                listContent += ",";
            listContent += a[i];
        }
        listContent += "]";
        return listContent;
    }

    public void ComputeEntanglementGraph()
    {
        for(int diceIndex=0; diceIndex<DiceManager.instance.dice.Length; diceIndex++)
        {
            List<int> entanglementList = new List<int>();
            for(int entanglementIndex=0; entanglementIndex<DiceManager.instance.intricationGroups.Length; entanglementIndex++)
            {
                if(DiceManager.instance.intricationGroups[entanglementIndex].diceIndex.Contains<int>(diceIndex))
                {
                    entanglementList.Add(entanglementIndex);
                }
            }
            entanglementGraph[diceIndex] = entanglementList.ToArray();    
            // Debug.Log("dice " + diceIndex + " : " + ArrayToString(entanglementGraph[diceIndex]));
        }
    }

    
    private struct DieEntanglementData
    {
        public int dieIndex;
        public int entanglementIndex;

        public override string ToString()
        {
            return "(" + dieIndex + ", " + entanglementIndex + ")";
        }
    }

    public void ComputeAvailableValues(int[] diceValues)
    {
        availableValues = new int[diceValues.Length];
        forcedValues = new int[diceValues.Length];
        List<DieEntanglementData> entanglementsToHandle = new List<DieEntanglementData>();
        for(int i=0; i<availableValues.Length; i++)
        {
            availableValues[i] = 0xff; // All flags on / All values possible
            if(diceValues[i] > 0)
            {
                forcedValues[i] = diceValues[i] - 1;
                availableValues[i] = 1 << (diceValues[i]-1);
                for(int entanglementIndex = 0; entanglementGraph.ContainsKey(i) && entanglementIndex < entanglementGraph[i].Length; entanglementIndex++)
                {
                    DieEntanglementData entanglementData = new DieEntanglementData();
                    entanglementData.dieIndex = i;
                    entanglementData.entanglementIndex = entanglementGraph[i][entanglementIndex];
                    
                    if(!entanglementsToHandle.Contains(entanglementData))
                    {
                        entanglementsToHandle.Add(entanglementData); // Add all entanglements linked to rolled dice to the todo list
                    }
                }
            }
            else forcedValues[i] = -1;
        }
        while(entanglementsToHandle.Count > 0)
        {
            DieEntanglementData[] toHandle = entanglementsToHandle.ToArray();
            entanglementsToHandle.Clear();
            // Debug.Log("Entanglements To Handle : " + ArrayToString(toHandle));
            foreach(DieEntanglementData entanglementToHandle in toHandle)
            {
                IntricationGroup entanglement = DiceManager.instance.intricationGroups[entanglementToHandle.entanglementIndex];
                int originAvailableValues = availableValues[entanglementToHandle.dieIndex];
                foreach(int dieIndex in entanglement.diceIndex)
                {
                    if(dieIndex != entanglementToHandle.dieIndex)
                    {
                        int oldAvailableValues = availableValues[dieIndex];
                        switch(entanglement.mode)
                        {
                            case IntricationMode.Gregarious:
                                availableValues[dieIndex] &= originAvailableValues;
                                break;
                            case IntricationMode.Opposite:
                                availableValues[dieIndex] &= OppositeValues(originAvailableValues);
                                break;
                            case IntricationMode.Selfish:
                                int valuesToRemove = 0;
                                // array with cell i containing the count of dice that cannot have the value i
                                int[] dieWithoutValueCount = new int[6];
                                foreach(int linkedDieIndex in entanglement.diceIndex)
                                {
                                    if(forcedValues[linkedDieIndex] >= 0 && linkedDieIndex != dieIndex)
                                        valuesToRemove |= 1 << forcedValues[linkedDieIndex];
                                    
                                }

                                availableValues[dieIndex] &= ~valuesToRemove;

                                foreach(int linkedDieIndex in entanglement.diceIndex)
                                {
                                    if(forcedValues[linkedDieIndex] >= 0 && linkedDieIndex != dieIndex)
                                        valuesToRemove |= 1 << forcedValues[linkedDieIndex];
                                    for(int i=0; i<6; i++)
                                    {
                                        if((availableValues[linkedDieIndex] & (1 << i)) == 0) // Check if the linked die can get the value i
                                        {
                                            dieWithoutValueCount[i]++;
                                        }
                                    }
                                }
                                for(int i=0; i<6; i++)
                                {
                                    if(dieWithoutValueCount[i] == 5 && (availableValues[dieIndex] & (1 << i)) > 0)
                                    {
                                        availableValues[dieIndex] = 1 << i;
                                    }
                                }
                                
                                break;
                        }
                        if(oldAvailableValues != availableValues[dieIndex])
                        {
                            foreach(var entanglementIndex in entanglementGraph[dieIndex])
                            {
                                DieEntanglementData entanglementData = new DieEntanglementData();
                                entanglementData.dieIndex = dieIndex;
                                entanglementData.entanglementIndex = entanglementIndex;
                                entanglementsToHandle.Add(entanglementData);
                            }
                        }
                        switch(availableValues[dieIndex])
                        {
                            case 1 << 0:
                                forcedValues[dieIndex] = 0;
                                break;
                            case 1 << 1:
                                forcedValues[dieIndex] = 1;
                                break;
                            case 1 << 2:
                                forcedValues[dieIndex] = 2;
                                break;
                            case 1 << 3:
                                forcedValues[dieIndex] = 3;
                                break;
                            case 1 << 4:
                                forcedValues[dieIndex] = 4;
                                break;
                            case 1 << 5:
                                forcedValues[dieIndex] = 5;
                                break;
                        }
                    }
                }
                
            }
        }
        // Debug.Log("forced values : " + ArrayToString(forcedValues));
    }

    public int[] GetAvailableValues(int dieIndex)
    {
        // Debug.Log("Available Values " + availableValues[dieIndex]);
        List<int> result = new List<int>();
        for(int i=0; i<6; i++)
        {
            if((availableValues[dieIndex] & (1<<i)) > 0)
                result.Add(i);
        }
        return result.ToArray();
    }

    public int OppositeValues(int values)
    {
        int result = 0;
        for(int i=0; i<6; i++)
        {
            if((values & (1 << i)) > 0)
                result |=  1 << (5 - i);
        }
        return result;
    }

    public bool IsGraphValid()
    {
        ComputeEntanglementGraph();
        int[] values = new int[DiceManager.instance.dice.Length];
        for(int i=0; i<values.Length; i++)
            values[i] = 0;
        return CheckValidity(values, 0);
    }

    public bool CheckValidity(int[] values, int cursor)
    {
        if(cursor >= values.Length)
            return true;
        ComputeAvailableValues(values);
        int[] availableValues = GetAvailableValues(cursor);
        foreach(int availableValue in availableValues)
        {
            values[cursor] = availableValue + 1;
            if(CheckValidity(values, cursor + 1))
                return true;
        }
        values[cursor] = -1;
        return false;
    }

    public override string ToString()
    {
        string result = "Forced Values : " + ArrayToString(forcedValues) + "\n";
        result += "Available Values : \n";
        for(int i=0; i<availableValues.Length; i++)
        {
            result += i + " : (";
            for(int j=0; j<6; j++)
            {
                if((availableValues[i] & (1<<j)) > 0)
                    result += (j+1);
            }
            result += ")\n";
        }
        return result;
    }
}

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public LevelConfig config { get { return GameConfigHandler.instance.currentLevel; }}
    public DiceState[] dice;
    public IntricationGroup[] intricationGroups;
    public System.Action diceRollDelegate;
    Graph graphDice;
    DiceLinkGraph linkGraph = new DiceLinkGraph();

    public void Start()
    {
        dice = new DiceState[config.diceCount];
        for (int i = 0; i < config.diceCount; i++)
        {
            dice[i].value = 0;
            dice[i].id = i;
            dice[i].possibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };
        }
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
        Debug.Log("Graph Validity : " + linkGraph.IsGraphValid());
    }

    public bool CheckGraphValidity()
    {
        return linkGraph.IsGraphValid();
    }

    public void Awake()
    {
        instance = this;
    }

    public void AddToIntricationGroup(int groupIndex, int dieIndex)
    {
        int[] oldIndex = intricationGroups[groupIndex].diceIndex;
        int newIndexLength = 1;
        if (oldIndex != null)
            newIndexLength = oldIndex.Length + 1;
        int[] newIndex = new int[newIndexLength];
        if (oldIndex != null)
        {
            for (int i = 0; i < oldIndex.Length; i++)
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
        int[] newIndex = new int[oldIndex.Length - 1];
        int cursor = 0;
        for (int i = 0; i < oldIndex.Length; i++)
        {
            if (oldIndex[i] != dieIndex)
            {
                newIndex[cursor] = oldIndex[i];
                cursor++;
            }
        }
        intricationGroups[groupIndex].diceIndex = newIndex;
    }

    public void RemoveIntricationGroup(int groupIndex)
    {
        IntricationGroup[] oldGroups = intricationGroups;
        IntricationGroup[] newGroups = new IntricationGroup[intricationGroups.Length - 1];
        int cursor = 0;
        for(int i=0; i<oldGroups.Length; i++)
        {
            if(i != groupIndex)
            {
                newGroups[cursor] = oldGroups[i];
                cursor++;
            }
        }
        intricationGroups = newGroups;
    }
    public void RollDice(int diceIndex)
    {
        Debug.Log($"Lancer du dés {diceIndex} !");
        dice[diceIndex].value = -1;
        linkGraph.ComputeEntanglementGraph();
        int[] diceValues = new int[DiceManager.instance.dice.Length];
        for(int i=0; i<diceValues.Length; i++)
            diceValues[i] = DiceManager.instance.dice[i].value;
        linkGraph.ComputeAvailableValues(diceValues);

        // System.Random random = new System.Random();
        //Debug.Log(graphDice.nodes[diceIndex].state.possibleValues);
        // int index = random.Next(graphDice.nodes[diceIndex].state.possibleValues.Count);

        // graphDice.BreadthFirstSearch(graphDice.nodes[diceIndex], graphDice.nodes[diceIndex].state.possibleValues[index]);
        int[] availableValues = linkGraph.GetAvailableValues(diceIndex);
        int selectedValue = availableValues[UnityEngine.Random.Range(0, availableValues.Length)];
        dice[diceIndex].value = selectedValue + 1;
        diceValues[diceIndex] = selectedValue + 1;
        Debug.Log(linkGraph);
        linkGraph.ComputeEntanglementGraph();
        linkGraph.ComputeAvailableValues(diceValues);
        Debug.Log(linkGraph);
        Debug.Log("Selected value : " + dice[diceIndex].value);

        diceRollDelegate?.Invoke();

        Debug.Log("GAGNER "+ Victory());
    }

    public bool CheckVictoryCondition(List<VictoryCondition> condition)
    {
        bool IsVictory = true;
        foreach(VictoryCondition cond in config.conditions){

            switch (cond.vc)
                    {
                        case VictoryConditionType.NSupSum:
                            int sum = 0;
                            for(int i=0; i<dice.Length; i++)
                            {
                                sum += dice[i].value;
                            }
                            IsVictory &= sum >= cond.value;
                            break;

                        case VictoryConditionType.AllDifferentValues:
                            IsVictory &= dice.Where(d => d.value != 0).Select(d => d.value).Distinct().Count() == cond.N;
                            break;

                        case VictoryConditionType.NAllSame:
                            IsVictory &= dice.Where(d => d.value != 0).GroupBy(d => d.value).Any(g => g.Count() == cond.N);
                            break;
                        case VictoryConditionType.MixedValues:
                            int countSpecificValues = dice.Where(d => d.value != 0).Count(d => d.value == cond.value);
                            int countOtherValues = dice.Length - countSpecificValues;
                            IsVictory &= countSpecificValues == cond.N && countOtherValues == dice.Length - cond.N;
                            break;
                        case VictoryConditionType.PairValues:
                            int countPairValues = dice.Count(d => (d.value % 2 == 0 && d.value !=0));
                            IsVictory &= countPairValues == cond.N;
                            break;
                        case VictoryConditionType.ImpairValues:
                            int countImpairValues = dice.Count(d => d.value % 2 == 1);
                            IsVictory &= countImpairValues == cond.N;
                            break;
                        case VictoryConditionType.NSumEqual:
                            var combinations = GetCombinations(dice.Where(d => d.value != 0).Select(d => d.value), cond.N);
                            IsVictory &= combinations.Any(c => c.Sum() == cond.value);
                            break;
                        case VictoryConditionType.NOccurence:
                            IsVictory &= dice.Where(d => d.value != 0)
                                                    .GroupBy(d => d.value)
                                               .Count(g => g.Count() >= cond.N) >= 2
                                               ||
                                         dice.Where(d => d.value != 0)
                                         .GroupBy(d => d.value)
                                               .Count(g => g.Count() >= 2*cond.N) >= 1
                                               ;
                            break;

                default:
                            Debug.LogError("Unknown victory condition: " + condition);
                            break;
            }
        }

        return IsVictory;
        
    }

    public bool Victory()
    {
        return CheckVictoryCondition(config.conditions);
    }
    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int n)
    {
        if (n == 0)
            return new[] { new T[0] };
        else
        {
            return list.SelectMany((e, i) =>
                GetCombinations(list.Skip(i + 1), n - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }
}
