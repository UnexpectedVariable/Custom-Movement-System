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
            //m_UXML.CloneTree(root);

            var InputActionAssetProperty = serializedObject.FindProperty("_inputAsset");
            //var MovementActionMapProperty = serializedObject.FindProperty("_movementActionMap");
            root.Add(BuildInputAssetProperty(InputActionAssetProperty, playerController));

            var foldout = new Foldout()
            {
                viewDataKey = "PlayerControllerFullInspectorFoldout",
                text = "Full Inspector"
            };
            InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
            root.Add(foldout);

            return root;
        }

        private VisualElement BuildInputAssetProperty(SerializedProperty inputAsset, PlayerController controller)
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

            /*actionMapsDropdown.RegisterValueChangedCallback(evt =>
            {
                Debug.Log($"{target.name} input action asset dropdown field value changed to: {evt.newValue}!");
            });*/

            root.Add(actionMapsDropdown);

            return root;
        }
    }

#endif
}
