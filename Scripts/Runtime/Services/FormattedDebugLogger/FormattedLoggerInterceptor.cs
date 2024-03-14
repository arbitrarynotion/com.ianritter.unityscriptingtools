using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    /* This was an idea I was pursuing with the help of ChatGPT to try and automate the log start and end calls
       using an attribute. The primary reasons for this was both to reduce lines of code and to ensure that all 
       and to ensure that all log blocks were closed automatically; when doing them manually, forgetting the
       LogEnd() call will result in an problems.
       
       I wasn't able to get this to work yet, though. So I'll come back to it later.
     */
    [ExecuteInEditMode]
    public class FormattedLoggerInterceptor : MonoBehaviour
    {
        // [SerializeField] private FormattedLogger logger;
        //
        // private void OnEnable()
        // {
        //     logger.LogStart( true );
        //     
        //     
        //     // Get all MonoBehaviour instances in the scene
        //     ILoggable[] loggables = FindObjectsOfType<MonoBehaviour>(true).OfType<ILoggable>().ToArray();
        //     logger.LogIndentStart( $"Found {GetColoredStringGreenYellow( loggables.Length.ToString() )} ILoggables in the scene:" );
        //
        //     foreach ( ILoggable loggable in loggables )
        //     {
        //         logger.LogIndentStart( $"• {GetColoredStringYellow( loggable.GetName() )} ({GetColoredStringAqua( loggable.GetType().Name )}) which contains the methods:" );
        //         
        //         // Get all methods of the MonoBehaviour instance.
        //         MethodInfo[] methods = loggable.GetType().GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
        //         foreach ( MethodInfo methodInfo in methods )
        //         {
        //             // Check if the method has the FormattedLogger attribute
        //             if( !Attribute.IsDefined( methodInfo, typeof( FormattedAutoLoggerAttribute ) ) ) continue;
        //             
        //             logger.Log( $"• {GetColoredStringGoldenrod( methodInfo.Name )}" );
        //
        //             ReplaceMethod( loggable, methodInfo );
        //         }
        //
        //         logger.DecrementMethodIndent();
        //     }
        //
        //     logger.LogEnd();
        // }
        //
        // private void ReplaceMethod( ILoggable loggable, MethodInfo methodInfo )
        // {
        //     // var methodDelegate = (Action) Delegate.CreateDelegate( typeof( Action ), loggable, methodInfo );
        //     
        //     Delegate methodDelegate;
        //     methodDelegate = Delegate.CreateDelegate( methodInfo.ReturnType == typeof(void) 
        //         ? typeof(Action) 
        //         : typeof(Func<>).MakeGenericType(methodInfo.ReturnType), loggable, methodInfo );
        //
        //     // Wrap the original method in start and end calls.
        //     
        //     Delegate interceptedMethod = BuildInterceptedMethod(methodInfo, methodDelegate, loggable);
        //     // Action interceptMethod = () =>
        //     // {
        //     //     logger.LogStart();
        //     //     methodDelegate();
        //     //     logger.LogEnd();
        //     // };
        //
        //     MethodInfo originalMethod = methodInfo;
        //     Type originalDeclaringType = originalMethod.DeclaringType;
        //     
        //     var newMethod = new DynamicMethod( originalMethod.Name, typeof( void ), new Type[] { originalDeclaringType }, originalDeclaringType );
        //     
        //     ILGenerator il = newMethod.GetILGenerator();
        //     // Load "this" onto the evaluation stack.
        //     il.Emit( OpCodes.Ldarg_0 );
        //     // Call the intercept method.
        //     il.Emit( OpCodes.Call, interceptedMethod.Method );
        //     // Return from the method
        //     il.Emit( OpCodes.Ret );
        //     // var newMethodDelegate = (Action) newMethod.CreateDelegate( typeof( Action ), null );
        //     originalMethod.DeclaringType.GetMethod( originalMethod.Name ).Invoke( loggable, null );
        // }
        //
        // private Delegate BuildInterceptedMethod( MethodInfo originalMethod, Delegate methodDelegate, object instance )
        // {
        //     if( originalMethod.ReturnType == typeof( void ) )
        //     {
        //         return (Action<object>) ( param =>
        //         {
        //             logger.LogStart( originalMethod.Name );
        //             methodDelegate.DynamicInvoke( param );
        //             logger.LogEnd( originalMethod.Name );
        //         } );
        //     }
        //
        //     return Delegate.CreateDelegate( typeof( Func<,> ).MakeGenericType( typeof( object ), originalMethod.ReturnType ), instance, originalMethod );
        // }
    }
}
