using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    //중앙방식인 로그매니저
    //나중에 아카식레코드랑 같이 사용할예정
    //아직좀더 생각해볼것
    private static LogManager instance;
    public static LogManager Instance
    {
        get
        {
            if (ReferenceEquals(instance, null))
            {
                instance = FindObjectOfType<LogManager>();
                if (ReferenceEquals(instance, null))
                {
                    GameObject go = new GameObject();
                    go.name = "LogManager";
                    instance = go.AddComponent<LogManager>();
                }
            }
            return instance;
        }
    }

    bool useLogSystem = false;

    public void useLog(object target)
    {
        if (useLogSystem)
        {
            
        }
    }
}
