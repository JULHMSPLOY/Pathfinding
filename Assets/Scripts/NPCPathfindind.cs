using UnityEngine;
using System.Collections.Generic;
using Priority_Queue; // อย่าลืมใส่ Namespace นี้ และต้องมีไฟล์ SimplePriorityQueue ในโปรเจกต์

public class NPCPathfinding : MonoBehaviour 
{
    public Waypoint[] waypoints; 

    // ข้อ 4a) หา Waypoint ที่ใกล้ตำแหน่งที่ระบุมากที่สุด
    public Waypoint GetClosestWaypoint(Vector3 position) 
    {
        Waypoint closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Waypoint wp in waypoints) 
        {
            float dist = Vector3.Distance(position, wp.transform.position);
            if (dist < minDistance) 
            {
                minDistance = dist;
                closest = wp;
            }
        }
        return closest;
    }

    // ฟังก์ชัน A* สำหรับหาเส้นทางที่สั้นที่สุด
    public List<Waypoint> FindPath(Waypoint start, Waypoint goal) 
    {
        List<Waypoint> closedSet = new List<Waypoint>();
        SimplePriorityQueue<Waypoint> openSet = new SimplePriorityQueue<Waypoint>();
        openSet.Enqueue(start, Heuristic(start, goal));

        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
        Dictionary<Waypoint, float> gScore = new Dictionary<Waypoint, float>();

        foreach (Waypoint wp in waypoints) {
            gScore.Add(wp, Mathf.Infinity);
        }
        gScore[start] = 0;

        while (openSet.Count > 0) {
            Waypoint current = openSet.Dequeue();

            if (current == goal) {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Waypoint neighbor in current.edges) {
                if (closedSet.Contains(neighbor)) continue;

                float tentative_gScore = gScore[current] + Heuristic(current, neighbor);

                if (tentative_gScore >= gScore[neighbor]) continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_gScore;
                float fScore = gScore[neighbor] + Heuristic(neighbor, goal);

                if (!openSet.Contains(neighbor)) {
                    openSet.Enqueue(neighbor, fScore);
                } else {
                    openSet.UpdatePriority(neighbor, fScore);
                }
            }
        }
        return new List<Waypoint>(); // หาทางไม่เจอ
    }

    private float Heuristic(Waypoint a, Waypoint b) {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    private List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current) {
        List<Waypoint> totalPath = new List<Waypoint> { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}
