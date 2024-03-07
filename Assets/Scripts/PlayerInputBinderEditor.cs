﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Assets.Scripts
{
    [CustomEditor(typeof(PlayerInputBinder))]
    public class PlayerInputBinderEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var inputBinder = (PlayerInputBinder)target;

            //var inputActionAssetProperty = serializedObject.FindProperty("_actionAsset");
            //PropertyField inputActionAssetPropertyField = BuildInputActionAssetPropertyField(inputActionAssetProperty);

            //var actionMapsDropdown = new DropdownField("Action Maps");

            var automaticValidationPropertyField = new PropertyField(serializedObject.FindProperty("AutomaticValidation"), "Automatic Validation");

            //root.Add(inputActionAssetPropertyField);
            //root.Add(actionMapsDropdown);
            root.Add(automaticValidationPropertyField);

            //if (inputActionAssetProperty.objectReferenceValue == null) return root;

            //var inputAssetObject = inputActionAssetProperty.objectReferenceValue as InputActionAsset;
            //BuildActionMapsDropdown(inputBinder, actionMapsDropdown, inputAssetObject);

            return root;
        }

        private PropertyField BuildInputActionAssetPropertyField(SerializedProperty inputActionAssetProperty)
        {
            var inputActionAssetPropertyField = new PropertyField(inputActionAssetProperty);
            inputActionAssetPropertyField.RegisterValueChangeCallback(OnInputActionAssetValueChanged);
            return inputActionAssetPropertyField;
        }

        private void BuildActionMapsDropdown(PlayerInputBinder inputBinder, DropdownField actionMapsDropdown, InputActionAsset inputAssetObject)
        {
            actionMapsDropdown.choices = inputAssetObject.actionMaps.Select(x => x.name).ToList();
            actionMapsDropdown.viewDataKey = $"{target.name} PlayerInputBinderActionMapsDropdown";
            actionMapsDropdown.RegisterValueChangedCallback(evt =>
            {
                inputBinder.ActionMap = inputAssetObject.FindActionMap(evt.newValue);
            });
        }

        private void OnInputActionAssetValueChanged(SerializedPropertyChangeEvent evt)
        {
            var actionMapsDropdown = (evt.target as VisualElement).parent.Q<DropdownField>();

            if(evt.changedProperty.objectReferenceValue == null)
            {
                if (actionMapsDropdown.choices.Count == 0) return;
                actionMapsDropdown.choices.Clear();
                return;
            }

            var inputAssetObject = evt.changedProperty.objectReferenceValue as InputActionAsset;
            actionMapsDropdown.choices = inputAssetObject.actionMaps.Select(x => x.name).ToList();
            return;
        }
    }
}
