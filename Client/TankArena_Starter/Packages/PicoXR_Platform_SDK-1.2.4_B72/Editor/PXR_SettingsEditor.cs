/************************************************************************************
 【PXR SDK】
 Copyright 2015-2020 Pico Technology Co., Ltd. All Rights Reserved.

************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.XR.PXR;

namespace Unity.XR.PXR.Editor
{
    [CustomEditor(typeof(PXR_Settings))]
    public class PXR_SettingsEditor : UnityEditor.Editor
    {
        private const string StereoRenderingModeAndroid = "stereoRenderingModeAndroid";
        private const string UseDefaultRenderTexture = "useDefaultRenderTexture";
        private const string EyeRenderTextureResolution = "eyeRenderTextureResolution";
        private const string SystemDisplayFrequency = "systemDisplayFrequency";

        static GUIContent guiStereoRenderingMode = EditorGUIUtility.TrTextContent("Stereo Rendering Mode");
        static GUIContent guiUseDefaultRenderTexture = EditorGUIUtility.TrTextContent("Use Default Render Texture");
        static GUIContent guiEyeRenderTextureResolution = EditorGUIUtility.TrTextContent("Render Texture Resolution");
        static GUIContent guidisplayFrequency = EditorGUIUtility.TrTextContent("Display Refresh Rates");


        private SerializedProperty stereoRenderingModeAndroid;
        private SerializedProperty useDefaultRenderTexture;
        private SerializedProperty eyeRenderTextureResolution;
        private SerializedProperty systemDisplayFrequency;

        void OnEnable()
        {
            if (stereoRenderingModeAndroid == null) 
                stereoRenderingModeAndroid = serializedObject.FindProperty(StereoRenderingModeAndroid);
            if (useDefaultRenderTexture == null) 
                useDefaultRenderTexture = serializedObject.FindProperty(UseDefaultRenderTexture);
            if (eyeRenderTextureResolution == null) 
                eyeRenderTextureResolution = serializedObject.FindProperty(EyeRenderTextureResolution);
            if (systemDisplayFrequency == null)
                systemDisplayFrequency = serializedObject.FindProperty(SystemDisplayFrequency);
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            serializedObject.Update();

            BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.HelpBox("PicoXR settings cannot be changed when the editor is in play mode.", MessageType.Info);
                EditorGUILayout.Space();
            }
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            if (selectedBuildTargetGroup == BuildTargetGroup.Android)
            {
                EditorGUILayout.PropertyField(stereoRenderingModeAndroid, guiStereoRenderingMode);
                EditorGUILayout.PropertyField(useDefaultRenderTexture, guiUseDefaultRenderTexture);
                if (!((PXR_Settings)target).useDefaultRenderTexture)
                {
                    EditorGUILayout.PropertyField(eyeRenderTextureResolution, guiEyeRenderTextureResolution);
                }
                EditorGUILayout.PropertyField(systemDisplayFrequency, guidisplayFrequency);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndBuildTargetSelectionGrouping();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
