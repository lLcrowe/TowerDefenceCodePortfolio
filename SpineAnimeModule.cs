using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimeModule : MonoBehaviour
{
    //20221022
    //스파인용 모듈

    //20221024
    //Live2D AnyPotrait의 컨트롤러를 계속 생각해보니 애니메이션을 위아래옆등에 대한 설정을 각각 애니메이션으로 만들어
    //각각 위치에 맞게 설정후 트랙두개이상을 혼합해서 사용하는 방식을 써야 원하는 컨트롤러를 만들수 있을거 같음.
    //만들려는 모듈마다 종류가 좀 달라보임
    //Live2D와 AnyPotrait 종류 같은 초상화쪽 애니메이션 종류






    //인게임에서 작동되는 종류 크게
    //1. 탑뷰
    //1-1. 완전탑뷰 형식
    //1-2. 사이드뷰처럼보이지만 탑뷰 형식
    //1-2-1.




    //2. 사이드뷰
    //


    //같이작업하기
    //20220720
    //스파인 유틸리티API 내마음되로 커스텀하기
    //기존 API가 기존코드 작동구조와 최적화가 마음에 안들음
    //=>시간 상당히 오래걸림 체크할것
    //=> 구조체크가 먼저 SkeletonAnimation이 메인같은데


    [System.Serializable]
    public class SpineDirectionAnimInfo
    {
        [SpineAnimation] public string upAnim;
        [SpineAnimation] public string downAnim;
        [SpineAnimation] public string leftAnim;
        [SpineAnimation] public string rightAnim;
    }

    public SpineDirectionAnimInfo headAnimInfo;


    //화면의 중간지점을 확인후 해당방향으로 알파값(두개의 애님을 블랜드한 값)을 정하는게
    //믹스 0.3
    //스파인엔 트랙이 5개인데 API는 몇까지될려나 확인하기
    //API 사용법체크
    





    public SkeletonDataAsset animData;
    public SkeletonAnimation anim;


    

    [SpineAnimation]
    public string idleAnim;

    [SpineEvent]
    public string spineEvent;


    public void Test()
    {
        anim.AnimationState.SetAnimation(0, idleAnim, true);
    }
}
