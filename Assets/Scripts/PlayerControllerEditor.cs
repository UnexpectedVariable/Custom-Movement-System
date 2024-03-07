using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
#if true

    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : Editor
    {
        public VisualTreeAsset m_UXML;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var playerController = (PlayerController)target;

#if UNITY_EDITOR
            if(m_UXML) m_UXML.CloneTree(root);
            else
            {
                var uxmlGuid = AssetDatabase.FindAssets("PlayerControllerEditor", new string[] { "Assets/ToolingUI/UXML" });
                m_UXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(uxmlGuid[0]));
                m_UXML.CloneTree(root);
            }
#endif

            var mcListView = root.Q<MultiColumnListView>();
            mcListView.viewDataKey = $"{target.name} PlayerControllerActionMaps_MultiColumnListView";
            mcListView.itemsSource = playerController.PlayerInputBinders;


            var InputActionAssetProperty = serializedObject.FindProperty("_inputAsset");
            //var MovementActionMapProperty = serializedObject.FindProperty("_movementActionMap");
            BuildInputAssetProperty(mcListView, InputActionAssetProperty, playerController);

            var foldout = new Foldout()
            {
                viewDataKey = "PlayerControllerFullInspectorFoldout",
                text = "Full Inspector"
            };
            InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
            root.Add(foldout);

            return root;
        }

        private void BuildInputAssetProperty(MultiColumnListView listView, SerializedProperty inputAsset, PlayerController controller)
        {
            if (inputAsset.objectReferenceValue == null) return;

            var serializedInputAsset = new SerializedObject(inputAsset.objectReferenceValue);
            var inputAssetObject = inputAsset.objectReferenceValue as InputActionAsset;

            var cols = listView.columns;
            cols["input-handler"].makeCell = () => new Label();
            cols["action-map"].makeCell = () => new DropdownField();
            cols["automatic-validation"].makeCell = () => new Toggle();

            cols["input-handler"].bindCell = (VisualElement element, int index) =>
            {
                var label = element as Label;
                label.viewDataKey = $"{target.name} input-handler-label row.{index}";
                label.text = controller.PlayerInputBinders[index].name;
            };

            cols["action-map"].bindCell = (VisualElement element, int index) =>
            {
                var dropdown = element as DropdownField;
                dropdown.viewDataKey = $"{target.name} action-map-dropdown row.{index}";
                dropdown.choices = inputAssetObject.actionMaps.Select(x => x.name).ToList();
                dropdown.RegisterValueChangedCallback(evt =>
                {
                    controller.PlayerInputBinders[index].ActionMap = inputAssetObject.FindActionMap(evt.newValue);
                    controller.PlayerMovementController.ValidateInputs();
                });
            };

            cols["automatic-validation"].bindCell = (VisualElement element, int index) =>
            {
                var toggle = element as Toggle;
                toggle.viewDataKey = $"{target.name} automatic-validation row.{index}";
                toggle.SetValueWithoutNotify(controller.PlayerInputBinders[index].AutomaticValidation);
                toggle.RegisterValueChangedCallback(evt =>
                {
                    controller.PlayerInputBinders[index].AutomaticValidation = evt.newValue;
                });
            };
        }
        
        /*private VisualElement BuildInputAssetProperty(SerializedProperty inputAsset, PlayerController controller)
        {
            var root = new VisualElement();

            if (inputAsset.objectReferenceValue == null) return root;

            var serializedInputAsset = new SerializedObject(inputAsset.objectReferenceValue);
            var inputAssetObject = inputAsset.objectReferenceValue as InputActionAsset;

            var actionMapsDropdown = new DropdownField("Action Maps", inputAssetObject.actionMaps.Select(x => x.name).ToList(), 0, null, null);
            actionMapsDropdown.viewDataKey = "PlayerControllerActionMapsDropdown";
            actionMapsDropdown.RegisterValueChangedCallback(evt =>
            {
                controller.MovementActionMapName = evt.newValue;
            });

            root.Add(actionMapsDropdown);

            return root;
        }*/
    }

#endif
        }
