using UnityEngine;


// 諸々キャッシュされるMonoBehaviour
public abstract class CachedBehaviour : MonoBehaviour {
    // 算術値のキャッシュ、operatorのオーバーヘッドを省略する
    private static readonly Vector3 VECTOR_ONE = Vector3.one;
    private static readonly Quaternion ROTATE_NONE = Quaternion.identity;


    [System.NonSerialized]
    public UNIQUEID uniqueId = UNIQUEID.ZERO;

    // 参照キャッシュ
    protected GameObject go_ = null;
    protected Transform trans_ = null;

    private bool alive = false;    // オブジェクトが有効か
    private bool reqStop = false;  // 停止するか


    // 生存フラグ公開
    public bool isAlive { get { return this.alive; } }


    // 生成処理
    public void Create(UNIQUEID uniqueId) {
        this.uniqueId = uniqueId;
        this.go_ = this.gameObject;
        this.trans_ = this.transform;
        this.go_.SetActive(false);
        this.OnCreate();
    }
    // 廃棄処理
    public void Release() {
        this.OnRelease();    // 派生クラス処理
    }
    // 起動処理
    public void WakeUp(int no, Vector3 localPoint) {
        this.uniqueId.Update();
        this.go_.SetActive(true);
        this.trans_.localRotation = ROTATE_NONE;
        this.trans_.localScale = VECTOR_ONE;
        this.trans_.localPosition = localPoint;

        this.OnAwake(no);    // 派生クラス処理
    }
    // 停止処理
    public void Sleep() {
        this.go_.SetActive(false);
        this.OnSleep();   // 派生クラス処理
    }
    // 更新処理
    public bool Run(int no, float elapsedTime) {
        // 強制終了
        if (this.reqStop)
            return false;
        this.alive = this.OnRun(no, elapsedTime);    // 派生クラス処理
        return this.alive;
    }

    // 停止命令
    public virtual void Stop() {
        this.reqStop = true;
        this.alive = false;
    }

    // 派生クラスでの固有定義
    protected abstract void OnCreate();
    protected abstract void OnRelease();
    protected abstract void OnAwake(int no);
    protected abstract void OnSleep();
    protected abstract bool OnRun(int no, float elapsedTime);
}