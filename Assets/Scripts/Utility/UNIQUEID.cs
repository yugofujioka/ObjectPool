
// オブジェクトに付与するユニークID
public struct UNIQUEID {
    public static readonly UNIQUEID ZERO = UNIQUEID.Create(0UL);
    public const ulong MASK_CATEGORY  = 0xff00000000000000; // カテゴリ
    public const ulong MASK_TYPE      = 0x00ff000000000000; // 種類
    public const ulong MASK_INDEX     = 0x0000ffff00000000; // 種類No.
    public const ulong MASK_ID        = 0x00000000ffffffff; // フラッシュID


    private static uint flashId = 0; // フラッシュID
    private ulong uniqueId; // ユニークID本体


    private UNIQUEID(ulong id) {
        this.uniqueId = id;
    }

    public static UNIQUEID Create(ulong id) {
        return new UNIQUEID(id);
    }
    // カテゴリ番号をID化（0～255）
    public static ulong CATEGORYBIT(int category) {
        return (ulong)category << 56;
    }
    // 種類番号をID化（0～255）
    public static ulong TYPEBIT(int type) {
        return (ulong)type << 48;
    }
    // 配列インデックスをID化（0～65535）
    public static ulong INDEXBIT(int index) {
        return (ulong)index << 32;
    }
    // ID一致確認用オペレータ
    public static bool operator ==(UNIQUEID a, UNIQUEID b) {
        ulong aId = ((object)a == null ? 0xffffffffffffffff : a.uniqueId);
        ulong bId = ((object)b == null ? 0xffffffffffffffff : b.uniqueId);
        return aId == bId;
    }
    public static bool operator !=(UNIQUEID a, UNIQUEID b) {
        ulong aId = ((object)a == null ? 0xffffffffffffffff : a.uniqueId);
        ulong bId = ((object)b == null ? 0xffffffffffffffff : b.uniqueId);
        return aId != bId;
    }

    public static ulong operator &(UNIQUEID a, UNIQUEID b) {
        return (a.uniqueId & b.uniqueId);
    }
    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;
        UNIQUEID p = (UNIQUEID)obj;
        return this.uniqueId == p.uniqueId;
    }
    public bool Equals(UNIQUEID p) {
        if ((object)p == null)
            return false;
        return this.uniqueId == p.uniqueId;
    }
    public override int GetHashCode() {
        return (int)this.uniqueId;    // 使わないので適当
    }
    public override string ToString() {
        return string.Format("{0}", this.uniqueId);
    }

    // フラッシュIDの更新
    // オブジェクトが起動する度に更新することで、プールから使い回されたものか判定する
    public ulong Update() {
        ulong header = this.uniqueId & (MASK_CATEGORY|MASK_TYPE|MASK_INDEX);
        ++flashId;    // 32bit範囲内でループ
        this.uniqueId = header | flashId;
        return this.uniqueId;
    }
    // IDから実体の取得
    public ulong num {
        get { return this.uniqueId; }
    }
    // IDからカテゴリの取得
    public int category {
        get { return (int)((this.uniqueId & MASK_CATEGORY) >> 56); }
    }
    // IDから種類の取得
    public int type {
        get { return (int)((this.uniqueId & MASK_TYPE) >> 48); }
    }
    // IDから生成No.の取得
    public int index {
        get { return (int)((this.uniqueId & MASK_INDEX) >> 32); }
        set {
            this.uniqueId &= ~(this.uniqueId & MASK_INDEX);
            this.uniqueId |= INDEXBIT(value);
        }
    }
    // IDからフラッシュIDの取得
    public uint id {
        get { return (uint)(this.uniqueId & MASK_ID); }
    }
}