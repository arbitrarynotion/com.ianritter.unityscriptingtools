using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.EditorBridge
{
    public class EditorBridgeRuntimeSide
    {
        // Concept:
        // Create events in Runtime assembly that are subscribed to by 
        // a script in the editor assembly. Then a script could call this end
        // to trigger something on that end, that would then request the info
        // from this end - resulting in a script on the runtime side having some
        // limited use of scripts on the editor side.
        
        // Catch:
        // The main way that I know for doing this is using a Custom Editor. This
        // is a viable option, however a window would need to be open
        // that used that editor script in order for the script to be accessible.
        // I'll have to think on this.
    }
}
