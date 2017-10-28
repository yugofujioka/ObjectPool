using UnityEngine;


// ミサイル種類
public enum MISSILE {
    UP,
    RIGHT,
    DOWN,
    LEFT,

    MAX,
}

// ミサイルカタログ
[CreateAssetMenu(menuName = "Catalog/Missile")]
public sealed class MissileCatalog : AssetCatalog { }
