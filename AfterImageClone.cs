using UnityEngine;
using System.Collections.Generic;

namespace AfterImage.Clone
{
    /// <summary>
    /// AfterImageClone is a collection of AfterImageClone_Mesh it represents a capture of all meshes of a model
    /// </summary>
    public struct AfterImageClone
    {
        public float SpawnTime;
        List<AfterImageClone_Mesh> CloneImageComponents;

        public AfterImageClone(List<AfterImageClone_Mesh> imageClones)
        {
            CloneImageComponents = imageClones;
            SpawnTime = Time.time;
        }

        public void DrawClone()
        {
            foreach (AfterImageClone_Mesh component in CloneImageComponents)
                component.DrawCloneComponent();
        }
    }
}