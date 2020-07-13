using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyEngine : MonoBehaviour
{
    public float t;
    public int lineTMax;

    OrbitBody[] allbodies;
    Vector3[] velocity;
    bool isStart;

    void Awake()
    {
        //OrbitBody 스크립트를 가진 게임오브젝트 찾기
        allbodies = FindObjectsOfType<OrbitBody>();
        velocity = new Vector3[allbodies.Length];
    }

    void FixedUpdate()
    {
        //OrbitBody 스크립트를 가진 게임오브젝트의 갯수만큼 반복하는 for문
        for (int i = 0; i < allbodies.Length; i++)
        {
            //스페이스 키를 눌렀다면 실행 시작
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isStart = true;
            }

            if (isStart)
            {
                //시작했을 경우, 초기 속도가 설정되어 있지 않다면 첫번째 실행이므로 좌표를 시작 좌표로 설정
                if (!allbodies[i].isSet)
                {
                    allbodies[i].trans.position = allbodies[i].originalPosition;
                }

                //OrbitBody 스크립트의 UpdateVelocity 함수 작동
                allbodies[i].trans.position += allbodies[i].UpdateVelocity(ref velocity[i], allbodies[i].trans.position, false, allbodies) * 0.1f;
            }
        }

        if (!isStart)
        {
            PosWhenTime(t, allbodies);
            DrawOrbitLine(lineTMax, allbodies);
        }
    }

    //시간 t일 때 좌표를 연산하는 함수
    void PosWhenTime(float t, OrbitBody[] bodies)
    {
        Vector3[] TimeVelocity = new Vector3[bodies.Length];
        Vector3[] SimPos = new Vector3[bodies.Length];

        //인덱스
        int j = 0;

        //초기값 설정
        foreach (OrbitBody targetScript in bodies)
        {
            //초기 좌표 설정
            SimPos[j] = targetScript.originalPosition;
            j++;
        }

        //시간 t까지 반복하는 for문
        for (float i = 0; i < t; i += Time.fixedDeltaTime)
        {
            j = 0;

            foreach (OrbitBody targetScript in bodies)
            {
                //UpdateVelocity 함수를 통해 업데이트한 속도를 벡터에 더함
                SimPos[j] += targetScript.UpdateVelocity(ref TimeVelocity[j], SimPos[j], true, bodies) * 0.1f;
                //스크립트의 SimPos를 연산 값으로 설정
                targetScript.SimPos = SimPos[j];
                j++;
            }
        }

        j = 0;

        //결과 좌표 설정
        foreach (OrbitBody targetScript in bodies)
        {
            //좌표 설정
            targetScript.trans.position = SimPos[j];
            //UpdateVelocity 사용 여부 초기화
            targetScript.isSet = false;
            j++;
        }
    }

    //PosWhenTime 함수의 원리로 궤도 라인 효과를 연산하는 함수
    void DrawOrbitLine(float t, OrbitBody[] bodies)
    {
        Vector3[] LineVelocity = new Vector3[bodies.Length];
        Vector3[] SimPos = new Vector3[bodies.Length];
        LineRenderer[] line = new LineRenderer[bodies.Length];

        //인덱스
        int j = 0;
        int k = 0;

        //초기값 설정
        foreach (OrbitBody targetScript in bodies)
        {
            //초기 좌표 설정
            SimPos[j] = targetScript.originalPosition;
            //컴포넌트 할당
            line[j] = targetScript.GetComponent<LineRenderer>();
            j++;
        }

        //인수 t까지 반복하는 for문
        for (float i = 0; i < t; i += Time.fixedDeltaTime)
        {
            j = 0;

            foreach (OrbitBody targetScript in bodies)
            {
                //UpdateVelocity 함수를 통해 업데이트한 속도를 벡터에 더함
                SimPos[j] += targetScript.UpdateVelocity(ref LineVelocity[j], SimPos[j], true, bodies) * 0.1f;
                //스크립트의 SimPos를 연산 값으로 설정
                targetScript.SimPos = SimPos[j];
                //LineRenderer 카운트 값 설정
                line[j].positionCount = (int)(k + 1);
                //LineRenderer line 좌표 설정
                line[j].SetPosition(k, SimPos[j]);
                j++;
            }

            k++;
        }

        //마무리
        foreach (OrbitBody targetScript in bodies)
        {
            //UpdateVelocity 사용 여부 초기화
            targetScript.isSet = false;
        }
    }
}
