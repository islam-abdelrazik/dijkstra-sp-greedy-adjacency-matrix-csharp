

TextWriter textWriter = new StreamWriter("output.txt", true);

int t = Convert.ToInt32(Console.ReadLine().Trim());

if (t < 1 || t > 10)
{
    throw new ArgumentOutOfRangeException();
}

for (int tItr = 0; tItr < t; tItr++)
{
    string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

    int n = Convert.ToInt32(firstMultipleInput[0]);

    int m = Convert.ToInt32(firstMultipleInput[1]);

    List<List<int>> edges = new List<List<int>>();

    for (int i = 0; i < m; i++)
    {
        edges.Add(Console.ReadLine().TrimEnd().Split(' ').ToList().Select(edgesTemp => Convert.ToInt32(edgesTemp)).ToList());
    }

    int s = Convert.ToInt32(Console.ReadLine().Trim());

    List<int> result = Result.shortestReach(n, edges, s);

    textWriter.WriteLine(String.Join(" ", result));
}

textWriter.Flush();
textWriter.Close();


//int t = 1;
//for (int tItr = 0; tItr < t; tItr++)
//{
//    string[] firstMultipleInput = "4 4".TrimEnd().Split(' ');

//    int n = Convert.ToInt32(firstMultipleInput[0]);

//    int m = Convert.ToInt32(firstMultipleInput[1]);

//    List<List<int>> edges = new List<List<int>>();


//    edges.Add("1 2 24".TrimEnd().Split(' ').ToList().Select(edgesTemp => Convert.ToInt32(edgesTemp)).ToList());
//    edges.Add("1 4 20".TrimEnd().Split(' ').ToList().Select(edgesTemp => Convert.ToInt32(edgesTemp)).ToList());
//    edges.Add("3 1 3".TrimEnd().Split(' ').ToList().Select(edgesTemp => Convert.ToInt32(edgesTemp)).ToList());
//    edges.Add("4 3 12".TrimEnd().Split(' ').ToList().Select(edgesTemp => Convert.ToInt32(edgesTemp)).ToList());


//    int s = Convert.ToInt32("1".Trim());

//List<int> result = Result.shortestReach(9, null,1);

//    Console.WriteLine(String.Join(" ", result));
//}
/*
 1
4 4
1 2 24
1 4 20
3 1 3
4 3 12
1
*/
/*
 1
5 3
1 2 10
1 3 6
2 4 8
2
*/
class Result
{

    /*
     * Complete the 'shortestReach' function below.
     *
     * The function is expected to return an INTEGER_ARRAY.
     * The function accepts following parameters:
     *  1. INTEGER n
     *  2. 2D_INTEGER_ARRAY edges
     *  3. INTEGER s
     */

    static string sptPath = "";
    static int infinite = 999999;
    static int[] distances;
    static int[] parents;
    static int[,] adjacencyMatrix;
    static int N;

    static void InitAll()
    {
        sptPath = "";
        infinite = 999999;
        distances = null;
        parents = null;
        adjacencyMatrix = null;
        N = 0;
    }
    static void AddToSpt(int node)
    {
        sptPath += $"-{node}-";
    }

    static void UpdateAdjacentOfNode(int node)
    {
        for (int i = 0; i < N; i++)
        {
            int weigth = adjacencyMatrix[node, i];
            int newDistance = distances[node] + weigth;
            if (weigth != -1 && newDistance <= distances[i])
            {
                distances[i] = newDistance;
                parents[i] = node;
            }
        }
    }

    static int GetNodeWithMinDistanceAndNotinSPT()
    {
        int node = -1;
        int minDistance = infinite;
        for (int i = 0; i < N; i++)
        {
            int dist = distances[i];
            bool includedInSPT = sptPath.Contains($"-{i}-");
            if (dist != infinite && includedInSPT == false && dist <= minDistance)
            {
                node = i;
                minDistance = dist;
            }
        }
        return node;
    }
    static void UpdateUnreachableNodesDistance()
    {
        for (int i = 0; i < N; i++)
        {
            if (distances[i] == infinite)
            {
                distances[i] = -1;
            }
        }
    }
    public static List<int> shortestReach(int n, List<List<int>> edges, int s)
    {
        InitAll();
        N = n;
        if (n < 2 || n > 3000)
        {
            throw new ArgumentOutOfRangeException();
        }

        int pickedNode = s - 1;
        distances = new int[n];
        parents = new int[n];
        adjacencyMatrix = new int[n, n];
        //#region Temp
        //adjacencyMatrix = new int[,] { { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
        //                              { 4, 0, 8, 0, 0, 0, 0, 11, 0 },
        //                              { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
        //                              { 0, 0, 7, 0, 9, 14, 0, 0, 0 },
        //                              { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
        //                              { 0, 0, 4, 14, 10, 0, 2, 0, 0 },
        //                              { 0, 0, 0, 0, 0, 2, 0, 1, 6 },
        //                              { 8, 11, 0, 0, 0, 0, 1, 0, 7 },
        //                              { 0, 0, 2, 0, 0, 0, 6, 7, 0 } };
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        if(adjacencyMatrix[i, j] == 0)
        //        {
        //            adjacencyMatrix[i, j] = -1;
        //        }

        //    }
        //}
        //#endregion

        //Update adjacencey matrix/distance with intial values
        for (int i = 0; i < n; i++)
        {
            if (i == pickedNode)
            {
                distances[i] = 0;
            }
            else
            {
                distances[i] = infinite;
            }
            parents[i] = -1;

            for (int j = 0; j < n; j++)
            {
                adjacencyMatrix[i, j] = -1;
            }
        }
        //Update adjacencey matrix with actual values
        foreach (var edge in edges)
        {
            int startNode = edge[0] - 1;
            int endNode = edge[1] - 1;
            int weight = edge[2];
            int currentWeight = adjacencyMatrix[startNode, endNode];
            //This condition for multiple paths
            //Sometime we have multiple paths between same nodes with different weight and we will always pick the
            //smalles one
            if (weight < currentWeight || currentWeight == -1)
            {
                adjacencyMatrix[startNode, endNode] = weight;
                adjacencyMatrix[endNode, startNode] = weight;
            }
        }

        do
        {
            AddToSpt(pickedNode);
            //Update all of it's adjacnets distances
            UpdateAdjacentOfNode(pickedNode);
            pickedNode = GetNodeWithMinDistanceAndNotinSPT();
        }
        while (pickedNode != -1);
        UpdateUnreachableNodesDistance();
        return distances.Where((d, index) => index != s - 1).ToList();
    }

}









