using UnityEngine;


// 下方向直進弾
public class MissileDown : Missile2D {
    public override void Ignition() {
        this.direct = Vector3.down;
    }
}