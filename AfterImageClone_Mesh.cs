using UnityEngine;

namespace AfterImage.Clone
{
    /// <summary>
    /// AfterImageClone_Mesh represent a copy of a mesh inside the engine 3D space
    /// </summary>
    public struct AfterImageClone_Mesh
    {
        public Mesh Mesh;
        public Matrix4x4 CloneMatrix;
        public Material SharedMaterial;

        public AfterImageClone_Mesh(Vector3 clonePos, Quaternion cloneRot, Vector3 cloneScale, Mesh mesh, Material material)
        {
            CloneMatrix = Matrix4x4.TRS(clonePos, cloneRot, cloneScale);
            this.Mesh = mesh;
            SharedMaterial = material;
        }

        public void DrawCloneComponent() => Graphics.DrawMesh(Mesh, CloneMatrix, SharedMaterial, 1);
    }
}
