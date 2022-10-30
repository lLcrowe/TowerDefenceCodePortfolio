using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimeModule : MonoBehaviour
{
    //20221022
    //�����ο� ���

    //20221024
    //Live2D AnyPotrait�� ��Ʈ�ѷ��� ��� �����غ��� �ִϸ��̼��� ���Ʒ���� ���� ������ ���� �ִϸ��̼����� �����
    //���� ��ġ�� �°� ������ Ʈ���ΰ��̻��� ȥ���ؼ� ����ϴ� ����� ��� ���ϴ� ��Ʈ�ѷ��� ����� ������ ����.
    //������� ��⸶�� ������ �� �޶���
    //Live2D�� AnyPotrait ���� ���� �ʻ�ȭ�� �ִϸ��̼� ����






    //�ΰ��ӿ��� �۵��Ǵ� ���� ũ��
    //1. ž��
    //1-1. ����ž�� ����
    //1-2. ���̵��ó���������� ž�� ����
    //1-2-1.




    //2. ���̵��
    //


    //�����۾��ϱ�
    //20220720
    //������ ��ƿ��ƼAPI �������Ƿ� Ŀ�����ϱ�
    //���� API�� �����ڵ� �۵������� ����ȭ�� ������ �ȵ���
    //=>�ð� ����� �����ɸ� üũ�Ұ�
    //=> ����üũ�� ���� SkeletonAnimation�� ���ΰ�����


    [System.Serializable]
    public class SpineDirectionAnimInfo
    {
        [SpineAnimation] public string upAnim;
        [SpineAnimation] public string downAnim;
        [SpineAnimation] public string leftAnim;
        [SpineAnimation] public string rightAnim;
    }

    public SpineDirectionAnimInfo headAnimInfo;


    //ȭ���� �߰������� Ȯ���� �ش�������� ���İ�(�ΰ��� �ִ��� ������ ��)�� ���ϴ°�
    //�ͽ� 0.3
    //�����ο� Ʈ���� 5���ε� API�� ������ɷ��� Ȯ���ϱ�
    //API ����üũ
    





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
