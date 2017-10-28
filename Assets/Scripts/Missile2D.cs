using UnityEngine;


// ミサイルのスーパークラス
public abstract class Missile2D : CachedBehaviour {
	// 最大移動距離
    private const float LIMIT_DIST = 1f;

    protected SpriteRenderer spRender = null;
    protected Vector3 direct = Vector3.down;
    protected float speed = 0.2f;


    // オブジェクトが生成される際の処理
    // GetComponentやnew等メモリアロケートの処理を主に行う
    protected override void OnCreate() {
        this.spRender = this.GetComponent<SpriteRenderer>();
    }
    
    // オブジェクトが破棄される際の処理
    // MaterialやMesh等を生成した場合は忘れずにDestroyする
    protected override void OnRelease() { }
    
    // オブジェクトが呼び出された際の処理
    // 共通パラメータ等の初期化を行う
    protected override void OnAwake(int no) {
        // 稼動No.をプライオリティにして後発の弾が手前
        this.spRender.sortingOrder = no;
    }
    
    // オブジェクトが回収された際の処理
    protected override void OnSleep() { }
    
    // 更新処理
    // no : タスクNo.
    // elapsedTime : 経過時間
    protected override bool OnRun(int no, float elapsedTime) {
        // 稼動No.をプライオリティにして後発の弾が手前
        this.spRender.sortingOrder = no;

        Vector3 point = this.trans_.localPosition;
        point += this.direct * (this.speed * elapsedTime);
    
        // 一定距離進んだら回収
        if (Mathf.Abs(point.x) > LIMIT_DIST)
            return false;
        if (Mathf.Abs(point.y) > LIMIT_DIST)
            return false;
    
        // 移動継続
        this.trans_.localPosition = point;
        return true;
    }
    
    // 点火
    public abstract void Ignition();
}