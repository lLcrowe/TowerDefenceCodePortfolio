using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor.U2D;

public class PathPlacer : MonoBehaviour {

    public float spacing = .1f;
    public float resolution = 1;
    public LineRenderer line;
	
	void Start () {
        Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        line.positionCount = points.Length;
        line.SetPositions(points);
	}
}
