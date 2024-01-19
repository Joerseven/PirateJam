using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DragandDrop : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Drag And Drop")]
    public static void ShowExample()
    {
        DragandDrop wnd = GetWindow<DragandDrop>();
        wnd.titleContent = new GUIContent("Drag And Drop");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        //Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Cards/DragandDrop.uxml");
        VisualElement labelUXML = visualTree.Instantiate();
        root.Add(labelUXML);

        //Load Stylesheet
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Cards/DragandDrop.uss");

        DragandDropManipulator manipulator = new(rootVisualElement.Q<VisualElement>("object"));
    }
}
