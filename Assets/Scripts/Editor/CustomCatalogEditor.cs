using UnityEditor;
using UnityEngine;


// MissilePrefabカタログ
[CustomEditor(typeof(MissileCatalog))]
public sealed class MissileCatalogEditor : AssetCatalogEditor<GameObject, MISSILE> { }
