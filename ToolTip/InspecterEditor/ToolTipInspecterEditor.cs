using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using lLCroweTool.ToolTipSystem;

namespace lLCroweTool.QC.EditorOnly
{
    [CustomEditor(typeof(ToolTipUiView), true)]
    [CanEditMultipleObjects]
    public class ToolTipInspecterEditor : Editor
    {
        ToolTipUiView module_Base;

        private void OnEnable()
        {
            module_Base = (ToolTipUiView)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (0 == module_Base.transform.childCount)
            {
                if (GUILayout.Button("-=����=-"))
                {
                    CreateObjectInitSetting(module_Base);
                }
                EditorGUILayout.HelpBox("������ư�� ���� �ʱ�ȭ���ּ���.", MessageType.Warning);
            }
            else if (!module_Base.TryGetComponent(out HorizontalOrVerticalLayoutGroup _horizontalOrVerticalLayoutGroup))
            {
                bool isClick = false;
                if (GUILayout.Button("-=Vertical������Ʈ�߰�=-"))
                {
                    _horizontalOrVerticalLayoutGroup = module_Base.gameObject.AddComponent<VerticalLayoutGroup>();
                    isClick = true;
                }

                if (GUILayout.Button("-=Horizontal������Ʈ�߰�=-"))
                {
                    _horizontalOrVerticalLayoutGroup = module_Base.gameObject.AddComponent<HorizontalLayoutGroup>();
                    isClick = true;
                }

                if (isClick)
                {
                    //ver or hor ������ ����
                    _horizontalOrVerticalLayoutGroup.childControlWidth = true;
                    _horizontalOrVerticalLayoutGroup.childControlHeight = true;
                    _horizontalOrVerticalLayoutGroup.childForceExpandWidth = false;
                    _horizontalOrVerticalLayoutGroup.childForceExpandHeight = false;
                    _horizontalOrVerticalLayoutGroup.padding.left = 10;
                    _horizontalOrVerticalLayoutGroup.padding.right = 10;
                    _horizontalOrVerticalLayoutGroup.padding.top = 10;
                    _horizontalOrVerticalLayoutGroup.padding.bottom = 10;
                    _horizontalOrVerticalLayoutGroup.spacing = 10;

                    isClick = false;
                }

                EditorGUILayout.HelpBox("VerticalLayoutGroup or HorizontalLayoutGroup \n ���� �ϳ��� ������Ʈ�� �����ؼ� �������ּ���.", MessageType.Warning);

            }
            else
            {
                if (GUILayout.Button("-=���¹�ư=-"))
                {
                    //�ڽĵ� �ʱ�ȭ(����-)
                    int count = module_Base.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        //�迭�߿� ������ �迭���� ���� �����ع����� ������
                        //DestroyImmediate�� ���ӿ�����Ʈ�� ��������
                        DestroyImmediate(module_Base.transform.GetChild(0).gameObject);
                    }

                    DestroyImmediate(module_Base.gameObject.GetComponent<Image>());
                    //DestroyImmediate(module_Base.gameObject.GetComponent<UIView>());
                    DestroyImmediate(module_Base.gameObject.GetComponent<ContentSizeFitter>());
                    if (module_Base.TryGetComponent(out VerticalLayoutGroup _verticalLayout))
                    {
                        DestroyImmediate(_verticalLayout);
                    }
                    if (module_Base.TryGetComponent(out HorizontalLayoutGroup _horizontalLayout))
                    {
                        DestroyImmediate(_horizontalLayout);
                    }
                    if (module_Base.TryGetComponent(out LayoutElement _layout))
                    {
                        DestroyImmediate(_layout);
                    }
                    //module_Base.toolTipImage = null;
                    //module_Base.toolTipText = null;
                    //module_Base.toolTipView = null;
                    //module_Base.layoutElement = null;
                    module_Base.SendMessage("Reset");
                }
                EditorGUILayout.HelpBox("Padding, Spacing �� �������ּ��� \n ControlChildSize�� width = true, height = true �� ���ִ��� Ȯ�����ּ��� ", MessageType.Info);
            }
        }
        private void CreateObjectInitSetting(ToolTipUiView _target)
        {
            _target.gameObject.name = "���� �ڽ�_";
            GameObject go;

            if (_target.TryGetComponent(out RectTransform _rect))
            {
                _rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 15, 0);
                _rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 15, 0);
                _rect.position = Vector3.zero;
                _rect.pivot = Vector2.zero;
            }
            Image image = _target.gameObject.AddComponent<Image>();//����
            image.color = new Color(0.5f, 0.5f, 0.5f);
            //_target.toolTipView = _target.gameObject.AddComponent<UIView>();//UIView
            //_target.toolTipView.UseCustomStartAnchoredPosition = false;

            //����������� ���� ����
            ContentSizeFitter contentSizeFitter = _target.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            //���̾ƿ�
            _target.layoutElement = _target.gameObject.AddComponent<LayoutElement>();
            _target.layoutElement.preferredWidth = _target.charLimit;


            //���������� �̹��� ����
            go = new GameObject();
            go.name = "���� ������ �̹���";
            go.transform.parent = _target.transform;
            _target.toolTipImage = go.AddComponent<Image>();
            _target.toolTipImage.raycastTarget = false;
            go.transform.position = _target.transform.position;
            go.transform.localScale = Vector3.one;

            //���� �ؽ�Ʈ ����
            go = new GameObject();
            go.name = "���� �ؽ�Ʈ";
            go.transform.parent = _target.transform;
            _target.toolTipText = go.AddComponent<TextMeshProUGUI>();
            _target.toolTipText.text = "Text ToolTip..";
            _target.toolTipText.color = new Color(0, 0, 0);
            //toolTipText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 15, 500);
            //toolTipText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 15, 100);
            _target.toolTipText.rectTransform.position = Vector3.zero;
            //toolTipText.rectTransform.rect.Set(4, 4, 0, 0);
            _target.toolTipText.enableWordWrapping = true;
            _target.toolTipText.overflowMode = TextOverflowModes.Overflow;
            _target.toolTipText.alignment = TextAlignmentOptions.BottomLeft;

            go.transform.position = _target.transform.position;
            go.transform.localScale = Vector3.one;
        }
    }
}