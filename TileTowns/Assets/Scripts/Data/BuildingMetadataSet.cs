﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Set", fileName = nameof(BuildingMetadataSet))]
    public class BuildingMetadataSet : ScriptableObject, IEnumerable<Building>
    {
        [field: SerializeField] public Building[] Buildings { get; set; }
        public IEnumerator<Building> GetEnumerator()
        {
            if (Buildings == null) yield break;

            for (int i = 0; i < Buildings.Length; i++)
            {
                yield return Buildings[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

