using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;

/* This class is responsible for generating a list of nodes which represent a grid.
 * It stores a list of those nodes, providing it on request.
 */
namespace Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Runtime
{
    // [Serializable]
    public class PlacementGrid
    {
        private GridNode[,] _nodeList;
        
        private Vector2 _gridSize;
        private float _nodeRadius;
        private float[,] _noiseMap;

        private Transform _transform;
        private int _gridSizeX;
        private int _gridSizeY;
        private float _nodeDiameter;

        private FormattedLogger _logger;

        // [SerializeField] private List<ListWrapper> nodeList;

        public PlacementGrid( Transform transform, float nodeRadius, Vector2 gridSize, float[,] noiseMap, FormattedLogger logger )
        {
            _logger = logger;
            
            _logger.LogStart();
            _transform = transform;
            _nodeDiameter = nodeRadius * 2;
            _nodeRadius = nodeRadius;
            _gridSize = gridSize;
            _gridSizeX = Mathf.RoundToInt( _gridSize.x / _nodeDiameter );
            _gridSizeY = Mathf.RoundToInt( _gridSize.y / _nodeDiameter );
            _noiseMap = noiseMap;
            
            CreateGrid();
            _logger.LogEnd();
        }
        
        // public void OnAwake( Transform transform, float nodeRadius, Vector2 gridSize, float[,] noiseMap, FormattedLogger logger )
        // {
        //     _logger = logger;
        //     
        //     _logger.LogStart();
        //     _logger.Log( "Initializing values." );
        //     _transform = transform;
        //     _nodeDiameter = nodeRadius * 2;
        //     _nodeRadius = nodeRadius;
        //     _gridSize = gridSize;
        //     _gridSizeX = Mathf.RoundToInt( _gridSize.x / _nodeDiameter );
        //     _gridSizeY = Mathf.RoundToInt( _gridSize.y / _nodeDiameter );
        //     _noiseMap = noiseMap;
        //
        //     if ( nodeList == null || nodeList.Count == 0 )
        //     {
        //         _logger.Log( "NodeList is null. Calling CreateGrid() to build NodeList." );
        //         CreateGrid();
        //     }
        //     else
        //     {
        //         _logger.Log( $"NodeList is NOT null and contains {nodeList.Count.ToString()} entries." );
        //     }
        //
        //     _logger.LogEnd();
        // }

        // public void UpdateGrid( bool reset = false )
        // {
        //     _logger.LogStart();
        //
        //     CreateGrid( reset );
        //     
        //     _logger.LogEnd();
        // }

        private void CreateGrid( bool reset = false )
        {
            _logger.LogStart();

            _logger.Log( "Updating PlacementGrid..." );

            if ( _nodeList == null )
            {
                _logger.Log( "PlacementGrid was null. Resetting PlacementGrid..." );
                _nodeList = new GridNode[_gridSizeX, _gridSizeY];
                // nodeList = new List<ListWrapper>();
            }
            
            if ( reset )
            {
                _logger.Log( "PlacementGrid was Not null but Reset was selected. Resetting PlacementGrid..." );
                _nodeList = new GridNode[_gridSizeX, _gridSizeY];
                // nodeList = new List<ListWrapper>();
            }
            
            Vector3 worldBottomLeft = _transform.position - ( Vector3.right * ( _gridSize.x / 2 ) ) - ( Vector3.forward * ( _gridSize.y / 2 ) );

            for (int x = 0; x < _gridSizeX; x++)
            {
                // nodeList.Add( new ListWrapper() );
                
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + ( Vector3.right * ( x * _nodeDiameter + _nodeRadius ) ) + ( Vector3.forward * ( y * _nodeDiameter + _nodeRadius ) );

                    var node = new GridNode( new Vector3( worldPoint.x, 0, worldPoint.z ), _noiseMap[x, y], new Vector2Int( x, y ) );
                    
                    // node.DistanceFromWorldCenter = node.WorldPosition.magnitude;
                    _nodeList[x, y] = node;
                    // nodeList[x].nodeList.Add( node );
                }
            }
            
            _logger.LogEnd();
        }

        public GridNode[,] GetNodeList() => _nodeList;
        
        // public GridNode[,] GetNodeList()
        // {
        //     GridNode[,] nodeList = new GridNode[_gridSizeX, _gridSizeY];
        //
        //     for (int x = 0; x < this.nodeList.Count; x++)
        //     {
        //         ListWrapper currentListWrapper = this.nodeList[x];
        //         for (int y = 0; y < _gridSizeY; y++)
        //         {
        //             nodeList[x, y] = currentListWrapper.nodeList[y];
        //         }
        //     }
        //     
        //     return nodeList;
        // }
    }
}