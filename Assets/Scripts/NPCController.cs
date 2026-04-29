using UnityEngine;
using UnityEngine.InputSystem; // ใช้สำหรับ Input System ตัวใหม่
using System.Collections.Generic;

public class NPCController : MonoBehaviour 
{
    public NPCPathfinding npcPathfinding; // อ้างอิง Script Pathfinding Manager
    public Transform targetGoal;    // วัตถุเป้าหมายที่ต้องการให้ NPC เดินไปหา
    public float moveSpeed = 3.0f;  // ความเร็วในการเคลื่อนที่

    private List<Waypoint> currentPath = new List<Waypoint>();
    private int currentStep = 0;

    void Update() 
    {
        // เมื่อกด Space ให้เริ่มคำนวณและเดินทาง
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) 
        {
            StartNavigation();
        }

        MoveAlongPath();
    }

    void StartNavigation() 
    {
        if (npcPathfinding == null || targetGoal == null) return;

        // ข้อ 4a) หาจุด Waypoint ที่ใกล้ตัว NPC และใกล้เป้าหมายที่สุด
        Waypoint start = npcPathfinding.GetClosestWaypoint(transform.position);
        Waypoint goal = npcPathfinding.GetClosestWaypoint(targetGoal.position);
        // คำนวณเส้นทางด้วย A*
        currentPath = npcPathfinding.FindPath(start, goal);
        currentStep = 0; 

        if (currentPath.Count > 0) 
            Debug.Log("Start! " + goal.name);
    }

    void MoveAlongPath() 
    {
        // ถ้าไม่มีเส้นทาง หรือเดินจนครบทุกจุดแล้วให้หยุด
        if (currentPath == null || currentStep >= currentPath.Count) return;

        // ข้อ 4b) เคลื่อนที่ NPC ไปยัง Waypoint ปัจจุบัน
        Vector3 targetPos = currentPath[currentStep].transform.position;
        
        // ล็อกความสูง (Y) ให้เท่ากับ NPC เพื่อไม่ให้ตัวละครจมหรือลอย
        targetPos.y = transform.position.y; 

        // เคลื่อนที่ทีละน้อย (Frame by Frame)
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // หมุนหน้า NPC ไปยังจุดหมาย (Optional - เพื่อความสวยงาม)
        Vector3 direction = targetPos - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10 * Time.deltaTime);
        }

        // เมื่อเดินถึงจุดปัจจุบัน (ในระยะใกล้มาก) ให้เปลี่ยนไปเป้าหมายถัดไป
        if (Vector3.Distance(transform.position, targetPos) < 0.1f) 
        {
            currentStep++;
        }
    }
}
