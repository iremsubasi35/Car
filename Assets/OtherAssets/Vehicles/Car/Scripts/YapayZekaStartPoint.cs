using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class YapayZekaStartPoint : MonoBehaviour
    {
        public List<Transform> StartPointsForAI;
        public List<YapayZekaController> YapayAraclar;
        public int TotalYapayRacer = 3;

        private void Start()
        {
            var pickRandom = YapayAraclar.OrderBy(x => Guid.NewGuid()).ToArray();

            var counter = TotalYapayRacer;
            while (counter > 0)
            {
                int.TryParse(StartPointsForAI[counter - 1].name, out var spawnPoint);
                var arac = pickRandom[counter];
                arac.SpawnPointIndex = spawnPoint;
                arac.transform.position = StartPointsForAI[counter - 1].position;
                arac.gameObject.SetActive(true);
                counter--;
            }
        }
    }
}