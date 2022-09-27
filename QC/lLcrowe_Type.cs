using UnityEngine;
using System.Collections;


//모든타입을 여기서 정의한다음 
//다른 곳에서 불려와서 쓴다.
//20191114
//장단점이 있음
//각자의 오브젝트에 집어넣으면 나중에 해당 오브젝트에 해당 타입이 있는지 해당클래스가서 확인해야 하지만
//현재 방식으로 한 클래스에 다집어넣으면 여기서 어떤 타입이 있는지 체크해줄수 있음
//단지 문제점은 현클래스를 통해서 작업을 해야하니 안좋긴함
//좀더 생각해볼것
//일단은 명칭변경부터 lLcrowe_Type ->> TypeCataCategory ==> TypeCAT (CAT : 카테고리 약자)

//월드타입 
//일단 대미지로직에서는 빼고 나중에 집어넣자
//분별쪽타입
public enum WorldObjectType
{
    Unit,//유닛
    Building,//건물
    Object,//오브젝트//나무 등등
    Resource,//자원
    Item,//아이템
    Vehicle,//차량 등등 탈것   
    Etc//기타 등등
}

//유닛타입
public enum UnitType
{
    None,//모든걸 무시하는것//알수없는
    Bio,//생체유닛
    Mech,//기계유닛
    Complex,//복합
    Unknown//에너지//미확인
}

//유닛사이즈타입
//public enum UnitSizeType
//{
//    Small,//소형
//    Medium,//중형
//    Bigger,//대형
//    ExtraLarge//초대형 넣을까..
//}

//차량타입(유닛)탈것
//public enum UnitVehicleType
//{
//    Car,//차량
//    APT,//장갑차량
//    Tank,//탱크
//    Walker,//워커
//    SpaceShip,//우주선
//}

//유닛팀타입
public enum UnitTeamType
{
    Player,//플레이어
    Ally,//아군
    Enemy,//적
    NPC,//NPC
    Unknown,//알수없는
    Neutrality,//중립
    Nothing,//공격할수 없게 만들게 하는 타입//해당배열을 유지하기 위해 집어넣는 것
    //Resource,//자원->월드오브젝트타입으로 변경됫음
    
    //환경
}

//유닛컨디션타입
public enum UnitConditionType
{
    //활성화비활성화
    //드론 차량 포탑 시설물등등
    Active,//작동되는 상태
    DeActive,
    //건축물
    normalcy,//정상상태
    Destruction,//파괴상태
    //유닛        
    Dead,//죽었는지
    Live,//살았는지
    //무력화
}
//탈것컨디션타입
public enum VehicleConditionType { }

//빌딩컨디션타입
public enum BuildingConditionType { }

//무기타입//없어도 될꺼 같다
//public enum WeaponType
//{
//    Light,
//    Normal,
//    Heavy,
//}

//대미지타입
//공격타입에 들어갈 대미지타입
//대미지타입 그림을 만들어볼것
public enum DamageType
{


    //      :0
    //+     :5
    //++    :10
    //+++   :20


    Normal,//일반적인공격
    Antimatalial,//대물(아머++//쉴드+)
    Plasma,//플라즈마(아머+//쉴드++)

    
    Explosion,//폭발(생체+//물질+//복합++)
    Laser,//레이저(생체++//물질+//복합+)
    Beam,//빔(생체+//물질++//복합+)
    Unknown,//알수 없는(생체++//물질++//복합++//언노운++)


    //기타 공격형식

    //광산용   
    Mine,

    //(체력회복용)
    Repair,//건축물과 탑승물 수리    //현재만든상태상 -값으로 집어넣야지 회복됨
    Healing,//힐                     //현재만든상태상 -값으로 집어넣야지 회복됨
    ShildCharge,//쉴드 회복기            //현재만든상태상 -값으로 집어넣야지 회복됨

    //불관련
    Fire,//불(생체++//물질+//복합+)    
    FireSuppression,//불진압
}

//투사체타입
public enum ProjectileType
{        
    Bullet,//블렛타입
    Laser,//레이저타입
    LaserBeam,//레이저입자빔
    Rocket,//로켓타입
    Missile,//미사일타입       
}

//아머타입
public enum ArmorType
{
    //None,//없음
    Light,//경장갑
    Normal,//장갑
    Heavy,//중장갑
}

//쉴드타입
public enum ShildType
{        
   // None,//없음
    Light,//가벼운 방어구
    Normal,//기본보호막
    Heavy,//일정최대수치만달게하는보호막
    //Special,
}

////스킬타입
//public enum SkillType
//{
//    Active, //액티브스킬
//    Passive //패시브스킬
//}

////버프타입
//public enum BuffType
//{
//    Timer,//타이머 반복
//    Apply,//적용
//}

//플레이어의 마우스와 카메라등을 설정해주는 모드
public enum PlayMode
{
    NormalMode,//노말모드//평상시모드        //운전모드로 변경
    //DriveMode,//운전모드
    CommandMode,//지휘모드
    BuildMode,//빌드모드
    SkillTreeMode,//스킬트리를 켯을시 설정
}

//플레이모드에 따른 오버레이상태
public enum WorldObjectLayer
{
    Space,
    NonSpace,//애매한태그-->
    Land,
    InSpaceShip,
    OutSpaceShip
}

//아이템타입
//public enum ItemType
//{
//    Matarial,
//    Weapon,        
//    Armor,//방어구아이템
//    Ammo,//탄창아이템
//    //소모아이템
//}

/// <summary>
/// 말단기능성건축물오브젝트에 필요한 인풋아웃풋타입 이넘
/// IN_OneWay : 입력, 흡입 /
/// OUT_OneWay : 출력, 배출
/// </summary>
public enum IOType
{
    IN_OneWay,//단방향 인풋
    OUT_OneWay,//단방향 아웃풋    
}



