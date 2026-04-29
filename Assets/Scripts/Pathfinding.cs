using UnityEngine;
using System.Collections.Generic;
using Priority_Queue; // อย่าลืมใส่ Namespace ของ Priority Queue ที่โหลดมา

public class Pathfinding : MonoBehaviour 
{
    public Waypoint[] waypoints; //

    // ข้อ b) เพิ่ม Parameters เพื่อกำหนดจุดเริ่มและจุดเป้าหมาย
    public List<Waypoint> FindPath(Waypoint start, Waypoint goal) 
    {
        // ชุดของโหนดที่พิจารณาเสร็จแล้ว
        List<Waypoint> closedSet = new List<Waypoint>(); 
        
        // ชุดของโหนดที่กำลังพิจารณา (ใช้ Priority Queue เพื่อประสิทธิภาพสูงสุด)
        SimplePriorityQueue<Waypoint> openSet = new SimplePriorityQueue<Waypoint>();
        openSet.Enqueue(start, Heuristic(start, goal)); 

        // ตารางสำหรับเก็บว่าโหนดนี้เดินมาจากโหนดไหน
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>(); 

        // gScore: ระยะทางจริงจากจุดเริ่มต้นถึงโหนด n
        Dictionary<Waypoint, float> gScore = new Dictionary<Waypoint, float>(); 
        foreach (Waypoint wp in waypoints) {
            gScore.Add(wp, Mathf.Infinity); 
        }
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            // ดึงโหนดที่มีค่า fScore (g+h) ต่ำที่สุดออกมา
            Waypoint current = openSet.Dequeue(); 

            // ถ้าถึงจุดหมายแล้ว ให้รวบรวมเส้นทางและส่งกลับ
            if (current == goal) {
                return ReconstructPath(cameFrom, current); 
            }

            closedSet.Add(current);

            foreach (Waypoint neighbor in current.edges)
            {
                if (closedSet.Contains(neighbor)) continue; 

                // คำนวณระยะทางจากจุดเริ่มผ่าน current ไปยัง neighbor
                float tentative_gScore = gScore[current] + Heuristic(current, neighbor); 

                if (tentative_gScore >= gScore[neighbor]) continue; 

                // ถ้าทางนี้ดีกว่าทางเดิม ให้บันทึกไว้
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
        return new List<Waypoint>(); // กรณีหาทางไม่เจอ
    }

    // ฟังก์ชันช่วยคาดคะเนระยะทาง (Heuristic)
    float Heuristic(Waypoint a, Waypoint b) {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    // ฟังก์ชันรวบรวมเส้นทางจากจุดจบย้อนกลับไปจุดเริ่ม
    List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current) {
        List<Waypoint> totalPath = new List<Waypoint> { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}