using UnityEngine;


// 上方向に向かうミサイル
public class MissileUp : Missile2D {
    public override void Ignition() {
        this.direct = Vector3.up;
    }
}
