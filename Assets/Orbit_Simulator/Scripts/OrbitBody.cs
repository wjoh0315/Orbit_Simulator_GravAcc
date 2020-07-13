using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBody : MonoBehaviour
{
    [Header("BodyData")]
    public float Gravity;
    public float mass;
    public Vector3 originalPosition;
    public Vector3 Firstvelocity;

    [HideInInspector]
    public Transform trans;
    [HideInInspector]
    public Vector3 SimPos;
    [HideInInspector]
    public bool isSet;

    void Awake()
    {
        //Transform 컴포넌트 가져오기
        trans = GetComponent<Transform>();
    }

    //속도 업데이트 함수
    public Vector3 UpdateVelocity(ref Vector3 velocity, Vector3 myPos, bool isTimeSim, OrbitBody[] bodies)
    {
        //각 게임오브젝트의 OrbitBody 클래스를 불러오는 foreach문
        foreach (OrbitBody targetScript in bodies)
        {
            //불러온 클래스가 자신이 아니라면 궤도 연산 가능
            if (targetScript != this)
            {
                //방향 벡터
                Vector3 dir = ((isTimeSim ? targetScript.SimPos : targetScript.trans.position) - myPos).normalized;
                //거리
                float r = (myPos - (isTimeSim ? targetScript.SimPos : targetScript.trans.position)).magnitude;
                //중력의 크기 및 방향
                Vector3 force = dir * targetScript.Gravity * mass * targetScript.mass / (r * r);
                //중력 가속도
                Vector3 a = force / mass;
                //현재 속도에 중력 가속도 +
                velocity += a;
            }
        }

        //속도 값을 반환
        velocity += isSet ? new Vector3(0, 0, 0) : Firstvelocity;
        isSet = true;
        return velocity;
    }
}
