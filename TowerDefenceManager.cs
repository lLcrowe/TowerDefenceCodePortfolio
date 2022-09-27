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
    //����//������Ʈ���
    public UnitObject defenceTargetUnitObject;
    public TextMeshProUGUI lifeTextObject;


    //�ڿ���
    //����(����ġ)=>����������Ʈ
    public int cumulativeScore;//�����Ǵ� ����
    public int score;//����
    public TextMeshProUGUI scoreTextObject;//������ ������

    public int metalAmount;
    public TextMeshProUGUI metalTextObject;

    public int curElectricAmount;//���� ���ⷮ
    public int maxElectricAmount;//�ִ�ġ ���ⷮ
    public TextMeshProUGUI electricTextObject;


    //���۷��̵�ó��
    //��Ż����    
    public int metalIncomeUpgrade;
    public int upgradePerMetalIncomeAmount;

    //�����ִ�ġ
    public int electricSupplyUpgrade;
    public int upgradePerElectricSupplyMaxAmount;

    //���۵�����//��Ʈ���� ������//��Ż���� �����ϰ� ó��
    [System.Serializable]
    public class UpgradeInfoDataBible : CustomDictionary<int, UpgradeInfo> { }
    [SerializeField] public UpgradeInfoDataBible upgradeInfoDataBible = new UpgradeInfoDataBible();


    //��������
    //���̺� ����
    //���̺굥����//��Ʈ���� ������
    [System.Serializable]
    public class EnemyWaveInfoDataBible : CustomDictionary<int, EnemyWaveInfo> { }
    [SerializeField] public EnemyWaveInfoDataBible enemyWaveInfoDataBible = new EnemyWaveInfoDataBible();


    public int maxWave;
    public int curWave;
    public int waveCurDuringTime;
    public TextMeshProUGUI waveTextObject;

    //������ ��ġ
    public float height;//�Ÿ�üũ
    public Transform enemySpawnPos;//���� ����ϴ� ��ġ

    [System.Serializable]
    public class EnemyObjectPool : CustomObjectPool<Enemy> { }//��������Ʈ��
    [SerializeField] public EnemyObjectPool normalEnemyObjectPool = new EnemyObjectPool();
    [SerializeField] public EnemyObjectPool middleEnemyObjectPool = new EnemyObjectPool();
    [SerializeField] public EnemyObjectPool highEnemyObjectPool = new EnemyObjectPool();

    //���۰���
    public AssetReferenceAtlasedSprite upgradeMetalIncomeImage;
    public AssetReferenceAtlasedSprite upgradeElectricSupplyImage;
    public Button upgradeMetalButton;
    public Button upgradeElectricButton;


    //����ȭ�����
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
            text.text = LocalizingManager.Instance.GetLocalLizeText("��Ż����");
        }

        text = upgradeElectricButton.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = LocalizingManager.Instance.GetLocalLizeText("�������");
        }


        //case BuildingButtonType.UpgradeMetal:
        //    //����
        //    tempTitle = LocalizingManager.Instance.GetLocalLizeText("��Ż����");
        //    tempContent = LocalizingManager.Instance.GetLocalLizeText("��Ż������ �ø��ϴ�.");
        //    break;
        //case BuildingButtonType.UpgradeEletric:
        //    //����
        //    tempTitle = LocalizingManager.Instance.GetLocalLizeText("�������");
        //    tempContent = LocalizingManager.Instance.GetLocalLizeText("���⺸���� �ø��ϴ�.");
        //    break;
    }

    /// <summary>
    /// ���׷��̵��ư �Լ�
    /// </summary>
    /// <param name="upgradeType">���׷��̵�Ÿ��</param>
    private void UpgradeButtonFunc(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.MetalIncome:
                //����
                if (!CheckUpgrade(upgradeType))
                {
                    //���Ѵٴ� �ȳ�

                    return;
                }

                //�۵�
                CalUpgrade(upgradeType);

                break;
            case UpgradeType.ElectricSupply:
                //����
                //üũ
                if (!CheckUpgrade(upgradeType))
                {
                    //���Ѵٴ� �ȳ�

                    return;
                }

                //�۵�
                CalUpgrade(upgradeType);
                break;
        }
    }

    /// <summary>
    /// �Ŵ���������Ʈ
    /// </summary>
    private void UpdateManager()
    {
        //���̺�ó��//���̺�UIó��
        //������ó���� ���̺��۵�
        if (maxWave < curWave)
        {
            //����
            ShowScoreWindow();
            return;
        }

        //���̺�ó��
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

        //���� ó��
        if (metalIncomeUpgrade != 0)
        {
            int tempIncome = metalIncomeUpgrade * upgradePerMetalIncomeAmount;
            AddResource(ResourceType.Metal, tempIncome);
        }
    }

    /// <summary>
    /// ���̺����
    /// </summary>
    private IEnumerator StartWave(EnemyWaveInfo enemyWaveInfo)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
        StartCoroutine(RespawnEnemy(enemyWaveInfo, waitForSeconds));
        yield return waitForSeconds;
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="enemyWaveInfo">�� ���̺�����</param>
    /// <param name="waitForSeconds"></param>
    private IEnumerator RespawnEnemy(EnemyWaveInfo enemyWaveInfo, WaitForSeconds waitForSeconds)
    {
        Vector2 spawnPos = enemySpawnPos.position;
        int i;

        //�Ϲ����赵����
        for (i = 0; i < enemyWaveInfo.normalEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.Normal);
            yield return waitForSeconds;
        }

        //�߰����赵����
        for (i = 0; i < enemyWaveInfo.middleEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.Middle);
            yield return waitForSeconds;
        }

        //�������赵����
        for (i = 0; i < enemyWaveInfo.highEnemyAmount; i++)
        {
            RespawnEnemy(height, spawnPos, EnemyType.High);
            yield return waitForSeconds;
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="randomY">Y�� ������</param>
    /// <param name="spawnPos">������ ��ġ��</param>
    /// <param name="enemyType">����Ÿ��</param>
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
    /// ���ӽ���
    /// </summary>
    public void StartGame()
    {
        ResetGame();
        timerModule.enabled = true;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void EndGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// �ڿ��� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="resourceType">�ڿ�Ÿ��</param>
    /// <param name="value">�ڿ� ��</param>
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
    /// Ÿ�Կ� ���� ���׷��̵带 Ȯ���ϴ� �Լ�(����1)
    /// </summary>
    /// <param name="upgradeType"></param>
    /// <returns>�������� ����</returns>
    public bool CheckUpgrade(UpgradeType upgradeType)
    {
        bool isDone = true;
        switch (upgradeType)
        {
            case UpgradeType.MetalIncome:
                if (upgradeInfoDataBible.ContainsKey(metalIncomeUpgrade))
                {
                    UpgradeInfo upgradeInfo = upgradeInfoDataBible[metalIncomeUpgrade];
                    //����
                    if (upgradeInfo.needScore > score)
                    {
                        isDone = false;
                    }

                    //��Ż
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
                    //����
                    if (upgradeInfo.needScore > score)
                    {
                        isDone = false;
                    }

                    //��Ż
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
    /// Ÿ�Կ� ���� ���׷��̵��ϴ� �Լ�(����2)
    /// </summary>
    /// <param name="upgradeType">���׷��̵� Ÿ��</param>
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

        //��������
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

    //����ó��
    /// <summary>
    /// ���� �ڿ� Ȯ���ϴ� �Լ�(����1)
    /// </summary>
    /// <param name="buildingInfo">��������</param>
    /// <returns>�ڿ��� ������� ����</returns>
    public bool CheckBuildingResource(BuildingInfo buildingInfo)
    {
        //����
        if (buildingInfo.score > score)
        {
            return false;
        }

        //��Ż
        if (buildingInfo.metal > metalAmount)
        {
            return false;
        }

        //����
        if (buildingInfo.eletric > curElectricAmount)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// ���� �ڿ� ����ϴ� �Լ�(����2)
    /// </summary>
    /// <param name="buildingInfo">��������</param>
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
    /// �������̺� ����
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
    /// ���׷��̵� ����
    /// </summary>
    [System.Serializable]
    public class UpgradeInfo
    {
        public int needScore;
        public int needMetal;
    }

    /// <summary>
    /// ����Ÿ��
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