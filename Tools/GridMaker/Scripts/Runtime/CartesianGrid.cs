using System;

/* This class is responsible for generating a list of nodes which represent a grid.
 * It stores a list of those nodes, providing it on request.
 */
namespace Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Runtime
{
    [Serializable]
    public class CartesianGrid
    {
        // [SerializeField] private GridNode[] nodeArray;
        // [SerializeField] private int _totalNodes;
        //
        // [SerializeField] private CustomLogger logger;
        //
        // public void Initialize( CustomLogger newLogger, int totalNodes, int gridSizeX, int gridSizeY,  )
        // {
        //     logger = newLogger;
        //     
        //     logger.LogStart();
        //
        //     _totalNodes = totalNodes;
        //
        //     logger.LogEnd();
        // }
        //
        // public void UpdateNodeArray()
        // {
        //     nodeArray = new GridNode[_totalNodes];
        //     PopulateNodeArray();
        // }
        //
        // private void PopulateNodeArray()
        // {
        //     int gridSizeX = _gameManager.GetGridSizeX();
        //     int gridSizeY = _gameManager.GetGridSizeY();
        //     
        //     if ( nodeArray.Length != ( gridSizeX * gridSizeY ) ) nodeArray = new GridNode[gridSizeX * gridSizeY];
        //     
        //     float nodeWidthX = _gameManager.GetGridCellWidth();
        //     float nodeWidthY = _gameManager.GetGridCellHeight();
        //
        //     
        //     var worldTopLeft = new Vector3( ( -nodeWidthX * gridSizeX ) / 2, 0f, ( -nodeWidthY * gridSizeY ) / 2 );
        //     var shiftToCenter = new Vector3( ( nodeWidthX / 2), 0f, ( nodeWidthY / 2 ) );
        //     worldTopLeft += _gameManager.GetCenterPosition();
        //     worldTopLeft += shiftToCenter;
        //
        //     for (int x = 0; x < gridSizeX; x++)
        //     {
        //         for (int y = 0; y < gridSizeY; y++)
        //         {
        //             var nodePos = new Vector3( ( nodeWidthX * x ), 0f, ( nodeWidthY * y ) );
        //             // var gridNode = new GridNode( worldTopLeft + nodePos, noiseMap[x,y], new Vector2Int( x, y ) );
        //             var gridNode = new GridNode( worldTopLeft + nodePos, 0, new Vector2Int( x, y ) );
        //             
        //             SetNodeAtGridPosition( x, y, gridNode );
        //         }
        //     }
        // }
        //
        // private int GetArrayPositionForGridPosition( int gridX, int gridY ) => ( gridX * _gameManager.GetGridSizeX() ) + gridY;
        //
        // public int GetNodesIndexNumberInArray( GridNode gridNode ) => nodeArray.ToList().IndexOf( gridNode );
        //
        // private void SetNodeAtGridPosition( int gridX, int gridY, GridNode gridNode ) => 
        //     nodeArray[GetArrayPositionForGridPosition( gridX, gridY )] = gridNode;
        //
        // private void SetNodeValueAtGridPosition( int gridX, int gridY, float value ) => 
        //     nodeArray[GetArrayPositionForGridPosition( gridX, gridY )].SetNoiseValue( value );
        //
        // private float GetNodeValueAtGridPosition( int gridX, int gridY ) => 
        //     nodeArray[GetArrayPositionForGridPosition( gridX, gridY )].GetNoiseValue();
        //
        // public GridNode GetNodeFromGridPosition( int gridX, int gridY ) => 
        //     nodeArray[GetArrayPositionForGridPosition( gridX, gridY )];
        //
        // public GridNode[] GetNodeArray() => nodeArray;
        //
        //
        // /* Get All Nodes in Range
        //  * Returns a list of all nodes in a square area around the origin within the min and max range
        //  */
        // public List<GridNode> GetAllNodesInSquareRange( Vector2Int gridLocation, int minRange, int maxRange )
        // {
        //     logger.LogStart( false, $"Checking [{GetColoredStringYellow( gridLocation.x.ToString() )}, {GetColoredStringYellow( gridLocation.y.ToString() )}], range: [{minRange.ToString()}, {maxRange.ToString()}]" );
        //     // GridNode[,] nodeList = _placementGrid.GetNodeList();
        //     // Vector2 gridLocation = GetGridPosFromWorldPos( origin );
        //
        //     List<GridNode> nodesInRange = new List<GridNode>();
        //     int totalRange = maxRange * 2 + 1; // 2 for neighbors on each side, plus 1 for middle neighbor
        //     int gridSizeX = _gameManager.GetGridSizeX();
        //     int gridSizeY = _gameManager.GetGridSizeY();
        //     
        //     int originNodeX = gridLocation.x;
        //     int originNodeY = gridLocation.y;
        //     
        //     int leftOuterBoundX = -1 * ( ( totalRange - 1 ) / 2 ) + originNodeX;
        //     int rightOuterBoundX = ( totalRange - 1 ) / 2 + originNodeX;
        //     int leftInnerBoundX = -minRange + originNodeX;
        //     int rightInnerBoundX = minRange + originNodeX;
        //
        //     int leftOuterBoundY = -1 * ( ( totalRange - 1 ) / 2 ) + originNodeY;
        //     int rightOuterBoundY = ( totalRange - 1 ) / 2 + originNodeY;
        //     int leftInnerBoundY = -minRange + originNodeY;
        //     int rightInnerBoundY = minRange + originNodeY;
        //     
        //     logger.LogIndentStart( $"Boundaries set to: " +
        //                            $"x |--> [{leftOuterBoundX.ToString()}, {rightOuterBoundX.ToString()}] <---> [{leftInnerBoundX.ToString()}, {rightInnerBoundX.ToString()}] <--| " +
        //                            $"y |--> [{leftOuterBoundY.ToString()}, {rightOuterBoundY.ToString()}] <---> [{leftInnerBoundY.ToString()}, {rightInnerBoundY.ToString()}] <--| ", true );
        //
        //     for (int x = leftOuterBoundX; x <= rightOuterBoundX; x++)
        //     {
        //         for (int y = leftOuterBoundY; y <= rightOuterBoundY; y++)
        //         {
        //             // Filter based on min/max range
        //             if (x < leftInnerBoundX || y < leftInnerBoundY || x > rightInnerBoundX || y > rightInnerBoundY)
        //             {
        //                 // if (x >= 0 && x < nodeList.GetLength( 0 ) && y >= 0 && y < nodeList.GetLength( 1 ) &&
        //                 //     !( x == originNodeX && y == originNodeY ))
        //                 // {
        //                 //     nodesInRange.Add( nodeList[x, y] );
        //                 // }
        //                 
        //                 if ( x >= 0 && x < gridSizeX && 
        //                      y >= 0 && y < gridSizeY && 
        //                      !( x == originNodeX && y == originNodeY ) )
        //                 {
        //                     // nodesInRange.Add( nodeList[x, y] );
        //                     nodesInRange.Add( GetNodeFromGridPosition( x, y ) );
        //                 }
        //             }
        //         }
        //     }
        //
        //     logger.DecrementMethodIndent();
        //     logger.LogEnd( $"Returning {GetColoredStringYellow( nodesInRange.Count.ToString() )} nodes." );
        //     return nodesInRange;
        // }
        //
        // /// <summary>
        // /// Runs one for every node. Returns a list of node within the set range.
        // /// </summary>
        // public List<GridNode> GetAllNodesInCircularRange( Vector3 origin, int sqrMinRange, int sqrMaxRange )
        // {
        //     List<GridNode> neighbors = new List<GridNode>();
        //     foreach ( GridNode gridNode in nodeArray )
        //     {
        //         float distFromOrigin = ( gridNode.GetGridWorldPosition() - origin ).sqrMagnitude;
        //         
        //         if ( distFromOrigin >= sqrMinRange && distFromOrigin <= sqrMaxRange )
        //             neighbors.Add( gridNode );
        //     }
        //
        //     return neighbors;
        // }
        //
        //
        // public GridNode GetCenterNodeBetweenNodes( List<GridNode> nodeList )
        // {
        //     Vector3 centerPoint = Vector3.zero;
        //     foreach (GridNode currentNode in nodeList)
        //     {
        //         centerPoint += currentNode.GetGridWorldPosition();
        //     }
        //
        //     centerPoint = new Vector3( ( centerPoint.x / nodeList.Count ), ( centerPoint.y / nodeList.Count ), ( centerPoint.z / nodeList.Count ) );
        //     Vector2 gridPosition = GetGridPosFromWorldPos( centerPoint );
        //     int gridPositionX = Mathf.RoundToInt( gridPosition.x );
        //     int gridPositionY = Mathf.RoundToInt( gridPosition.y );
        //     GridNode centerGridNode = null;
        //     if (gridPositionX <= _gameManager.GetGridSizeX() && gridPositionY <= _gameManager.GetGridSizeY() )
        //     {
        //         centerGridNode = GetNodeFromGridPosition( gridPositionX, gridPositionY );
        //     }
        //
        //     return centerGridNode;
        // }
        //
        // /* Get Grid Position from World Position
        //  * Returns a Vector2, the x,y coordinates on the grid position, on success
        //  * Returns null if position is outside the range of the current map size (Not yet implemented)
        //  */
        // private Vector2 GetGridPosFromWorldPos( Vector3 worldPosition )
        // {
        //     float worldSizeX = _gameManager.GetWorldSizeX();
        //     float worldSizeY = _gameManager.GetWorldSizeY();
        //     float percentX = ( worldPosition.x + worldSizeX / 2 ) / worldSizeX;
        //     float percentY = ( worldPosition.z + worldSizeY / 2 ) / worldSizeY;
        //     percentX = Mathf.Clamp01( percentX );
        //     percentY = Mathf.Clamp01( percentY );
        //     int x = Mathf.RoundToInt( ( _gameManager.GetGridSizeX() - 1 ) * percentX );
        //     int y = Mathf.RoundToInt( ( _gameManager.GetGridSizeY() - 1 ) * percentY );
        //
        //     return new Vector2( x, y );
        // }
    }
}