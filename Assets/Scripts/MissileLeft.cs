using UnityEngine;


// 左方向直進弾
public class MissileLeft : Missile2D {
    // 弾の発射処理
    public override void Ignition() {
        this.direct = Vector3.left;
    }
}