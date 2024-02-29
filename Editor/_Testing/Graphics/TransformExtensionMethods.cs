using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.Graphics
{
    public static class TransformExtensionMethods
    {
        /// <summary>
        /// Call on a parent Transform to get the child's local to world relative matrix.
        /// Essentially, this means that the child transform will be aligned to the parent transform.
        /// </summary>
        public static Matrix4x4 LocalToWorldRelativeMatrix( this Transform transformToMatch, Transform transformToBeMatched )
        {
            // localToWorldMatrix is a property of the Transform class in Unity, representing the transformation matrix that converts local coordinates to world coordinates
            Matrix4x4 parentLocalToWorldMtx = transformToMatch.localToWorldMatrix;
                
            // This line constructs a transformation matrix (localToParentMtx) that represents the local transformation of the mesh relative to its parent.
            Matrix4x4 localToParentMtx = Matrix4x4.TRS( transformToBeMatched.localPosition, transformToBeMatched.localRotation, transformToBeMatched.localScale );
                
            // This line combines the transformations of the parent and the mesh to create a single transformation
            // matrix (localToWorldRelativeMtx) that transforms from the local space of the mesh to the world space relative to the parent transform.
            return ( parentLocalToWorldMtx * localToParentMtx );
        }
    }
}