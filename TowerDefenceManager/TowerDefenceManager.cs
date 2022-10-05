using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using lLCroweTool.ObjectPool;
using lLCroweTool.TimerSystem;
using lLCroweTool.ScoreSystem;
using lLCroweTool.Singleton;
using lLCroweTool;
using Assets;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using lLCroweTool.Dictionary;

[RequireComponent(typeof(TimerModule))]
public class TowerDefenceManager : MonoBehaviourSingleton<TowerDefenceManager>
{
    //생명//오브젝트방어
    public UnitObject defenceTargetUnitObject;
    public TextMeshProUGUI lifeTextObject;


    //자원수
    //점수(경험치)=>레벨업포인트
    public int cumulativeScore;//누적되는 점수
    public int score;//점수
    public TextMeshProUGUI scoreTextObject;//점수만 보여줌

    public int metalAmount;
    public TextMeshProUGUI metalTextObject;

    public int curElectricAmount;//현재 전기량
    public int maxElectricAmount;//최대치 전기량
    public TextMeshProUGUI electricTextObject;


    //업글레이드처리
    //메탈인컴    
    public int metalIncomeUpgrade;
    public int upgradePerMetalIncomeAmount;

    //전기최대치
    public int electricSupplyUpgrade;
    public int upgradePerElectricSupplyMaxAmount;

    //업글데이터//시트에서 가져옴//메탈전기 동일하게 처리
    [System.Serializable]
    public class UpgradeInfoDataBible : CustomDictionary<int, UpgradeInfo> { }
    [SerializeField] public UpgradeInfoDataBible upgradeInfoDataBible = new UpgradeInfoDataBible();


    //적군관련
    //웨이브 관련
    //웨이브데이터//시트에서 가져옴
    [System.Serializable]
    public class EnemyWaveInfoDataBible : CustomDictionary<int, EnemyWaveInfo> { }
    [SerializeField] public EnemyWaveInfoDataBible enemyWaveInfoDataBible = new EnemyWaveInfoDataBible();


    public int maxWave;
    public int curWave;
    public int waveCurDuringTime;
    public TextMeshProUGUI waveTextObject;

    //적스폰 위치
    public float height;//거리체크
    public Transform enemySpawnPos;//적군 출몰하는 위치

    [System.Serializable]
    public class EnemyObjectPool : CustomObjectPool<Enemy> { }//적오브젝트폴
    [SerializeField] public EnemyObjectPool normalEnemyObjectPool = new EnemyObjectPool();
    [SerializeField] public EnemyObjectPool middleEnemyObjectPool = new EnemyObjectPool();
    [SerializeField] public EnemyObjectPool highEnemyObjectPool = new EnemyObjectPool();

    //업글관련
    public AssetReferenceAtlasedSprite upgradeMetalIncomeImage;
    public AssetReferenceAtlasedSprite upgradeElectricSupplyImage;
    public Button upgradeMetalButton;
    public Button upgradeElectricButton;


    //점수화면관련
    public ScoreScreen scoreScreen;
    public BuildingPlace[] buildingPlaceArray;

    private TimerModule timerModule;

    protected override void Awake()
    {
        base.Awake();
        timerModule = GetComponent<TimerModule>();
        timerModule.SetTimer(1.0f);
        timerModule.AddUnityEvent(delegate { UpdateManager(); });

        upgradeMetalButton.onClick.AddListener(delegate { UpgradeButtonFunc(UpgradeType.MetalIncome); });
        upgradeElectricButton.onClick.AddListener(delegate { UpgradeButtonFunc(UpgradeType.ElectricSupply); });
    }

    private IEnumerator Start()
    {
        AsyncOperationHandle opHandle;
        opHandle = upgradeMetalIncomeImage.LoadAssetAsync<Sprite>();
        yield return opHandle;
        upgradeMetalButton.image.sprite = opHandle.Result as Sprite;

        opHandle = upgradeElectricSupplyImage.LoadAssetAsync<Sprite>();
        yield return opHandle;
        upgradeElectricButton.image.sprite = opHandle.Result as Sprite;

        TextMeshProUGUI text;
        text = upgradeMetalButton.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = LocalizingManager.Instance.GetLocalLizeText("메탈업글");
        }

        text = upgradeElectricButton.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = LocalizingManager.Instance.GetLocalLizeText("전기업글");
        }


        //case BuildingButtonType.UpgradeMetal:
        //    //툴팁
        //    tempTitle = LocalizingManager.Instance.GetLocalLizeText("메탈업글");
        //    tempContent = LocalizingManager.Instance.GetLocalLizeText("메탈인컴을 올립니다.");
        //    break;
        //case BuildingButtonType.UpgradeEletric:
        //    //툴팁
        //    tempTitle = LocalizingManager.Instance.GetLocalLizeText("전기업글");
        //    tempContent = LocalizingManager.Instance.GetLocalLizeText("전기보급을 늘립니다.");
        //    break;
    }

    /// <summary>
    /// 업그레이드버튼 함수
    /// </summary>
    /// <param name="upgradeType">업그레이드타입</param>
    private void UpgradeButtonFunc(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.MetalIncome:
                //업글
                if (!CheckUpgrade(upgradeType))
                {
                    //못한다는 안내

                    return;
                }

                //작동
                CalUpgrade(upgradeType);

                break;
            case UpgradeType.ElectricSupply:
                //업글
                //체크
                if (!CheckUpgrade(upgradeType))
                {
                    //못한다는 안내

                    return;
                }

                //작동
                CalUpgrade(upgradeType);
                break;
        }
    }

    /// <summary>
    /// 매니저업데이트
    /// </summary>
    private void UpdateManager()
    {
        //웨이브처리//웨이브UI처리
        //적군수처리후 웨이브작동
        if (maxWave < curWave)
        {
            //종료
            ShowScoreWindow();
            return;
        }

        //웨이브처리
        waveTextObject.text = curWave + " / " + maxWave;
        waveCurDuringTime--;
        if (waveCurDuringTime == 0)
        {
            if (enemyWaveInfoDataBible.ContainsKey(curWave))
            {
                EnemyWaveInfo enemyWaveInfo = enemyWaveInfoDataBible[curWave];
                waveCurDuringTime = enemyWaveInfo.waveTime;
                StartCoroutine(StartWave(enemyWaveInfo));
            }
        }

        //인컴 처리
        if (metalIncomeUpgrade != 0)
        {
            int tempIncome = metalIncomeUpgrade * upgradePerMetalIncomeAmount;
            AddResource(ResourceType.Metal, tempIncome);
        }
    }

    /// <summary>
    /// 웨이브시작
    /// </summary>
    private IEnumerator StartWave(EnemyWaveInfo enemyWaveInfo)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
        StartCoroutine(RespawnEnemy(enemyWaveInfo, waitForSeconds));
        yield return waitForSeconds;
    }

    /// <summary>
    /// 리스폰적군
    /// </summary>
    /// <param name="enemyWaveInfo">적 웨이브정보</param>
    /// <param name="waitForSeconds"></param>
    private IEnumerator RespawnEnemy(EnemyWaveInfo enemyWaveInfo, WaitForSeconds waitForSeconds)
    {
        Vector2 spawnPos = enemySpawnPos.position;
        int i;

        //일반위험도적군
        for (i = 0; i < enemyWaveInfo.normalEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.Normal);
            yield return waitForSeconds;
        }

        //중간위험도적군
        for (i = 0; i < enemyWaveInfo.middleEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.Middle);
            yield return waitForSeconds;
        }

        //높은위험도적군
        for (i = 0; i < enemyWaveInfo.highEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.High);
            yield return waitForSeconds;
        }
    }

    /// <summary>
    /// 리스폰적군
    /// </summary>
    /// <param name="randomY">Y축 랜덤값</param>
    /// <param name="spawnPos">스폰될 위치값</param>
    /// <param name="enemyType">적군타입</param>
    private void RespawnEnemy(float randomY, Vector2 spawnPos, EnemyType enemyType)
    {
        float posY = Random.Range(spawnPos.y - randomY, spawnPos.y + randomY);
        Vector2 newPos = new Vector2(spawnPos.x, posY);
        Enemy enemy = null;
        switch (enemyType)
        {
            case EnemyType.Normal:
                enemy = normalEnemyObjectPool.RequestObjectPrefab();
                break;
            case EnemyType.Middle:
                enemy = middleEnemyObjectPool.RequestObjectPrefab();
                break;
            case EnemyType.High:
                enemy = highEnemyObjectPool.RequestObjectPrefab();
                break;
        }
        enemy.gameObject.SetActive(true);
        enemy.transform.SetPositionAndRotation(newPos, Quaternion.identity);
    }

    /// <summary>
    /// 게임시작
    /// </summary>
    public void StartGame()
    {
        ResetGame();
        timerModule.enabled = true;
    }

    /// <summary>
    /// 게임종료
    /// </summary>
    public void EndGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 자원을 추가하는 함수
    /// </summary>
    /// <param name="resourceType">자원타입</param>
    /// <param name="value">자원 값</param>
    public void AddResource(ResourceType resourceType, int value)
    {
        switch (resourceType)
        {
            case ResourceType.Score:
                cumulativeScore += value;
                score += value;
                scoreTextObject.text = score.ToString();
                break;
            case ResourceType.Metal:
                metalAmount += value;
                metalTextObject.text = metalAmount.ToString() + " / " + metalIncomeUpgrade * upgradePerMetalIncomeAmount + "+";
                break;
            case ResourceType.Electric:
                curElectricAmount += value;
                electricTextObject.text = curElectricAmount + " / " + maxElectricAmount;
                break;
        }
    }

    /// <summary>
    /// 타입에 따라 업그레이드를 확인하는 함수(절차1)
    /// </summary>
    /// <param name="upgradeType"></param>
    /// <returns>가능한지 여부</returns>
    public bool CheckUpgrade(UpgradeType upgradeType)
    {
        bool isDone = true;
        switch (upgradeType)
        {
            case UpgradeType.MetalIncome:
                if (upgradeInfoDataBible.ContainsKey(metalIncomeUpgrade))
                {
                    UpgradeInfo upgradeInfo = upgradeInfoDataBible[metalIncomeUpgrade];
                    //점수
                    if (upgradeInfo.needScore > score)
                    {
                        isDone = false;
                    }

                    //메탈
                    if (upgradeInfo.needMetal > metalAmount)
                    {
                        isDone = false;
                    }
                }
                else
                {
                    isDone = false;
                }
                break;
            case UpgradeType.ElectricSupply:
                if (upgradeInfoDataBible.ContainsKey(electricSupplyUpgrade))
                {
                    UpgradeInfo upgradeInfo = upgradeInfoDataBible[electricSupplyUpgrade];
                    //점수
                    if (upgradeInfo.needScore > score)
                    {
                        isDone = false;
                    }

                    //메탈
                    if (upgradeInfo.needMetal > metalAmount)
                    {
                        isDone = false;
                    }
                }
                else
                {
                    isDone = false;
                }
                break;
        }
        return isDone;
    }

    /// <summary>
    /// 타입에 따라 업그레이드하는 함수(절차2)
    /// </summary>
    /// <param name="upgradeType">업그레이드 타입</param>
    public void CalUpgrade(UpgradeType upgradeType)
    {
        UpgradeInfo upgradeInfo = null;
        switch (upgradeType)
        {
            case UpgradeType.MetalIncome:
                upgradeInfo = upgradeInfoDataBible[metalIncomeUpgrade];
                score -= upgradeInfo.needScore;
                metalAmount -= upgradeInfo.needMetal;

                metalIncomeUpgrade++;
                metalTextObject.text = metalAmount.ToString() + " / " + metalIncomeUpgrade * upgradePerMetalIncomeAmount + "+";
                break;
            case UpgradeType.ElectricSupply:
                upgradeInfo = upgradeInfoDataBible[electricSupplyUpgrade];
                score -= upgradeInfo.needScore;
                metalAmount -= upgradeInfo.needMetal;

                electricSupplyUpgrade++;
                curElectricAmount += electricSupplyUpgrade * upgradePerElectricSupplyMaxAmount;
                maxElectricAmount += electricSupplyUpgrade * upgradePerElectricSupplyMaxAmount;
                electricTextObject.text = curElectricAmount + " / " + maxElectricAmount;
                break;
        }
    }

    public void ShowScoreWindow()
    {
        timerModule.enabled = false;

        //점수정산
        //int kill = 0;
        //int damage = 0;
        //int len = buildingPlaceArray.Length;
        //for (int i = 0; i < len; i++)
        //{
        //    BuildingPlace buildingPlace = buildingPlaceArray[i];
        //    if (ReferenceEquals(buildingPlace.targetTurret, null))
        //    {
        //        continue;
        //    }
        //    Turret turret = buildingPlace.targetTurret;

        //    if (!turret.TryGetComponent(out ScoreTarget scoreTarget))
        //    {
        //        continue;
        //    }


        //}
        ScoreManager manager = ScoreManager.Instance;
        int kill = manager.GetTotalKillScore();
        int damage = manager.GetTotalDamageScore();
        scoreScreen.SetText(cumulativeScore, score, kill, damage, curWave);
    }

    public void ResetGame()
    {
        cumulativeScore = 0;
        score = 100;
        scoreTextObject.text = score.ToString();
        metalAmount = 0;
        metalIncomeUpgrade = 0;
        metalTextObject.text = metalAmount.ToString() + " / " + metalIncomeUpgrade * upgradePerMetalIncomeAmount + "+";
        curElectricAmount = 0;
        maxElectricAmount = 0;
        electricSupplyUpgrade = 0;
        electricTextObject.text = curElectricAmount + " / " + maxElectricAmount;
        curWave = 0;
        waveTextObject.text = curWave + " / " + maxWave;
        waveCurDuringTime = 0;
        normalEnemyObjectPool.AllObjectDeActive();
        middleEnemyObjectPool.AllObjectDeActive();
        highEnemyObjectPool.AllObjectDeActive();
    }

    //빌딩처리
    /// <summary>
    /// 빌딩 자원 확인하는 함수(절차1)
    /// </summary>
    /// <param name="buildingInfo">빌딩정보</param>
    /// <returns>자원이 충분한지 여부</returns>
    public bool CheckBuildingResource(BuildingInfo buildingInfo)
    {
        //점수
        if (buildingInfo.score > score)
        {
            return false;
        }

        //메탈
        if (buildingInfo.metal > metalAmount)
        {
            return false;
        }

        //전기
        if (buildingInfo.eletric > curElectricAmount)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 빌딩 자원 계산하는 함수(절차2)
    /// </summary>
    /// <param name="buildingInfo">빌딩정보</param>
    public void CalBuildingResource(BuildingInfo buildingInfo)
    {
        score -= buildingInfo.score;
        metalAmount -= buildingInfo.metal;
        curElectricAmount -= buildingInfo.eletric;
    }

    private void OnDrawGizmos()
    {
        if (ReferenceEquals(enemySpawnPos, null))
        {
            return;
        }
        Vector2 target = new Vector2(enemySpawnPos.position.x, enemySpawnPos.position.y + height);
        Gizmos.DrawWireCube(target, Vector2.one * 0.5f);

        target = new Vector2(enemySpawnPos.position.x, enemySpawnPos.position.y - height);
        Gizmos.DrawWireCube(target, Vector2.one * 0.5f);
    }


    /// <summary>
    /// 적군웨이브 정보
    /// </summary>
    [System.Serializable]
    public class EnemyWaveInfo
    {
        public int normalEnemyAmount;
        public int middleEnemyAmount;
        public int highEnemyAmount;

        public int waveTime;
    }

    /// <summary>
    /// 업그레이드 정보
    /// </summary>
    [System.Serializable]
    public class UpgradeInfo
    {
        public int needScore;
        public int needMetal;
    }

    /// <summary>
    /// 적군타입
    /// </summary>
    public enum EnemyType
    {
        Normal,
        Middle,
        High,
    }

    public enum ResourceType
    {
        Score,
        Metal,
        Electric,
    }

    public enum UpgradeType
    {
        MetalIncome,
        ElectricSupply,
    }

}