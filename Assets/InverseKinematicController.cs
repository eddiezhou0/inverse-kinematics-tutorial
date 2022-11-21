using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InverseKinematicController : MonoBehaviour
{

    [SerializeField] private GameObject segment1;
    [SerializeField] private GameObject segment2;
    [SerializeField] private GameObject hand;

    private float _segment1Length;
    private float _segment2Length;

    // Start is called before the first frame update
    void Start()
    {
        _segment1Length = Vector3.Distance(segment1.transform.position, segment2.transform.position);
        _segment2Length = Vector3.Distance(segment2.transform.position, hand.transform.position);
    }
    

    private void OnMouseDrag()
    {
        // Mouse position
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        
        // Calculate relative position and distance from base to cursor
        var relativePosition = position - segment1.transform.position;
        var distance = relativePosition.magnitude;

        // Return if too far
        if (distance > _segment1Length + _segment2Length)
        {
            return;
        }
        
        // Set hand to mouse
        hand.transform.position = position;
        
        // Base rotation for direct line to point
        var angle = Mathf.Rad2Deg * Mathf.Atan(relativePosition.y/relativePosition.x);
        
        // Law of cosines
        var squares = Mathf.Pow(_segment1Length, 2) + Mathf.Pow(_segment2Length, 2) - Mathf.Pow(distance, 2);
        var elbowAngle = Mathf.Acos(squares/(2 * _segment1Length * _segment2Length)) ;
        
        // Law of sines
        var segment1Angle = Mathf.Asin(Mathf.Sin(elbowAngle) *_segment2Length/distance) * Mathf.Rad2Deg;
        
        // Calculate last angle
        var segment2Angle = elbowAngle * Mathf.Rad2Deg - 180.0f;

        // Set rotations
        segment1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle + segment1Angle);
        segment2.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, segment2Angle);
    }
}
