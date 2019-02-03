using UnityEngine;


// 仮砲台のクラス
public class Shooter : MonoBehaviour {
    [SerializeField]
    private MissileCatalog missileCatalog = null;

    private ObjectPool<Missile2D> pool = new ObjectPool<Missile2D>();
    private MISSILE nextType = MISSILE.UP;
	private float calcTime = 0f;


    public void Start() {
        // プール内バッファ生成
        this.pool.Initialize(0, this.missileCatalog);
        // オブジェクトの生成
        this.pool.Generate();
    }
    public void OnDestroy() {
        this.pool.Final();
    }
    public void Update() {
        // スペースキーで一括回収
        if (Input.GetKeyDown(KeyCode.Space))
            this.pool.Clear();

        // アクティブなオブジェクト数の更新
        // 呼び出されたフレームで経過時間0秒で処理されていたものを通常稼動扱いにする
        this.pool.FrameTop();

        float elapsedTime = Time.deltaTime;
        this.calcTime += elapsedTime;
        // とりあえず0.1秒毎に発射
        float span = 0.1f;
        if (this.calcTime >= span) {
            // プールから取り出して点火
            Missile2D missile;
            // ※Transformキャッシュを本来はすべき
            Vector3 point = this.transform.localPosition;
            if (this.pool.AwakeObject(this.nextType, point, out missile))
                missile.Ignition();

            // 上下左右と順番
            if (this.nextType== MISSILE.LEFT)
                this.nextType= MISSILE.UP;
            else
                ++this.nextType;
            this.calcTime -= span;
        }

        // アクティブなオブジェクトの更新
        this.pool.Proc(elapsedTime);
    }
}