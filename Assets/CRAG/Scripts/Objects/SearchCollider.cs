﻿using System.Collections.Generic;
using UnityEngine;

namespace CRAG
{
    class SearchCollider : MonoBehaviour
    {
        public GameActor actor;
        public string detectedTag = "detected";

        private List<Collider> _detected = new List<Collider>();

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Contains(detectedTag))
                _detected.Add(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Contains(detectedTag))
                _detected.Remove(other);
        }

        public Collider GetDetectedCollider()
        {
            return _detected[0];
        }
    }
}