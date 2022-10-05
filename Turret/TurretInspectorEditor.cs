using System.Collections;
using UnityEngine;
using UnityEditor;
using Assets.TowerDefencePortfolio;
using lLCroweTool;

namespace Assets.TowerDefencePortfolio
{
    [CustomEditor(typeof(Turret))]
    public class TurretInspectorEditor : Editor
    {
        private Turret turret;
        private void OnEnable()
        {
            turret = (Turret)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (turret.TryGetComponent(out SightTrigger sightTrigger))
            {
                sightTrigger.range = turret.distance;
                sightTrigger.tag = turret.tag;
            }
        }
    }
}