using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public static class MatrixUtilities
    {
        /// <summary>
        ///     Create a local to world matrix when you don't have a Transform to do it for you.
        /// </summary>
        public static Matrix4x4 CreateLocalToWorldMatrix( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Matrix4x4 translationMatrix = Matrix4x4.Translate( position );
            Matrix4x4 rotationMatrix = Matrix4x4.Rotate( rotation );
            Matrix4x4 scaleMatrix = Matrix4x4.Scale( scale );

            // Combine translation, rotation, and scale matrices to form the local-to-world matrix
            Matrix4x4 localToWorldMatrix = translationMatrix * rotationMatrix * scaleMatrix;

            return localToWorldMatrix;
        }

        /// <summary>
        ///     Call on a parent Transform to get the child's local to world relative matrix.
        ///     Essentially, this means that the child transform will be aligned to the parent transform.
        /// </summary>
        public static Matrix4x4 LocalToWorldRelativeMatrix( Matrix4x4 parentLocalToWorldMtx, Transform transformToBeMatched )
        {
            // This line constructs a transformation matrix (localToParentMtx) that represents the local transformation of the mesh relative to its parent.
            Matrix4x4 localToParentMtx = Matrix4x4.TRS( transformToBeMatched.localPosition, transformToBeMatched.localRotation, transformToBeMatched.localScale );

            // This line combines the transformations of the parent and the mesh to create a single transformation
            // matrix (localToWorldRelativeMtx) that transforms from the local space of the mesh to the world space relative to the parent transform.
            return parentLocalToWorldMtx * localToParentMtx;
        }
    }
}