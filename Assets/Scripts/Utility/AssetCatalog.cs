using UnityEngine;
using System.Collections.Generic;


// Asset列挙
public class AssetCatalog : ScriptableObject {
    // シリアライズ用クラス
    [System.Serializable]
    public sealed class SettingParam {
        public SettingParam(string key, Object origin, int genCount) {
            this.key = key;
            this.origin = origin;
            this.genCount = genCount;
        }

        public string key = null;      // ID名
        public Object origin = null;   // オリジナルObject
        public int genCount = 1;  // 生成数
    }

    // 保存用データ
    [SerializeField]
    private SettingParam[] serializedParams = new SettingParam[0];


    // Objectの取得
    // type : 種類
    public Object GetObject(int type) {
        return this.serializedParams[type].origin;
    }
    // Objectの生成数取得
    // type : 種類
    public int GetGenerateCount(int type) {
        return this.serializedParams[type].genCount;
    }
    // 種類数の取得
    public int GetCatalogCount() {
        return this.serializedParams.Length;
    }


#if UNITY_EDITOR
    // 編集用データ
    [System.NonSerialized]
    public Dictionary<string, SettingParam> editParamDic
                                   = new Dictionary<string, SettingParam>();

    // 編集用Dictionaryの作成
    public bool CreateDictionary<U>() where U : struct {
        System.Array enumArray = System.Enum.GetValues(typeof(U));
        int objectCount = enumArray.Length;
        this.editParamDic.Clear();
        
        List<SettingParam> serializedList
                             = new List<SettingParam>(this.serializedParams);

        bool changed = false;
        // シリアライズデータからEDITOR用のテーブルを作成
        for (int i = 0; i < serializedList.Count; ++i) {
            SettingParam p = serializedList[i];
            bool find = false;
            for (int j = 0; j < objectCount; ++j) {
                if (p.key == enumArray.GetValue(j).ToString()) {
                    find = true;
                    break;
                }
            }

            // 存在しない、または重複しているなら削除
            if (!find || this.editParamDic.ContainsKey(p.key)) {
                serializedList.Remove(p);
                --i;
                changed = true;
                continue;
            }

            this.editParamDic.Add(p.key, p);
        }
        
        for (int i = 0; i < objectCount; ++i) {
            string key = enumArray.GetValue(i).ToString();

            // 不足分の項目を作成
            if (!this.editParamDic.ContainsKey(key)) {
				SettingParam param = new SettingParam(key, null, 1);
                this.editParamDic.Add(key, param);
                changed = true;
            }
        }

        this.serializedParams = serializedList.ToArray();

        return changed;
    }

    // シリアライズデータの更新
    public void UpdateSerializedList<U>() where U : struct {
        System.Array enumArray = System.Enum.GetValues(typeof(U));
        int objectCount = enumArray.Length;
        this.serializedParams = new SettingParam[objectCount];

        for (int i = 0; i < objectCount; ++i) {
            string key = enumArray.GetValue(i).ToString();
            SettingParam param = null;
            if (!this.editParamDic.TryGetValue(key, out param))
                param = new AssetCatalog.SettingParam(key, null, 1);

            this.serializedParams[i] = param;
        }
    }
#endif
}
