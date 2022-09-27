using System.Collections;
using UnityEngine;
using lLCroweTool.Singleton;
using lLCroweTool.Dictionary;

namespace WallCrashGame
{
    public class SampleInputKeySetting : MonoBehaviourSingleton<SampleInputKeySetting>
    {
        //포폴용 샘플코드와 코드스타일 보여주기위한 프로젝트 진행
        //게임은 크게 두종류
        //블럭깨기 조작과 빌딩디펜스 조작
        //



        [System.Serializable] public class KeyBible : CustomDictionary<SampleCustomKeyCodeType, KeyCode> { }

        /// <summary>
        /// 기본용 키값
        /// </summary>
        public KeyBible NormalKeyBible { get; private set; }

       

        /// <summary>
        /// 선택툴바 키들
        /// </summary>
        public SampleCustomKeyCodeType[] chooseToolBarKeys =
        {
            SampleCustomKeyCodeType.ChooseSlot1Key,//1
            SampleCustomKeyCodeType.ChooseSlot2Key,//2
            SampleCustomKeyCodeType.ChooseSlot3Key,//3
            SampleCustomKeyCodeType.ChooseSlot4Key,//4
            SampleCustomKeyCodeType.ChooseSlot5Key,//5
            SampleCustomKeyCodeType.ChooseSlot6Key,//6
        };


        protected override void Awake()
        {
            base.Awake();
            InitPlayerInPutKeySetting();
        }

        /// <summary>
        /// 인풋키코드 초기화
        /// </summary>
        private void InitPlayerInPutKeySetting()
        {
            //--------------------
            //주석설명 관련 내용
            //해당되는 타겟 상태이름
            //상태 설명
            //공통 or 변환
            //추가작업 여부
            //해당되는 모드

            //공통 => 모드에 따라 변경이 없는 상태
            //공통아님 => 모드에 따라 변경이 있는 상태            
            //모드는 2가지 일반, 빌드
            //일반 상태 키세팅
            //추가작업 세팅=>좌컨트롤//현재게임 기능에선 사용안함
            //--------------------
            //추가작업시 좀더 세세히 분류해서 주석처리
            //일반 : 일반모드일시 작동되는 키코드
            //빌드 : 빌드모드일시 작동되는 키코드
            //--------------------

            //기본키 사전레이어
            //총키 갯수 : 
            NormalKeyBible = new KeyBible();

            //이동 키//오브젝트이동을 담당
            //공통
            //추가작업 없음   
            NormalKeyBible.Add(SampleCustomKeyCodeType.UpKey, KeyCode.W);
            NormalKeyBible.Add(SampleCustomKeyCodeType.DownKey, KeyCode.S);
            NormalKeyBible.Add(SampleCustomKeyCodeType.LeftKey, KeyCode.A);
            NormalKeyBible.Add(SampleCustomKeyCodeType.RightKey, KeyCode.D);


            //달리기 키//오브젝트의 속도를 높여줌
            //위와 같음
            NormalKeyBible.Add(SampleCustomKeyCodeType.ShiftLKey, KeyCode.LeftShift);


            //마우스//특정 위치를 찍어줌
            //공통//일반 빌드 RTS모드에 사용됨
            //추가작업 없음
            //일반 : 장비작업1을 작동
            //빌드 : 선택된 건설체를 건설후 끝냄            
            NormalKeyBible.Add(SampleCustomKeyCodeType.MouseLeftButton, KeyCode.Mouse0);       //좌클릭//선택 및 작동
            NormalKeyBible.Add(SampleCustomKeyCodeType.MouseRightButton, KeyCode.Mouse1);      //우클릭//취소


            //기능작동키
            //공통아님//일반에서 사용됨
            //추가작업 없음
            NormalKeyBible.Add(SampleCustomKeyCodeType.SpaceKey, KeyCode.Space);


            //인벤토리 슬롯 및 인벤토리 슬롯사용
            //공통아님//일반에서 사용됨
            //추가작업없음
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot1Key, KeyCode.Alpha1);
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot2Key, KeyCode.Alpha2);
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot3Key, KeyCode.Alpha3);
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot4Key, KeyCode.Alpha4);
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot5Key, KeyCode.Alpha5);
            NormalKeyBible.Add(SampleCustomKeyCodeType.ChooseSlot6Key, KeyCode.Alpha6);


            //플레이어모드, 지휘모드 변경키//모드를 변경해줌
            //공통//일반 빌드 RTS모드에 사용됨.
            //추가작업 없음
            NormalKeyBible.Add(SampleCustomKeyCodeType.BuildingConstructModeKey, KeyCode.B);//빌드모드


            //UI관련 키           
            //공통아님//일반에서 사용됨
            //추가작업없음
            NormalKeyBible.Add(SampleCustomKeyCodeType.MenuKey, KeyCode.Escape);
        }
    }
    //키를 지정해준것
    public enum SampleCustomKeyCodeType
    {
        UpKey,//위
        DownKey,//아래
        LeftKey,//좌
        RightKey,//우
        ShiftLKey,//왼쉬프트

        SpaceKey,//작동

        MouseLeftButton,//장비작동1 마우스좌클릭 
        MouseRightButton,//에임 마우스우클릭

        ChooseSlot1Key,//선택키 1
        ChooseSlot2Key,//선택키 2
        ChooseSlot3Key,//선택키 3
        ChooseSlot4Key,//선택키 4
        ChooseSlot5Key,//선택키 5
        ChooseSlot6Key,//선택키 6

        BuildingConstructModeKey,//건물건축모드 B

        FrameGroupButtonKey,//프레임그룹키 1
        FloorGroupButtonKey,//바닥그룹키 2
        WallGroupButtonKey,//벽그룹키3
        DoorGroupButtonKey,//문그룹키 4
        BuildingGroupButtonKey,//건물그룹키 5

        FixModeKey,//수리모드 Z
        DismantleBuildingModeKey,//건물해체모드 X

        MenuKey,//메뉴&탈출 ESC
    }
}

