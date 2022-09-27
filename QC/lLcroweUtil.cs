using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace lLCroweTool
{

    /// <summary>
    /// lLcrowe 작업에 필요한 여러 유틸들
    /// </summary>
    public static class lLcroweUtil 
    {
        /// <summary>
        /// 함수에서 float형 데이터를 리턴이나 Ref할때 사용하는 값
        /// </summary>
        public static float valueF;

        /// <summary>
        /// 함수에서 int형 데이터를 리턴이나 Ref할때 사용하는 값
        /// </summary>
        public static int value;


        //  String 클래스를 사용하는 경우 
        //  -문자열을 수정하는 수가 적을 경우(stringbuilder는 string에 비해서 무시해도 좋을 수준의 성능향상을 제공하거나 전혀 제공하지 않을 수 있음)
        //  -부분적인 문자열 글자로 고정된 수의 문자열 연결 작업을 할때(컴파일러가 연결 작업을 단일 작업으로 결합할 수 있음)
        //  -문자열을 작성하는 동안 광법위한 검색 작업을 수행할 때(StringBuilder 클래스는 IndexOf 또는 StartsWith같은 함수가 없다)

        //  StringBuiler 클래스를 사용 하는 경우
        //  -응용 프로그램이 설계시에는 알 수 없는 횟수의 문자열을 변경해야 할 때(사용자의 입력등으로 조합할때 )
        //  -문자열에서 많은 횟수의 변경이 예상될때

        //---------------스트링더하기&스트링빌더원리----------------
        //txt = txt + "1";
        //1. 힙에 특정문자열을 담는 공간을 할당
        //2. 스택에 있는 txt변수에 1번과정에서 할당된 힙의 주소를 저장
        //3. txt + "1" 동작을 수행하기 위해 txt.length + "1".length에 해당되는 크기의 메모리를 힙에 할당.
        //해당 메모리에 txt변수가 가리키는 힙의 문자열과 "1"문자열을 복사한다.
        //4. 다시 스택에 있는 txt변수에 3번과정에서 새롭게 할당된 힙의 주소를 저장
        //5. 3번, 4번의 과정을 X만큼 반복

        //stringBuilder.append("1");
        //1. stringBuilder는 내부적으로 일정한 양의 메모리를 미리할당한다.
        //2. Append 메서드에 들어온 이자를 미리 할당한 메모리에 복사한다.
        //3. 2번과정을 X만큼 반복. Append로 추가된 문자열이 미리 할당한 메모리보다 많아지면 새롭게 여유분의 메모리를 할당
        //4. ToString 메서드를 호출하면 연속적으로 연결된 하나의 문자열을 반환.

        private static StringBuilder builder = new StringBuilder();

        /// <summary>
        /// 여러문자열들 결합해주는 함수
        /// </summary>
        /// <param name="strings">집어넣을 문자열들</param>
        /// <returns>합쳐진 문자열</returns>
        public static string GetCombineString(params string[] strings)
        {   
            //builder.Clear();
            builder.Length = 0;

            for (int i = 0; i < strings.Length; i++)
            {
                builder.Append(strings[i]);
            }
            return builder.ToString(); //반환
        }

        /// <summary>
        /// 컴포넌트복사
        /// </summary>
        /// <typeparam name="T">컴포넌트 타입</typeparam>
        /// <param name="original">컴포넌트  타입</param>
        /// <param name="target">붙여넣을게임오브젝트</param>
        /// <returns>복사한 컴포넌트</returns>
        public static T GetCopyOf<T>(T original, GameObject target) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = target.AddComponent(type);

            System.Reflection.FieldInfo[] fields = type.GetFields();

            //foreach (System.Reflection.FieldInfo field in fields)
            //{
            //    field.SetValue(copy, field.GetValue(original));
            //}

            for (value = 0; value < fields.Length; value++)
            {
                System.Reflection.FieldInfo field = fields[value];
                field.SetValue(copy, field.GetValue(original));
            } 

            return copy as T;
        }

        /// <summary>
        /// 클래스복사
        /// </summary>
        /// <typeparam name="T">클래스</typeparam>
        /// <param name="original">원본타입</param>
        /// <param name="copyTarget">복사할 new한 클래스</param>
        /// <returns>값을 복사한 클래스</returns>
        public static T GetCopyOf<T>(T original, T copyTarget) where T : class
        {
            System.Type type = original.GetType();

            System.Reflection.FieldInfo[] fields = type.GetFields();

            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copyTarget, field.GetValue(original));
            }
            
            return copyTarget as T;
        }

        /// <summary>
        /// 해당 게임오브젝트에서 컴포넌트를 찾는 함수
        /// </summary>
        /// <typeparam name="T">찾을 컴포넌트타입</typeparam>
        /// <param name="go">타겟팅할 게임오브젝트</param>
        /// <returns>찾은 컴포넌트</returns>
        public static T GetAddComponent<T>(GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T component))
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        //https://daveoh.wordpress.com/2013/05/02/unity3d-vector3-magnitude-vs-sqrmagnitude/
        //https://answers.unity.com/questions/307612/inversetargetpoint-vs-vector3distance-which-is.html
        //https://answers.unity.com/questions/384932/best-way-to-find-distance.html
        //https://answers.unity.com/questions/125882/vectordistance-performance.html

        //PC(Intel Core i5, 10 million executions per run per function, averaged result over 10 runs)
        //sqrMagnitude: 2853 ms. (96.13% of magnitude’s time)
        //magnitude: 2968 ms. (104.03% of sqrMagnitude’s time)

        //Android(Samsung Galaxy S II, 10 million executions per run per function, averaged result over 10 runs)
        //sqrMagnitude: 6155 ms. (77.11% of magnitude’s time)
        //magnitude: 7982 ms. (129.68% of sqrMagnitude’s time)

        //명시적 형변환과 묵시적 형변환의 차이점
        //*결과적인 차이는 없다
        //*명시적 형변환의 경우 내부적으로 임시변수를 생성에 대입하는 방식으로 성능 저하를 일으킬 수 있다.
        //*묵시적 형변환의 경우 데이터 손실에 대한 경고가 발생한다.
        //CLR(공용 언어 런타임)은 값 형식을 boxing할 때 값을 System.Object 인스턴스 내부에 래핑하고 관리되는 힙에 저장합니다.
        //unboxing하면 개체에서 값 형식이 추출됩니다.
        //Boxing은 암시적이며 unboxing은 명시적입니다.
        //단순 할당에서는 boxing과 unboxing을 수행하는 데 많은 계산 과정이 필요합니다.
        //값 형식을 boxing할 때는 새로운 개체를 할당하고 생성해야 합니다. <=값 형식이 boxing되면 완전히 새로운 개체가 생성되어야 합니다.
        //정도는 약간 덜하지만 unboxing에 필요한 캐스트에도 상당한 계산 과정이 필요합니다
        //Boxing 및 unboxing은 계산을 많이 해야 하는 프로세스입니다. 
        //이 작업은 단순 참조 할당보다 20배나 오래 걸립니다.
        //unboxing 시 캐스팅 프로세스는 할당의 4배에 달하는 시간이 소요될 수 있습니다.
        //성능속도: 박싱(느림) < 언박싱 < 단순할당(빠름)
        //박싱 언박싱 참고할것
        //박싱
        //int a = 1;
        //object b = a;

        //public enum STATE{A = 1, B = 2,}
        //STATE a = STATE.A;
        //STATE b = STATE.B;
        //// Enum 비교 시 a, b가 boxing이 발생한다
        //if (a.Equals(b)) { }
        //int ia = 1;
        //int ib = 2;
        //// 단순 enum 비교로 boxing이 발생하지 않는다
        //if (ia.Equals(ib)) { }

        //boxing은 암시적으로 사용되고 있기 때문에 주의 깊게 생각하고 사용하지 않으면 생각보다 많은 곳에서 발생하게 된다.
        //꼭 필요한 곳에서 사용하는 것은 어쩔 수 없지만, 불필요한 곳에서 사용하게 되는 경우 성능 저하를 발생 시키기 때문에 유의해야한다.
        //=>언제한번 전체다 처리해버려야겠는걸. 결국 값형식으로 비교하면 Equal 할때 GC가 안쌓인다 라는것이군
        //=>ilspy로보니 Equals이 해당값과 object가 있다. object로 가면 GC가 쌓임
        //=> == 로 할시 ceq가 작동됨. equals로 할시 boxing callvirt 진행
        //ceq 스택의 두 값을 꺼내고, 같으면 1, 다르면 0을 스택에 넣는다.


        //타임.타임만 메인스레드말고는 작동안함//유니티가 동기화하면서 하면서 안되는 스레드들이 있다.//랜덤도 안됨//티슈에서 뽑아쓰듯이


        //CMP 연산자
        //0040150E 메모리주소
        //C74424 오퍼레이션코드 
        //Dword 4byte 
        //PTR 포인트
        //stack segment 
        //: []
        //오피코드 확인//exe. CPU가 적힘

        //JMP 점프
        //JNZ 두개의 연산을 해가주고 
        //SS 소스타겟

        //0040150E  |.  C74424 0C 000>MOV DWORD PTR SS:[ESP+C],0
        //00401516  |.  C74424 08 000>MOV DWORD PTR SS:[ESP+8],0

        //0040151E  |.  837C24 0C 01  CMP DWORD PTR SS:[ESP+C],1
        //00401523  |.  75 0A JNZ SHORT a.0040152F
        //00401525  |.  C74424 08 010>MOV DWORD PTR SS:[ESP+8],1
        //0040152D  |.  EB 2A JMP SHORT a.00401559

        //0040152F  |>  837C24 0C 02  CMP DWORD PTR SS:[ESP+C],2
        //00401534  |.  75 0A JNZ SHORT a.00401540
        //00401536  |.  C74424 08 020>MOV DWORD PTR SS:[ESP+8],2
        //0040153E  |.  EB 19         JMP SHORT a.00401559

        //00401540  |>  837C24 0C 00  CMP DWORD PTR SS:[ESP+C],0
        //00401545  |.  75 0A JNZ SHORT a.00401551
        //00401547  |.  C74424 08 030>MOV DWORD PTR SS:[ESP+8],3
        //0040154F  |.  EB 08         JMP SHORT a.00401559

        //00401551  |>  C74424 08 040>MOV DWORD PTR SS:[ESP+8],4


        //00401559  |>  C74424 0C 000>MOV DWORD PTR SS:[ESP+C],0
        //00401561  |.  C74424 08 000>MOV DWORD PTR SS:[ESP+8],0


        //00401569  |.  8B4424 0C MOV EAX,DWORD PTR SS:[ESP+C]
        //0040156D  |.  83F8 01       CMP EAX,1
        //00401570  |.  74 0B         JE SHORT a.0040157D
        //00401572  |.  83F8 02       CMP EAX,2
        //00401575  |.  74 10         JE SHORT a.00401587
        //00401577  |.  85C0 TEST EAX,EAX
        //00401579  |.  74 16         JE SHORT a.00401591
        //0040157B  |.  EB 1E         JMP SHORT a.0040159B
        //0040157D  |>  C74424 08 010>MOV DWORD PTR SS:[ESP+8],1
        //00401585  |.  EB 1C JMP SHORT a.004015A3
        //00401587  |>  C74424 08 020>MOV DWORD PTR SS:[ESP+8],2
        //0040158F  |.  EB 12         JMP SHORT a.004015A3
        //00401591  |>  C74424 08 030>MOV DWORD PTR SS:[ESP+8],3
        //00401599  |.  EB 08         JMP SHORT a.004015A3
        //0040159B  |>  C74424 08 040>MOV DWORD PTR SS:[ESP+8],4
        //004015A3  |>  C9 LEAVE
        //004015A4  \.  C3 RETN

        //0040150E  |.  C74424 0C 000>MOV DWORD PTR SS:[ESP+C],0   int a = 0;
        //00401516  |.  C74424 08 000>MOV DWORD PTR SS:[ESP+8],0   int b = 0;
        //0040151E  |.  837C24 0C 01  CMP DWORD PTR SS:[ESP+C],1   if (a == 1
        //00401523  |.  75 0A JNZ SHORT a.0040152F         goto ax)
        //00401525  |.  C74424 08 010>MOV DWORD PTR SS:[ESP+8],1   b = 1;
        //0040152D  |.  EB 2A JMP SHORT a.00401559         goto end
        //0040152F  |>  837C24 0C 02  CMP DWORD PTR SS:[ESP+C],2   ax, (if a == 2
        //00401534  |.  75 0A JNZ SHORT a.00401540         goto bx)
        //00401536  |.  C74424 08 020>MOV DWORD PTR SS:[ESP+8],2   b = 2;
        //0040153E  |.  EB 19         JMP SHORT a.00401559         goto end
        //00401540  |>  837C24 0C 00  CMP DWORD PTR SS:[ESP+C],0   bx, (if a == 0
        //00401545  |.  75 0A JNZ SHORT a.00401551         goto cx)
        //00401547  |.  C74424 08 030>MOV DWORD PTR SS:[ESP+8],3   b = 3;
        //0040154F  |.  EB 08         JMP SHORT a.00401559         goto end
        //00401551  |>  C74424 08 040>MOV DWORD PTR SS:[ESP+8],4   cx, b = 4;
        //00401559  |>  C74424 0C 000>MOV DWORD PTR SS:[ESP+C],0   end, a = 0;
        //00401561  |.  C74424 08 000>MOV DWORD PTR SS:[ESP+8],0   b = 0;
        //00401569  |.  8B4424 0C MOV EAX,DWORD PTR SS:[ESP+C]
        //0040156D  |.  83F8 01       CMP EAX,1
        //00401570  |.  74 0B         JE SHORT a.0040157D
        //00401572  |.  83F8 02       CMP EAX,2
        //00401575  |.  74 10         JE SHORT a.00401587
        //00401577  |.  85C0 TEST EAX,EAX
        //00401579  |.  74 16         JE SHORT a.00401591
        //0040157B  |.  EB 1E         JMP SHORT a.0040159B
        //0040157D  |>  C74424 08 010>MOV DWORD PTR SS:[ESP+8],1
        //00401585  |.  EB 1C JMP SHORT a.004015A3
        //00401587  |>  C74424 08 020>MOV DWORD PTR SS:[ESP+8],2
        //0040158F  |.  EB 12         JMP SHORT a.004015A3
        //00401591  |>  C74424 08 030>MOV DWORD PTR SS:[ESP+8],3
        //00401599  |.  EB 08         JMP SHORT a.004015A3
        //0040159B  |>  C74424 08 040>MOV DWORD PTR SS:[ESP+8],4
        //004015A3  |>  C9 LEAVE
        //004015A4  \.  C3 RETN

        /// <summary>
        /// 게임오브젝트를 씬이 변경될때 파괴되지않도록 하는 함수
        /// </summary>
        /// <param name="_targetGameObject">파괴시키지않을 게임오브젝트</param>
        public static void DontDestroyTargetObject(GameObject _targetGameObject)
        {
            Object.DontDestroyOnLoad(_targetGameObject);
        }

        /// <summary>
        /// 타겟이 될 트랜스폼을 최상단부모로 옮기
        /// </summary>
        /// <param name="target">트랜스폼</param>
        public static void DeActiveSetNullParent(Transform target)
        {
            if (target.childCount == 0)
            {
                target.SetParent(null);
                target.gameObject.SetActive(false);
            }
            else
            {
                Transform[] transformArray = target.GetComponentsInChildren<Transform>();
                for (int i = 0; i < transformArray.Length; i++)
                {
                    transformArray[i].SetParent(null);
                    transformArray[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 특정부모에 타겟팅한 오브젝트를 자식으로 집어넣고 부모위치와 회전값을 집어넣음
        /// </summary>
        /// <param name="parent">집어넣을 부모</param>
        /// <param name="target">타겟팅될 객체</param>
        public static void SetParentToTarget(Transform parent, Transform target)
        {
            target.SetParent(parent);
            target.SetPositionAndRotation(parent.position, Quaternion.identity);
        }

        /// <summary>
        /// 거리를 체크해주는 함수.유니티거리체크보다 빠름//차이는 솔직히 매우 미세함
        /// </summary>
        /// <param name="a">a위치좌표</param>
        /// <param name="b">b위치좌표</param>
        /// <param name="distance">거리</param>
        /// <returns>해당거리보다 가까운지 여부</returns>
        public static bool CheckDistance(Vector2 a, Vector2 b, float distance)
        {
            bool check = GetDistance(a, b) < distance * distance + 0.001f;
            return check;
        }

        /// <summary>
        /// 거리에 대한 크기를 가져오는 함수//비교할 거리의 제곱을 비교할것
        /// </summary>
        /// <param name="a">a위치좌표</param>
        /// <param name="b">b위치좌표</param>
        /// <returns>거리의 크기</returns>
        public static float GetDistance(Vector2 a, Vector2 b)
        {
            float distacne = (a - b).sqrMagnitude;
            return distacne;
        }


        /// <summary>
        /// A->B방향으로 회전값 가져오는 함수
        /// </summary>
        /// <param name="rotateTarget">회전하는 오브젝트</param>
        /// <param name="lookTarget">봐야될 위치</param>
        /// <param name="removeAngle">필요시 따로 설정하여 각도변환하기</param>
        /// <returns>회전값</returns>
        public static Quaternion GetRotation(Vector3 rotateTarget, Vector3 lookTarget, float removeAngle = 90)
        {
            Vector2 targetDir = lookTarget - rotateTarget;
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - removeAngle;
            //Debug.Log("Angle :" + newangle + "angle2" + Vector2.Angle(rotateTarget, lookTarget));
            Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            return quaternion;
        }

        /// <summary>
        /// A->B방향으로 제한된 회전값 가져오는 함수
        /// </summary>
        /// <param name="rotateTarget">회전하는 오브젝트</param>
        /// <param name="lookTarget">봐야될 위치</param>
        /// <param name="limitAngle">제한값</param>
        /// <param name="removeAngle">필요시 따로 설정하여 각도변환하기</param>
        /// <returns>회전값</returns>
        public static Quaternion GetRotation(Transform rotateTarget, Transform lookTarget, float limitAngle, float removeAngle = 90)
        {
            //각도 제한 테스트해야됨
            //봐야될 위치 기준으로 각도를 일정정해주고 회저

            //회전각도체크할 오브젝트 => lookTarget
            //회전시킬오브젝트 => rotateTarget;
            //볼대상의 각도에서 체크


            Vector3 euler = lookTarget.rotation.eulerAngles;
            euler.z = euler.z > 180 ? euler.z - 360 : euler.z;

            Vector3 rotateEuler = rotateTarget.rotation.eulerAngles;
            rotateEuler.z = rotateEuler.z > 180 ? rotateEuler.z - 360 : rotateEuler.z;

            rotateEuler.z = Mathf.Clamp(rotateEuler.z, euler.z - limitAngle, euler.z + limitAngle);



            Debug.Log($"봐야되는 오브젝트각도 : {euler.z} 회전오브젝트각도  {rotateEuler.z}");
            //if (newangle > limitAngle)
            //{
            //    return targetObject.rotation;
            //}

            


          



        

            Vector2 targetDir = lookTarget.position - rotateTarget.position;
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - removeAngle;
            //Debug.Log("Angle :" + newangle + "angle2" + Vector2.Angle(rotateTarget, lookTarget));
            Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            quaternion *= Quaternion.Euler(rotateEuler);
            return quaternion;
        }

        /// <summary>
        /// 투사체가 충돌체와 충돌시 특정각도내에 있을시 투사체의 발사각을 바꾸는 함수
        /// </summary>
        /// <param name="collision">충돌한 충돌체</param>
        /// <param name="projectileObject">투사체 오브젝트</param>
        /// <param name="reflectAngle">최대 도탄각도</param>
        /// <returns>도탄됫는지 여부</returns>
        public static bool ActionProjectileReflect(Collision2D collision, Transform projectileObject, float reflectAngle)
        {   
            //반사위치체크
            //결과값=Vector2.Reflect(입사각의 위치(충돌각도), 노말값의 위치(충돌시 타겟의 각도)
            //노말값은 충돌했을시 각도를 말하는것
            Vector2 direction = Vector2.Reflect(projectileObject.transform.up, collision.contacts[0].normal);

            //반사각도 확인
            float InAngle = Vector2.Angle(projectileObject.transform.up, direction);
            //Debug.Log($"{collision.collider.name} 충돌, 입사각 : {InAngle}, 입사각 위치 : { (Vector2)refectionAttackBox.transform.up} , 반사 결과값 : {direction } ");

            //왠만해선60~70각도이하부터가 볼만함//55도가 적당한것같기도하구
            if (InAngle > reflectAngle)
            {
                return false;
            }

            projectileObject.transform.up = direction;//방향동기화
            return true;
        }

        //사용안함
        public static float CalPow(int num, int RepeatValue)
        {
            float value = 0;

            value = (Mathf.Pow(num, RepeatValue + 1) - 1) / (num - 1);


            return value;
            //for (int i = 0; i < RepeatValue; i++)
            //{
            //    value += Mathf.Pow(num, i);
            //}


        }

        //사용안함
        public static Vector2 GetWorldPosToUIPos(Vector2 worldPos, Canvas canvas, Camera camera)
        {
            //미완
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
            Vector2 ViewportPosition = camera.WorldToViewportPoint(worldPos);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //Rect의 앵커포지션으로 반환
            return WorldObject_ScreenPosition;
        } 

        /// <summary>
        /// 지정한 좌표간의 센터위치 구하는 함수
        /// </summary>
        /// <param name="_points">좌표 값들</param>
        /// <returns></returns>
        public static Vector2 Centroid(Vector2[] _points)
        {
            Vector2 center = Vector2.zero;
            for (int i = 0; i < _points.Length; i++)
            {
                center += _points[i];
            }
            center /= _points.Length;
            return center;
        }

        //======================================================================
        //랜덤부분---------------------------------------------------------------
        //======================================================================

        /// <summary>
        /// 확률 계산
        /// </summary>
        /// <param name="probabilityNum">집어넣을 확률</param>
        /// <returns>확률에 들어갔는가 여부</returns>
        public static bool ProbabilityCal(int probabilityNum)
        {
            //bool isTure = false;
            int num = Random.Range(0, 101);
            return probabilityNum >= num ? true : false;
            //if (probabilityNum >= num)
            //{
            //    isTure = true;
            //}
            //else
            //{
            //    isTure = false;
            //}
            //return isTure;
        }



        /// <summary>
        /// 랜덤한 원형위치를 가져오는 함수
        /// </summary>
        /// <param name="targetPos">타겟 위치</param>
        /// <param name="size">크기</param>
        /// <param name="isCheck">체크여부</param>
        /// <returns>랜덤위치</returns>
        public static Vector2 GetRandomCirclePosition(Transform targetPos, float size, bool isCheck = false)
        {
            Vector2 randomPos = isCheck ? (Vector2)targetPos.position + Random.insideUnitCircle * size : targetPos.position;
            return randomPos;
        }

        /// <summary>
        /// 랜덤한 원형위치를 가져오는 함수
        /// </summary>
        /// <param name="targetPos">타겟 위치</param>
        /// <returns>랜덤위치</returns>
        public static Vector2 GetRandomCirclePosition(Vector2 targetPos, float size, bool isCheck = false)
        {
            Vector2 randomPos = isCheck ? targetPos + Random.insideUnitCircle * size : targetPos;
            return randomPos;
        }

        //======================================================================


        //타일맵관련

        /// <summary>
        /// 타일맵에 배치되있는 포지션을 모두가져오기. 매프레임으로 돌리지 말기
        /// </summary>
        /// <returns>타일이 있는 위치들</returns>
        public static Vector3Int[] GetAllTilePos(Tilemap tilemap)
        {
            //모든타일가져오기
            BoundsInt bounds = tilemap.cellBounds;
            List<Vector3Int> posList = new List<Vector3Int>();

            //int xMax = floorTileMap.GetTileMap().cellBounds.xMax;
            //int xMin = floorTileMap.GetTileMap().cellBounds.xMin;
            //int yMax = floorTileMap.GetTileMap().cellBounds.yMax;
            //int yMin = floorTileMap.GetTileMap().cellBounds.yMin;
            //Debug.Log(xMax + ", " + yMin + ", " + xMax + ", " + yMin + "");

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int target = new Vector3Int(x, y, 0);

                    TileBase tile = tilemap.GetTile(target);
                    if (tile != null)
                    {
                        //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                        posList.Add(new Vector3Int(x, y, 0));
                        //count++;
                    }
                    else
                    {
                        // Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    }
                }
            }
            return posList.ToArray();
        }

        /// <summary>
        /// 해당타일위치의 근처타일위치를 가져오는 함수
        /// </summary>
        /// <param name="tilePos">타일위치</param>
        /// <returns>위, 아래, 좌, 우  위치반환</returns>
        public static Vector3Int[] GetSideTilePos(Vector3Int tilePos, Tilemap tilemap)
        {
            Vector3Int upTilePos = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
            Vector3Int downTilePos = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);
            Vector3Int leftTilePos = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
            Vector3Int rightTilePos = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);

            Vector3Int[] sidePosArray = { upTilePos, downTilePos, leftTilePos, rightTilePos };

            return sidePosArray;
        }

        /// <summary>
        /// 해당타일위치의 근처타일을 가져오는 함수
        /// </summary>
        /// <param name="tilePos">타일위치</param>
        /// <returns>위, 아래, 좌, 우  타일반환</returns>
        public static Tile[] GetSideTile(Vector3Int tilePos, Tilemap tilemap)
        {
            Vector3Int[] sidePosArray = GetSideTilePos(tilePos, tilemap);
            //upTilePos = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
            //downTilePos = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);
            //leftTilePos = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
            //rightTilePos = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
            Tile upTile = tilemap.GetTile<Tile>(sidePosArray[0]);
            Tile downTile = tilemap.GetTile<Tile>(sidePosArray[1]);
            Tile leftTile = tilemap.GetTile<Tile>(sidePosArray[2]);
            Tile rightTile = tilemap.GetTile<Tile>(sidePosArray[3]);

            Tile[] sideTileArray = { upTile, downTile, leftTile, rightTile };

            return sideTileArray;
        }

        /// <summary>
        /// 해당타일위치의 근처타일이 존재하는 여부를 가져오는 함수
        /// </summary>
        /// <param name="tilePos"></param>
        /// <returns>위, 아래, 좌, 우  타일존재여부반환</returns>
        public static bool[] GetSideTileIsHas(Vector3Int tilePos, Tilemap tilemap)
        {
            Vector3Int[] sidePosArray = GetSideTilePos(tilePos, tilemap);

            bool upTile = tilemap.HasTile(sidePosArray[0]);
            bool downTile = tilemap.HasTile(sidePosArray[1]);
            bool leftTile = tilemap.HasTile(sidePosArray[2]);
            bool rightTile = tilemap.HasTile(sidePosArray[3]);

            bool[] sideIsHasArray = { upTile, downTile, leftTile, rightTile };

            return sideIsHasArray;
        }

        /// <summary>
        /// 좌표에 있는 타일이 사이드타일인지 확인하는 함수
        /// </summary>
        /// <param name="tilePos">타일위치</param>
        /// <returns>사이드타일여부</returns>
        public static bool GetIsSideTile(Vector3Int tilePos, Tilemap tilemap)
        {
            bool[] checkArray = GetSideTileIsHas(tilePos, tilemap);

            for (int i = 0; i < checkArray.Length; i++)
            {
                if (!checkArray[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 월드위치의 타일맵셀위치로 반환
        /// </summary>
        /// <param name="pos">월드위치</param>
        /// <returns>타일맵 셀위치</returns>
        public static Vector3Int GetWorldToCell(Vector2 pos, Tilemap tilemap)
        {
            //Vector3Int cellLocalPos = tilemap.LocalToCell(pos);//로컬 투 셀 위치로 변환//사용안함                    
            //Debug.Log("셀 위치:" + cellPos + ",로컬셀 위치:" + cellLocalPos + ",마우스위치:" + pos);        
            return tilemap.WorldToCell(pos);//월드 투 셀 위치로 변환                
        }

        //셀 위치:(-1, 3, 0),로컬셀 위치:(-1, 3, 0),마우스위치:(-0.4, 3.2, 0.0)
        //셀 위치:(-1, 3, 0),로컬셀 위치:(-1, 3, 0),마우스위치:(-0.6, 3.4, 0.0)
        //셀 위치:(2, 4, 0),로컬셀 위치:(-2, 1, 0),마우스위치:(-1.2, 1.6, 0.0)
        //셀 위치:(-1, 3, 0),로컬셀 위치:(-5, 0, 0),마우스위치:(-4.2, 0.9, 0.0)
        //월드포지션으로 하느게 맞아보임
        //로컬테스트 =>작동안됨


        /// <summary>
        /// 특정타일을 위치에 세팅하는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="targetTile">타일</param>
        public static void SetTile(Vector3Int pos, TileBase targetTile, Tilemap tilemap)
        {
            tilemap.SetTile(pos, targetTile);
        }

        /// <summary>
        /// 타일맵상에 존재여부를 체크후 특정타일을 위치에 세팅하는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="targetTile">타일</param>
        /// <param name="targetTileMap">확인할 타일맵</param>
        /// <param name="isExist">확인할 존재여부</param>
        public static void SetTile(Vector3Int pos, TileBase targetTile, Tilemap originTilemap, Tilemap targetTileMap, bool isExist)
        {
            if (targetTileMap.HasTile(pos) == isExist)//존재여부 체크
            {
                originTilemap.SetTile(pos, targetTile);
            }
        }

        /// <summary>
        /// 해당위치 색깔을 세팅해주는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="color">색깔</param>
        public static void SetTile(Vector3Int pos, Color color, Tilemap tilemap)
        {
            //tilemap.SetTileFlags(cellPos, TileFlags.None);
            tilemap.SetColor(pos, color);
        }

        /// <summary>
        /// 해당위치 색깔을 세팅해주는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="alpha">알파값</param>
        public static void SetTile(Vector3Int pos, float alpha, Tilemap tilemap)
        {
            Color color = tilemap.GetColor(pos);
            color.a = alpha;
            //Debug.Log(tilemap.GetTile(cellPos));
            //Debug.Log(tilemap.GetTileFlags(cellPos));
            //타일플래그 설정
            //tilemap.SetTileFlags(cellPos, TileFlags.LockTransform);
            tilemap.SetColor(pos, color);
        }

        /// <summary>
        /// 해당위치 색깔을 세팅해주는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="red">레드값</param>
        /// <param name="green">그린값</param>
        /// <param name="blue">블루값</param>
        /// <param name="alpha">알파값</param>
        public static void SetTile(Vector3Int pos, float red, float green, float blue, float alpha, Tilemap tilemap)
        {
            Color color = tilemap.GetColor(pos);
            color.r = red;
            color.g = green;
            color.b = blue;
            color.a = alpha;

            //Debug.Log(tilemap.GetTile(cellPos));
            //Debug.Log(tilemap.GetTileFlags(cellPos));
            //타일플래그 설정
            //tilemap.SetTileFlags(cellPos, TileFlags.LockTransform);
            tilemap.SetColor(pos, color);
        }

        /// <summary>
        /// 해당위치의 타일작동조건설정을 세팅하는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <param name="tileFlags">타일플래그</param>
        public static void SetTile(Vector3Int pos, TileFlags tileFlags, Tilemap tilemap)
        {
            tilemap.SetTileFlags(pos, tileFlags);
        }

        /// <summary>
        /// 해당위치의 타일을 가져오는함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <returns>위치의 타일</returns>
        public static TileBase GetTile(Vector3Int pos, Tilemap tilemap)
        {
            return tilemap.GetTile(pos);
        }

        /// <summary>
        /// 박스형태로 채워주기 함수
        /// </summary>
        /// <param name="targetStartPos">시작위치</param>
        /// <param name="targetEndPos">마지막위치</param>
        /// <param name="tile">배치할 타일</param>
        public static void BoxFill(Vector2 targetStartPos, Vector2 targetEndPos, TileBase tile, Tilemap tilemap)
        {
            Vector3Int startPos = tilemap.WorldToCell(targetStartPos);
            Vector3Int endPos = tilemap.WorldToCell(targetEndPos);
            
            //방향처리
            var xDir = startPos.x < endPos.x ? 1 : -1;
            var yDir = startPos.y < endPos.y ? 1 : -1;
            
            //놓을 타일수
            int xcolumn = 1 + Mathf.Abs(startPos.x - endPos.x);
            int ycolumn = 1 + Mathf.Abs(startPos.y - endPos.y);
            
            //그리기시작
            for (var x = 0; x < xcolumn; x++)
            {
                for (var y = 0; y < ycolumn; y++)
                {
                    var tilePos = startPos + new Vector3Int(x * xDir, y * yDir, 0);
                    tilemap.SetTile(tilePos, tile);
                }
            }
        }

        /// <summary>
        /// 박스형태로 채워주기 함수
        /// </summary>
        /// <param name="targetStartPos">시작위치</param>
        /// <param name="targetEndPos">마지막위치</param>
        /// <param name="tile">배치할 타일</param>
        public static void BoxFill(Vector3Int targetStartPos, Vector3Int targetEndPos, TileBase tile, Tilemap tilemap)
        {
            //Determine directions on X and Y axis
            //방향처리
            var xDir = targetStartPos.x < targetEndPos.x ? 1 : -1;
            var yDir = targetStartPos.y < targetEndPos.y ? 1 : -1;
            //How many tiles on each axis?
            //놓을 타일수
            int xcolumn = 1 + Mathf.Abs(targetStartPos.x - targetEndPos.x);
            int ycolumn = 1 + Mathf.Abs(targetStartPos.y - targetEndPos.y);
            //Start painting
            //놓기시작
            for (var x = 0; x < xcolumn; x++)
            {
                for (var y = 0; y < ycolumn; y++)
                {
                    var tilePos = targetStartPos + new Vector3Int(x * xDir, y * yDir, 0);
                    tilemap.SetTile(tilePos, tile);
                }
            }
        }

        /// <summary>
        /// 그리드에 맞게 좌표를 스냅하는 함수
        /// </summary>
        /// <param name="position">원본 좌표</param>
        /// <returns>그리드에 맞게 스냅된 좌표</returns>
        public static Vector2 SnapPosToGridPos(Vector2 position, Tilemap tilemap)
        {
            Vector3Int cellPos = tilemap.WorldToCell(position);
            position = tilemap.GetCellCenterWorld(cellPos);
            return position;
        }

        /// <summary>
        /// 특정영역파괴
        /// </summary>
        /// <param name="radius">반경</param>
        /// <param name="position">위치</param>
        public static void DestroyArea(float radius, Vector2 position, Tilemap tilemap)
        {
            int radiusInt = Mathf.RoundToInt(radius) + 1;//1개의 크기를 더확인해줌//위치확인용

            for (int i = -radiusInt; i <= radiusInt; i++)
            {
                for (int j = -radiusInt; j <= radiusInt; j++)
                {
                    //새위치를 지정
                    Vector2 newPos = new Vector2(position.x + i, position.y + j);

                    //해당위치에서부터 거리체크
                    if (Vector2.Distance(newPos, position) <= radius)
                    //if (Vector3.Distance(targetDestroyPos, position) - 0.001f <= radius) 
                    {
                        //파괴로직
                        Vector3Int targetDestroyPos = tilemap.WorldToCell(newPos);
                        tilemap.SetTile(targetDestroyPos, null);
                        //추가처리
                    }
                }
            }
        }

        //https://playground10.tistory.com/62
        //DDA 알고리즘
        public static void DDALine(Vector2 targetStartPos, Vector2 targetEndPos, Tile tile, Tilemap tilemap, bool fillGaps)
        {

            Vector3Int startPos = tilemap.WorldToCell(targetStartPos);
            Vector3Int endPos = tilemap.WorldToCell(targetEndPos);


            foreach (Vector3Int point in GetPointsOnLine(startPos, endPos, fillGaps))
            {
                Vector3Int paintPos = new Vector3Int(point.x, point.y, point.z);
                tilemap.SetTile(paintPos, tile);
            }
            
            

            //GetPointsOnLine(startPos, endPos, lineBrush.fillGaps)
            ////초기값
            //Vector3Int startPos = tilemap.WorldToCell(targetStartPos);
            //Vector3Int endPos = tilemap.WorldToCell(targetEndPos);

            ////방향처리
            //var xDir = startPos.x < endPos.x ? 1 : -1;
            //var yDir = startPos.y < endPos.y ? 1 : -1;

            ////놓을 타일수
            //int xcolumn = 1 + Mathf.Abs(startPos.x - endPos.x);
            //int ycolumn = 1 + Mathf.Abs(startPos.y - endPos.y);


            //int x = xcolumn;
            //int y = ycolumn;
            //int w = endPos.x - startPos.x;
            //int h = endPos.y - startPos.y;
            //int f = 2 * h - w;

            ////각 판별식 공식
            //int dF1 = 2 * h;
            //int dF2 = 2 * (h - w);

            //for (x = startPos.x; x <= endPos.x; x++)
            //{
            //    //점 그리기
            //    tilemap.SetTile(new Vector3Int(x, y), tile);

            //    if (f < 0)
            //    {
            //        //0보다 작으면 그에 맞는 공식으로 판별식 갱신, y값은 그대로 
            //        f += dF1;
            //    }
            //    else
            //    {
            //        //0보다 크거나 같으면
            //        //그에 맞는 공식으로 반별식 갱신, y값은 증가
            //        ++y;
            //        f += dF2;
            //    }
            //}
        }
       
        /// <summary>
        /// Enumerates all the points between the start and end position which are
        /// linked diagonally or orthogonally.
        /// </summary>
        /// <param name="startPos">Start position of the line.</param>
        /// <param name="endPos">End position of the line.</param>
        /// <param name="fillGaps">Fills any gaps between the start and end position so that
        /// all points are linked only orthogonally.</param>
        /// <returns>Returns an IEnumerable which enumerates all the points between the start and end position which are
        /// linked diagonally or orthogonally.</returns>
        public static IEnumerable<Vector3Int> GetPointsOnLine(Vector3Int startPos, Vector3Int endPos, bool fillGaps)
        {
            var points = GetPointsOnLine(startPos, endPos);
            if (fillGaps)
            {
                var rise = endPos.y - startPos.y;
                var run = endPos.x - startPos.x;

                if (rise != 0 || run != 0)
                {
                    var extraStart = startPos;
                    var extraEnd = endPos;


                    if (Mathf.Abs(rise) >= Mathf.Abs(run))
                    {
                        // up
                        if (rise > 0)
                        {
                            extraStart.y += 1;
                            extraEnd.y += 1;
                        }
                        // down
                        else // rise < 0
                        {

                            extraStart.y -= 1;
                            extraEnd.y -= 1;
                        }
                    }
                    else // Mathf.Abs(rise) < Mathf.Abs(run)
                    {

                        // right
                        if (run > 0)
                        {
                            extraStart.x += 1;
                            extraEnd.x += 1;
                        }
                        // left
                        else // run < 0
                        {
                            extraStart.x -= 1;
                            extraEnd.x -= 1;
                        }
                    }

                    var extraPoints = GetPointsOnLine(extraStart, extraEnd);
                    extraPoints = extraPoints.Except(new[] { extraEnd });
                    points = points.Union(extraPoints);
                }

            }

            return points;
        }

        /// <summary>
        /// Gets an enumerable for all the cells directly between two points
        /// http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        /// </summary>
        /// <param name="p1">A starting point of a line</param>
        /// <param name="p2">An ending point of a line</param>
        /// <returns>Gets an enumerable for all the cells directly between two points</returns>
        private static IEnumerable<Vector3Int> GetPointsOnLine(Vector3Int p1, Vector3Int p2)
        {
            int x0 = p1.x;
            int y0 = p1.y;
            int x1 = p2.x;
            int y1 = p2.y;

            bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Mathf.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Vector3Int((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        /// <summary>
        /// 해당위치에 타일이 존재하는지 확인하는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <returns>존재여부</returns>
        public static bool GetIsExistTile(Vector3 pos, Tilemap tilemap)
        {
            Vector3Int cellPos = tilemap.WorldToCell(pos);//월드 투 셀 위치로 변환    
            return tilemap.HasTile(cellPos);
            //TileBase target = tilemap.GetTile(cellPos);
            //Debug.Log(target);        
        }

        /// <summary>
        /// 해당위치에 타일이 존재하는지 확인하는 함수
        /// </summary>
        /// <param name="pos">위치</param>
        /// <returns>존재여부</returns>
        public static bool GetIsExistTile(Vector3Int pos, Tilemap tilemap)
        {
            return tilemap.HasTile(pos);
            //TileBase target = tilemap.GetTile(cellPos);
            //Debug.Log(target);        
        }

        /// <summary>
        /// 타일맵 새로고침함수
        /// </summary>
        public static void RefreshTileMap(Tilemap tilemap)
        {
            tilemap.RefreshAllTiles();
        }

        /// <summary>
        /// 타일맵 새로고침함수
        /// </summary>
        /// <param name="pos">위치</param>
        public static void RefreshTileMap(Vector3Int pos, Tilemap tilemap)
        {
            tilemap.RefreshTile(pos);
        }

        /// <summary>
        /// 타일맵 리셋시키는 함수
        /// </summary>
        public static void ResetTilemap(Tilemap tilemap)
        {
            tilemap.ClearAllTiles();
        }



        //무브모듈//20220601 옳김
        //움직임에 관련해서 모든걸 여기서 관할함    
        //물리이용하는건 여기서 안다룬다

        //x좌표를 움직인다. 
        //대입할때 + -로 오른족 왼쪽으로 움직이게 할수 있다,
        public static void MoveXPos(Transform target, float speed)
        {
            target.position += Vector3.right * speed;
            target.Translate(Vector3.right * speed);
        }
        public static void MoveXPos(Transform target, float speed, float time)
        {
            //target.position += Vector3.right * speed * time;
            target.Translate(Vector3.right * speed * time);
        }

        //y좌표를 움직인다. 
        //대입할때 + -로 위 아래로 움직이게 할수 있다,
        public static void MoveYPos(Transform target, float speed)
        {
            //target.position += Vector3.up * speed;
            target.Translate(Vector3.up * speed);
        }
        public static void MoveYPos(Transform target, float speed, float time)
        {
            //target.position += Vector3.up * speed * time;
            target.Translate(Vector3.up * speed * time);
        }

        //z좌표를 움직인다. 
        //대입할때 + -로 forward, backword로 움직이게 할수 있다,
        public static void MoveZPos(Transform target, float speed)
        {
            //target.position += Vector3.forward * speed;
            target.Translate(Vector3.forward * speed);
        }
        public static void MoveZPos(Transform target, float speed, float time)
        {
            target.position += Vector3.forward * speed * time;
            target.Translate(Vector3.forward * speed * time);
        }

        //XY좌표를 움직이게 한다.
        //XY를 같이 움직이게할려면 이걸로 하는게 좋다
        public static void MoveXYPos(Transform target, float Xspeed, float Yspeed)
        {
            target.SetPositionAndRotation(target.position += new Vector3(Xspeed, Yspeed, 0), Quaternion.identity);
        }

        public static void MoveXYPos(Transform target, float Xspeed, float Yspeed, float time)
        {
            target.SetPositionAndRotation(target.position += new Vector3(Xspeed * time, Yspeed * time, 0), Quaternion.identity);
        }


        //XYZ좌료를 움직이게 한다.
        //XYZ를 같이 움직이게할려면 이걸로 하는게 좋다
        public static void MoveXYZPos(Transform target, float Xspeed, float Yspeed, float Zspeed)
        {
            target.SetPositionAndRotation(target.position += new Vector3(Xspeed, Yspeed, Zspeed), Quaternion.identity);
        }
        public static void MoveXYZPos(Transform target, float Xspeed, float Yspeed, float Zspeed, float time)
        {
            target.SetPositionAndRotation(target.position += new Vector3(Xspeed * time, Yspeed * time, Zspeed * time), Quaternion.identity);
        }

        //20190823 새로추가한 함수
        //월드트랜스폼 무브
        public static void MoveLerpXYPos(Transform target, float XPos, float YPos, float moveSpeed)
        {
            //target.position = Vector3.Slerp(target.position, target.position += new Vector3(XPos, YPos, 0), moveSpeed * Time.deltaTime);
            target.position = Vector2.Lerp(target.position, target.position += new Vector3(XPos, YPos, 0), moveSpeed * Time.deltaTime);//문제없음
        }
        //로컬트랜스폼 무브
        public static void MoveLerpYPos(Transform target, float YPos, float moveSpeed)
        {
            //target.position = Vector3.Slerp(target.position, target.position += new Vector3(XPos, YPos, 0), moveSpeed * Time.deltaTime);
            //target.position = Vector2.Lerp(target.position, target.position += new Vector3(0, YPos, 0), moveSpeed * Time.deltaTime);//문제없음
            target.position = Vector2.Lerp(target.position, target.position += target.up * YPos, moveSpeed * Time.deltaTime);//문제없음
        }
        public static void MoveLerpXPos(Transform target, float XPos, float moveSpeed)
        {
            //target.position = Vector3.Slerp(target.position, target.position += new Vector3(XPos, YPos, 0), moveSpeed * Time.deltaTime);
            //target.position = Vector2.Lerp(target.position, target.position += new Vector3(0, YPos, 0), moveSpeed * Time.deltaTime);//문제없음
            target.position = Vector2.Lerp(target.position, target.position += target.right * XPos, moveSpeed * Time.deltaTime);//문제없음
        }

        public static void MoveLerp(Transform target, Vector3 targetPos, float moveSpeed)
        {
            target.position = Vector2.Lerp(target.position, targetPos, moveSpeed);
        }
        public static Vector2 GetMoveLerp(Vector3 target, Vector3 targetPos, float moveSpeed)
        {
            return Vector2.Lerp(target, targetPos, moveSpeed);
        }

        public static void MoveSmoothDamp(Transform target, Vector3 targetPos, ref Vector2 refSpeed, float moveSpeed)
        {   
            target.position = Vector2.SmoothDamp(target.position, targetPos, ref refSpeed, moveSpeed);
        }



        //회전모듈
        //회전에 관련해서 모든걸 여기서 관할함

        //X축을 기준으로 돌린다.
        public static void RotationX(Transform target, float speed)
        {
            target.Rotate(speed * Time.deltaTime, 0, 0);
        }
        public static void RotationX(Transform target, float speed, float time)
        {
            target.Rotate(speed * time * Time.deltaTime, 0, 0);
        }

        //Y축을 기준으로 돌린다.
        public static void RotationY(Transform target, float speed)
        {
            target.Rotate(0, speed * Time.deltaTime, 0);
        }
        public static void RotationY(Transform target, float speed, float time)
        {
            target.Rotate(0, speed * time * Time.deltaTime, 0);
        }

        //Z축을 기준으로 돌린다.
        public static void RotationZ(Transform target, float speed)
        {
            target.Rotate(0, 0, speed * Time.deltaTime);
        }
        public static void RotationZ(Transform target, float speed, float time)
        {
            target.Rotate(0, 0, speed * time * Time.deltaTime);
        }

        /// <summary>
        /// Slerp로 회전하는 함수
        /// </summary>
        /// <param name="rotateTarget">회전하는 오브젝트</param>
        /// <param name="lookTarget">봐야될 위치</param>
        /// <param name="rotateSpeed">회전속도</param>
        public static void SlerpRotationZ(Transform rotateTarget, Vector3 lookTarget, float rotateSpeed)
        {
            Vector2 targetDir = lookTarget - rotateTarget.position;
            //float newangle = Mathf.Atan2(targetDir.x, targetDir.y) * Mathf.Rad2Deg;
            //if (newangle > 0 && newangle < 180)//정상작동
            //{
            //    newangle = newangle * -1;
            //}
            //else
            //{
            //    newangle = newangle * -1;
            //}
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            //Quaternion quaternion = Quaternion.AngleAxis(newangle - 90, Vector3.forward);
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, quaternion, rotateSpeed * Time.deltaTime);//slerp     
        }

        //20190922//신규 추가 회전
        public static void RotationZTarget(Transform rotateObject, Transform lookTarget, float rotateSpeed)
        {   
            //Quaternion lookRotation = Quaternion.LookRotation(rotateTarget);
            //Debug.Log(lookRotation);
            //Vector3 tmpEuler = Quaternion.RotateTowards(rotateObject.rotation, lookRotation,speed * Time.deltaTime).eulerAngles;
            //Debug.Log(tmpEuler);
            Vector2 targetDir = lookTarget.position - rotateObject.position;
            float mouseAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            float turretAngle = Quaternion.Angle(rotateObject.rotation, lookTarget.rotation);
            //if (mouseAngle > 0 && mouseAngle < 180)//정상작동
            //{
            //    mouseAngle = mouseAngle * -1;
            //}
            //else
            //{
            //    mouseAngle = mouseAngle * -1;
            //}
            //if (mouseAngle < 0)
            //{
            //    turretAngle *= -1;
            //}

            //float newangle3 = Quaternion.Dot(rotateObject.rotation, rotateTarget.rotation);
            //Debug.Log(newangle + "///" + newangle2 + "///" + newangle3);

            //테스트중
            //3가지를 고려해서 짜야함
            //마우스 각도  터렛각도 현재 각도와제일 가까운 값을향한 변수

            Debug.Log(mouseAngle + "///" + turretAngle);
            if ((int)turretAngle == (int)mouseAngle)
            {
                return;
            }
            float firResult = mouseAngle - turretAngle;
            float secResult = turretAngle - mouseAngle;

            if (firResult > secResult)
            {
                RotationZ(rotateObject, rotateSpeed, 1);
            }
            else
            {
                RotationZ(rotateObject, -rotateSpeed, 1);
            }

            //아직봉인




            //rotateObject.rotation = Quaternion.Euler(0, 0, rotateObject.rotation.z + -speed * Time.deltaTime);
            //Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            //rotateObject.rotation = Quaternion.Slerp(rotateObject.rotation, quaternion, speed * Time.deltaTime);//slerp
        }




        //20220914새로처리한 회전
        //slerp < Lerp가 더빠름

        /// <summary>
        /// Slerp로 회전//구면 선형보간
        /// </summary>
        /// <param name="rotateTarget">회전하는 트랜스폼</param>
        /// <param name="lookTarget">봐야할 타겟</param>
        /// <param name="rotateSpeed">회전 속도</param>
        public static void RotateSlerp(Transform rotateTarget, Vector3 lookTarget, float rotateSpeed)
        {
            //Slerp회전
            Vector2 targetDir = lookTarget - rotateTarget.position;
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
            Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, quaternion, rotateSpeed);//slerp     
        }

        /// <summary>
        /// Lerp로 회전//선형보간
        /// </summary>
        /// <param name="rotateTarget">회전하는 트랜스폼</param>
        /// <param name="lookTarget">봐야할 타겟</param>
        /// <param name="rotateSpeed">회전 속도</param>
        public static void RotateLerp(Transform rotateTarget, Vector3 lookTarget, float rotateSpeed)
        {
            //Lerp회전
            Vector2 targetDir = lookTarget - rotateTarget.position;
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
            Quaternion quaternion = Quaternion.AngleAxis(newangle, Vector3.forward);
            rotateTarget.rotation = Quaternion.Lerp(rotateTarget.rotation, quaternion, rotateSpeed);
        }

        /// <summary>
        /// 일정한 속도로 회전
        /// </summary>
        /// <param name="rotateTarget">회전하는 트랜스폼</param>
        /// <param name="lookTarget">봐야할 타겟</param>
        /// <param name="rotateSpeed">회전 속도</param>
        public static void RotateTurret(Transform rotateTarget, Vector3 lookTarget, float rotateSpeed)
        {
            Vector2 targetDir = lookTarget - rotateTarget.position;
            float newangle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
            float zAngle = Mathf.MoveTowardsAngle(rotateTarget.eulerAngles.z, newangle, rotateSpeed);//일정하게 움직임
            rotateTarget.rotation = Quaternion.Euler(0, 0, zAngle);
        }





        ////리지드바디모듈
        public static void RigidBody2DMovePos(Rigidbody2D rb2d, Transform target)
        {
            //rb2d.MovePosition((Vector2)transform.position + (NormalizeVecter2(target.position)) * Time.deltaTime);
        }
        public static void RigidBody2DMovePos(Rigidbody2D rb2d, Transform target, float moveSpeed)
        {
            //rb2d.MovePosition((Vector2)target.position + (NormalizeVecter2(target.position))* moveSpeed * Time.deltaTime);
        }

        //노말라이즈를 해놓고 집어넣기
        public static void RigidBody2DMovePos(Rigidbody2D rb2d, Transform target, Vector2 _direction)
        {
            rb2d.MovePosition((Vector2)target.position + (_direction.normalized * Time.deltaTime));
        }

        public static void RigidBody2DMovePos(Rigidbody2D rb2d, Transform target, Vector2 _direction, float moveSpeed)
        {
            rb2d.MovePosition((Vector2)target.position + (_direction * moveSpeed * Time.deltaTime));
        }

        public static void RigidBody2DMoveRotate(Rigidbody2D rb2d, Transform rotateTarget, Transform lookTarget)
        {
            Vector3 direction = lookTarget.position - rotateTarget.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //rb2d.rotation = angle;
            rb2d.SetRotation(angle);
        }
        public static void RigidBody2DMoveRotate(Rigidbody2D rb2d, float _angle)
        {
            rb2d.rotation = _angle;
        }

        //아직 검증안됨
        public static void RotationObject(Rigidbody2D rb2d)
        {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg + 90;
            rb2d.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        private static float tau = Mathf.PI * 2;
        /// <summary>
        /// Sin웨이브를 주는 
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="frequency"></param>
        /// <returns>값</returns>
        public static float SinWave(float amplitude, float frequency)
        {
            //Time.timeSinceLevelLoad
            return amplitude * Mathf.Sin(tau * Time.time * frequency);
        }

        public static float SinWave(float amplitude, float frequency, int index)
        {
            //Time.timeSinceLevelLoad
            return amplitude * Mathf.Sin(tau * Time.time * frequency + index);
        }

        /// <summary>
        /// 두개의 System.object가 동일한지 체크하는 함수
        /// </summary>
        /// <param name="target1">System.Object</param>
        /// <param name="target2">System.Object</param>
        public static void CheckBothTarget(object target1, object target2)
        {
            //동일한 객체이면 트루로 반환함
            Debug.Log(ReferenceEquals(target1, target2));
        }


        public static IEnumerator<float> WaitAndDestroy(Transform targetObject, float timer)
        {
            yield return Timing.WaitForSeconds(timer);
            DeActiveSetNullParent(targetObject);

            //체크//어차피 오브젝트폴로 작동함
            //DestroyManager.Instance.AddWaitDestoryGameObject(emtpyMagazine.gameObject);
        }

        public enum ColliderCheckType
        {
            Box,
            Circle,

        }

        /// <summary>
        /// 게임오브젝트의 태그가 원하는 태그들중에 있는 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="_interectTags">상호작용 태그들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionTag(GameObject _gameObject, string[] _interectTags)//여러 원하는태그중에서 찾아서 있는지 체크하는 함수 //수정함 20190920//20210511수정
        {
            bool isExist = false;
            //태그[]배열로 했을시interectTag.Lengh
            //리스트로 했을시 interectTag.Count
            for (int i = 0; i < _interectTags.Length; i++)
            {
                if (_gameObject.CompareTag(_interectTags[i]))
                {
                    isExist = true;
                    //return returnTag;
                    break;
                }
            }
            return isExist;
        }

        /// <summary>
        /// 게잉오브젝트의 레이어가 원하는 레이어들중에 있는 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="_interectLayers">상호작용 레이어들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionLayer(GameObject _gameObject, int[] _interectLayers)//원하는 레이어가 여러개일 경우 사용원하는 레이어를 반환해서 있는지 체크//신규제작 20200827//20210511수정
        {
            bool isExist = false;
            //태그[]배열로 했을시interectTag.Lengh
            //리스트로 했을시 interectTag.Count
            for (int i = 0; i < _interectLayers.Length; i++)
            {
                if (_gameObject.layer == _interectLayers[i])
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        /// <summary>
        /// 게잉오브젝트의 레이어가 원하는 레이어들중에 있는 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="_interectLayers">상호작용 레이어들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionLayer(GameObject _gameObject, LayerMask[] _interectLayers)//원하는 레이어가 여러개일 경우 사용원하는 레이어를 반환해서 있는지 체크//신규제작 20200827//20210511수정
        {
            bool isExist = false;
            //태그[]배열로 했을시interectTag.Lengh
            //리스트로 했을시 interectTag.Count
            for (int i = 0; i < _interectLayers.Length; i++)
            {
                if (_gameObject.layer == _interectLayers[i])
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        /// <summary>
        /// 게잉오브젝트의 레이어가 원하는 레이어들중에 있는 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="_interectLayers">상호작용 레이어들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionLayer(GameObject _gameObject, LayerMask _interectLayers)//원하는 레이어가 여러개일 경우 사용원하는 레이어를 반환해서 있는지 체크//신규제작 20200827//20210511수정
        {
            bool isExist = false;
            //태그[]배열로 했을시interectTag.Lengh
            //리스트로 했을시 interectTag.Count
            if (_gameObject.layer == _interectLayers)
            {
                isExist = true;
            }
            return isExist;
        }

        //조건에 맞는가
        //사용법
        //if(FitCondition)

        /// <summary>
        /// 게임오브젝트의 레이어와 태그를 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="useLayerCondition">레이어 체크여부</param>
        /// <param name="interectLayer">체크할 레이어들</param>
        /// <param name="useTagCondition">태그 체크여부</param>
        /// <param name="interectTag">체크할 태그들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionAll(GameObject _gameObject, bool _useLayerCondition, int[] _interectLayer, bool _useTagCondition, string[] _interectTag)
        {
            bool isRight = false;

            //신규 제작 20200827
            //수정20210511

            //레이어체크
            if (_useLayerCondition)
            {
                if (!(isRight = FitConditionLayer(_gameObject, _interectLayer)))
                {
                    return isRight;
                }
            }
            else
            {
                isRight = true;
            }

            //태그 체크
            if (_useTagCondition)
            {
                isRight = FitConditionTag(_gameObject, _interectTag);
            }
            return isRight;
        }

        /// <summary>
        /// 게임오브젝트의 레이어와 태그를 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="useLayerCondition">레이어 체크여부</param>
        /// <param name="interectLayer">체크할 레이어들</param>
        /// <param name="useTagCondition">태그 체크여부</param>
        /// <param name="interectTag">체크할 태그들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionAll(GameObject _gameObject, bool _useLayerCondition, LayerMask[] _interectLayer, bool _useTagCondition, string[] _interectTag)
        {
            bool isRight = false;

            //신규 제작 20200827
            //수정20210511

            //레이어체크
            if (_useLayerCondition)
            {
                if (!(isRight = FitConditionLayer(_gameObject, _interectLayer)))
                {
                    return isRight;
                }
            }
            else
            {
                isRight = true;
            }

            //태그 체크
            if (_useTagCondition)
            {
                isRight = FitConditionTag(_gameObject, _interectTag);
            }
            return isRight;
        }

        /// <summary>
        /// 게임오브젝트의 레이어와 태그를 체크하는 함수
        /// </summary>
        /// <param name="_gameObject">타겟된 게임오브젝트</param>
        /// <param name="useLayerCondition">레이어 체크여부</param>
        /// <param name="interectLayer">체크할 레이어들</param>
        /// <param name="useTagCondition">태그 체크여부</param>
        /// <param name="interectTag">체크할 태그들</param>
        /// <returns>조건에 맞는가?</returns>
        public static bool FitConditionAll(GameObject _gameObject, bool _useLayerCondition, LayerMask _interectLayer, bool _useTagCondition, string[] _interectTag)
        {
            bool isRight = false;

            //신규 제작 20200827
            //수정20210511

            //레이어체크
            if (_useLayerCondition)
            {
                if (!(isRight = FitConditionLayer(_gameObject, _interectLayer)))
                {
                    return isRight;
                }
            }
            else
            {
                isRight = true;
            }

            //태그 체크
            if (_useTagCondition)
            {
                isRight = FitConditionTag(_gameObject, _interectTag);
            }
            return isRight;
        }

        /// <summary>
        /// 궤적포인트(타겟오브젝트)를 가져오는 함수
        /// </summary>
        /// <param name="targetObject">타겟오브젝트</param>
        /// <param name="direction">방향</param>
        /// <param name="power">파워</param>
        /// <param name="time">시간</param>
        /// <returns>궤적 포인트</returns>
        public static Vector2 GetArcPoint(Transform targetObject, Vector2 direction, float power, float time , bool worldSpace)
        {
            //transform.position += velocity * Time.deltaTime * Mathf.Lerp(tempTime, 1, acel.Evaluate(tempTime / 1));
            //tempTime += Time.deltaTime;
            //velocity.x += (gravity.x * mass) * Time.deltaTime;
            //velocity.y -= (gravity.y * mass) * Time.deltaTime;
            //공식최적화필요//최적인듯

            Quaternion targetRotation = worldSpace ? Quaternion.identity : targetObject.rotation;
            Vector2 velocity = targetRotation * direction;
            velocity = (Vector2)targetObject.position + (velocity.normalized * power * time) + 0.5f * Physics2D.gravity * (time * time);
            //Vector2 pos = (Vector2)targetObject.position + (direction.normalized * power * time) + 0.5f * Physics2D.gravity * (time * time);
            return velocity;
        }

        /// <summary>
        /// 궤적포인트(벡터0기준)을 가져오는 함수
        /// </summary>
        /// <param name="direction">방향</param>
        /// <param name="power">파워</param>
        /// <param name="time">시간</param>
        /// <returns>궤적포인트</returns>
        public static Vector2 GetArcPoint(Vector2 direction, float power, float time)
        {
            //transform.position += velocity * Time.deltaTime * Mathf.Lerp(tempTime, 1, acel.Evaluate(tempTime / 1));
            //tempTime += Time.deltaTime;
            //velocity.x += (gravity.x * mass) * Time.deltaTime;
            //velocity.y -= (gravity.y * mass) * Time.deltaTime;
            //공식최적화필요//최적인듯
            Vector2 pos = (direction.normalized * power * time) + 0.5f * Physics2D.gravity * (time * time);
            return pos;
        }

        /// <summary>
        /// 궤적포인트(3포인트)를 가져오는 함수//쓸지는 모름
        /// </summary>
        /// <param name="targetObject">x</param>
        /// <param name="direction"></param>
        /// <param name="power"></param>
        /// <param name="time"></param>
        public static Vector2[] GetArc3Point(Transform targetObject, Vector2 direction, float power, float time)
        {
            var velocity = (Vector2)(targetObject.rotation * (direction.normalized * power));//속도
            var p0 = (Vector2)targetObject.position;//첫번쨰위치
            var p1 = p0 + 0.5f * velocity * time;//중간위치
            var p2 = velocity * time + Physics2D.gravity * time * time * 0.5f;//마지막위치

            Vector2[] temp = { p0,p1,p2 };

            return temp;
        }

        /// <summary>
        /// B=>A 방향을 구함
        /// </summary>
        /// <param name="targetAPos">A 위치</param>
        /// <param name="targetBPos">B 위치</param>
        /// <returns></returns>
        public static Vector2 CalDirection(Vector2 targetAPos, Vector2 targetBPos)
        {
            Vector3 direction = targetAPos - targetBPos;
            float newAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Debug.Log(newAngle);

            ////사분명처리를 위해 라디안사용해야됨
            ////c= 3.14*r
            //float radian = newangle * Mathf.PI / 180; //라디안값//동일값
            float radian = newAngle * Mathf.Deg2Rad; //라디안값//동일값

            //수평속도 = 투사속도 * cos(각도)
            //수직속도 = 투사속도 * sin(각도)
            //Vector2 velocity = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            Vector2 velocity;
            velocity.x = Mathf.Cos(radian);
            velocity.y = Mathf.Sin(radian);

            //수평일시 속도:(-1.0, 0.0)각도:180라디안:3.141593
            //각을 주었을시 속도:(-0.9, -0.4)각도:-153.4669라디안:-2.678504
            //Debug.Log("속도:" + velocity + "각도:" + newAngle + "라디안:" + radian);
            return velocity;
        }

        //20220915//포폴에만 있음
        //배열변경


        public static Vector2[] ConvertVector2Array(this Vector3[] v3)
        {
            return System.Array.ConvertAll<Vector3, Vector2>(v3, GetV3fromV2);
        }

        public static Vector3[] ConvertVector3Array(this Vector2[] v3)
        {
            return System.Array.ConvertAll<Vector2, Vector3>(v3, GetV2fromV3);
        }


        public static Vector3 GetV2fromV3(Vector2 v3)
        {
            return new Vector3(v3.x, v3.y, 0);
        }

        public static Vector2 GetV3fromV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }
    }
}