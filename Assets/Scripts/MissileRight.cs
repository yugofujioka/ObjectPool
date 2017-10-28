using UnityEngine;


// 右方向直進弾
public class MissileRight : Missile2D {
    // 弾の発射処理
    public override void Ignition() {
        this.direct = Vector3.right;
    }
}