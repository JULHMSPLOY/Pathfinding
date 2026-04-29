using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PathfindingTest : MonoBehaviour 
{
    public Pathfinding pf;
    public Waypoint startNode;
    public Waypoint goalNode;
    
    void Update() {
    if (Keyboard.current.spaceKey.wasPressedThisFrame) { 
        List<Waypoint> resultPath = pf.FindPath(startNode, goalNode);

        if (resultPath != null && resultPath.Count > 0) {
            Debug.Log("--- Start of Path ---"); // หัวข้อเริ่มต้น
            
            // วนลูปแสดงผลทีละบรรทัด
            foreach (Waypoint wp in resultPath) {
                Debug.Log("Next Step: " + wp.name); 
            }
            
            Debug.Log("--- End of Path ---"); // สิ้นสุดเส้นทาง
        } else {
            Debug.Log("No path found.");
        }
    }
}}