using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.Attributes
{
	public class LayerAttribute : PropertyAttribute
	{
	}

	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			switch (property.propertyType)
			{
				case SerializedPropertyType.String:
					DrawPropertyForString(rect, property, label, GetLayers());
					break;
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label, GetLayers());
					break;
				default:
					EditorGUI.PropertyField(rect, property, label);
					break;
			}

			EditorGUI.EndProperty();
		}

		string[] GetLayers()
		{
			return UnityEditorInternal.InternalEditorUtility.layers;
		}

		static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, string[] layers)
		{
			int index = IndexOf(layers, property.stringValue);
			int newIndex = EditorGUI.Popup(rect, label.text, index, layers);
			string newLayer = layers[newIndex];

			if (!property.stringValue.Equals(newLayer, StringComparison.Ordinal))
			{
				property.stringValue = layers[newIndex];
			}
		}

		static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] layers)
		{
			int index = 0;
			string layerName = LayerMask.LayerToName(property.intValue);
			for (int i = 0; i < layers.Length; i++)
			{
				if (layerName.Equals(layers[i], StringComparison.Ordinal))
				{
					index = i;
					break;
				}
			}

			int newIndex = EditorGUI.Popup(rect, label.text, index, layers);
			string newLayerName = layers[newIndex];
			int newLayerNumber = LayerMask.NameToLayer(newLayerName);

			if (property.intValue != newLayerNumber)
			{
				property.intValue = newLayerNumber;
			}
		}

		static int IndexOf(string[] layers, string layer)
		{
			var index = Array.IndexOf(layers, layer);
			return Mathf.Clamp(index, 0, layers.Length - 1);
		}
	}
  #endif
}
