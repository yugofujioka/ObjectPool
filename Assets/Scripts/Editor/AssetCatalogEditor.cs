using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


// Object列挙一覧
// T : 列挙体
// U : 継承インターフェース
public class AssetCatalogEditor<T, U> : Editor where T : Object where U : struct {
    /// <summary>
    /// 有効時
    /// </summary>
    void OnEnable() {
        AssetCatalog t = this.target as AssetCatalog;
        if (t != null) {
            bool changed = t.CreateDictionary<U>();
            // MEMO: enumが増減していたら変更されてくる
            if (changed) {
                t.UpdateSerializedList<U>();
                EditorUtility.SetDirty(t);  // MEMO: 明示的に更新されたことを通知
                Debug.Log("Update Catalog : " + t.name);
            }
        }
    }

    /// <summary>
    /// Inspector表示
    /// </summary>
    public override void OnInspectorGUI() {
        AssetCatalog t = this.target as AssetCatalog;

        Dictionary<string, AssetCatalog.SettingParam> paramTable = t.editParamDic;

        GUILayout.Space(5f);

        // Prefab設定
        System.Array enumArray = System.Enum.GetValues(typeof(U));
        int objectCount = enumArray.Length;
        for (int i = 0; i < objectCount; ++i) {
            string key = enumArray.GetValue(i).ToString();
            // MEMO: CreateDictionaryで不足分を追加してくるのでチェックいらない
            //AssetCatalog.SettingParam param = null;
            //if (!paramTable.TryGetValue(key, out param)) {
            //    param = new AssetCatalog.SettingParam(key, null, 1);
            //    paramTable.Add(key, param);
            //}
            //if (param == null) {
            //    param = new AssetCatalog.SettingParam(key, null, 1);
            //    paramTable[key] = param;
            //}
            AssetCatalog.SettingParam param = paramTable[key];
            if (key == "MAX")
                continue;

            GUILayout.BeginHorizontal();
			GUIContent content = new GUIContent(i.ToString("D3") + " " + key, "Asset指定");
            paramTable[key].origin = EditorGUILayout.ObjectField(content, param.origin, typeof(T), false, GUILayout.MinWidth(250f)) as T;
            paramTable[key].genCount = EditorGUILayout.IntField(param.genCount, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
        }

        if (GUI.changed || (objectCount != t.GetCatalogCount())) {
            t.UpdateSerializedList<U>();
            EditorUtility.SetDirty(t);  // MEMO: 明示的に更新されたことを通知
            Debug.Log("Update Catalog : " + t.name);
        }
    }
}
