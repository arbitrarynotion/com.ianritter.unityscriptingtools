namespace Packages.com.ianritter.unityscriptingtools.Runtime.Enums
{
    public enum ElementFrameType
    {
        None,
        
        FullOutline,
        
        // Incomplete Rects
        SkipTop,
        SkipBottom,
        LeftAndBottomOnly,
        
        // Single Edge Rects
        BottomOnly,
        LeftOnly,
        
        // Corner Edge Rects
        Corners,
        CornersSkipTopLines,
        CornersBottomOnly,
        CornersLeftBottomOnly
    }
}