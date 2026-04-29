using UnityEngine;
using UnityEngine.AI; 

public class AgentControl : MonoBehaviour
{
    public Transform startPoint; // เพิ่มจุดเริ่มต้น (ลาก Object มาใส่)
    public Transform goal;       // จุดเป้าหมาย
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // 1. ตรวจสอบและย้าย NPC ไปยังจุดเริ่มต้นทันที
        if (startPoint != null)
        {
            // ใช้ Warp เพื่อให้ Agent ไปอยู่ในตำแหน่งที่ถูกต้องบน NavMesh ทันที
            agent.Warp(startPoint.position); 
        }

        // 2. สั่งให้เดินไปยังตำแหน่งเป้าหมาย
        if (goal != null)
        {
            agent.destination = goal.position; 
        }
    }
}