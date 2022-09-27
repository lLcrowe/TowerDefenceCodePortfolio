using System.Collections;
using UnityEngine;

namespace Assets.NodeMap
{
    public class MapMarkerData : ScriptableObject
    {
        //����̵Ǵ� ���ึĿ������
        public MapMarkerData targetPrecedeMarkerData;

        //ObjectDeScription Data
        //������ �ٿ�����Ʈ�� Ī�� => 20191221
        //������Ʈ�ؼ��� �� �����
        public Sprite objectSprite;//������Ʈ�� ������ �̹���
        [Space]
        public string objectName;//������Ʈ�� �̸�
        //[TextArea]//�ʹ�ũ��
        [Multiline]
        public string objectShortDescription;//������Ʈ�� ª������
        [TextArea]
        public string objectDescription;//������Ʈ�� ����
    }
}