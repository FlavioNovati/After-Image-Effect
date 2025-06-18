using UnityEngine;
using System.Collections.Generic;

namespace AfterImage
{
    using Clone;
    /// <summary>
    /// AfterImageVFX is used to draw AfterImageClones 
    /// </summary>
    public class AfterImageVFX : MonoBehaviour
    {
        public delegate void AfterImageVFX_Callback();
        public event AfterImageVFX_Callback OnVFXStarted;
        public event AfterImageVFX_Callback OnVFXEnded;

        [SerializeField, Tooltip("How long a clone should be drawn")] private float CloneLifeTime = 3f;
        [SerializeField, Tooltip("Time interval between clones")] private float CloneInterval = 0.5f;
        [SerializeField, Tooltip("Material applied to clones")] private Material CloneMaterial;

        //List of all Mesh Renderers
        private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
        private List<MeshFilter> _meshFilters;
        
        private Queue<AfterImageClone> _spawnedClones;

        //Last time a clone is spawned
        private float _lastCloneSpawnTime = 0f;

        private void Awake()
        {
            _spawnedClones = new Queue<AfterImageClone>();

            //Get all skinned mesh renderer of the current game object
            _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
            _skinnedMeshRenderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());

            //Get all mesh renderer of the current game object
            _meshFilters = new List<MeshFilter>();
            _meshFilters.AddRange(GetComponentsInChildren<MeshFilter>());

            //Disable Component
            this.enabled = false;
        }

        /// <summary>
        /// Draw all Clones
        /// </summary>
        private void Update()
        {
            foreach(AfterImageClone clone in _spawnedClones)
                clone.DrawClone();
        }

        /// <summary>
        /// Update VFX Behaviour
        /// </summary>
        private void FixedUpdate()
        {
            //Check if a new clone should be added
            if (_lastCloneSpawnTime + CloneInterval < Time.time)
            {
                Debug.Log(_lastCloneSpawnTime - Time.time);
                _lastCloneSpawnTime = Time.time;
                AddClone();
            }

            //Check if last clone in the list should be removed
            if (_spawnedClones.TryPeek(out AfterImageClone afterImageClone) && afterImageClone.SpawnTime + CloneLifeTime < Time.time)
                _spawnedClones.Dequeue();
        }

        private void AddClone()
        {
            List<AfterImageClone_Mesh> cloneComponents;
            cloneComponents = new List<AfterImageClone_Mesh>();

            //Get Meshes
            Mesh mesh = new Mesh();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
            {
                //Get Mesh position and rotation
                Vector3 clonePos = skinnedMeshRenderer.transform.position;
                Quaternion cloneRot = skinnedMeshRenderer.transform.rotation;
                Vector3 cloneScale = skinnedMeshRenderer.transform.lossyScale;
            
                //Get Mesh
                Mesh bakedMesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(bakedMesh);

                //Add mesh snapshot
                cloneComponents.Add( new AfterImageClone_Mesh(clonePos, cloneRot, cloneScale, bakedMesh, CloneMaterial) );
            }
            foreach (MeshFilter meshFilter in _meshFilters)
            {
                //Get Mesh position and rotation
                Vector3 clonePos = meshFilter.transform.position;
                Quaternion cloneRot = meshFilter.transform.rotation;
                Vector3 cloneScale = meshFilter.transform.lossyScale;

                //Add mesh snapshot
                cloneComponents.Add(new AfterImageClone_Mesh(clonePos, cloneRot, cloneScale, meshFilter.sharedMesh, CloneMaterial));
            }

            //Create Clone
            AfterImageClone clone = new AfterImageClone(cloneComponents);
            //Add clone to draw queue
            _spawnedClones.Enqueue(clone);
        }

        public void StartVFX()
        {
            this.enabled = true;

            OnVFXStarted?.Invoke();
        }

        public void StopVFX()
        {
            this.enabled = false;

            OnVFXEnded?.Invoke();
        }

        public bool IsEmitting => this.enabled;
    }
}