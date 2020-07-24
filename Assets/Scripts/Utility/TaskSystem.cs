using UnityEngine;
using Unity.IL2CPP.CompilerServices;


namespace TaskSystem {
    // true:継続, false:終了
    public delegate bool OrderHandler<T>(T obj, int no);
    
    // タスク管理
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TaskSystem<T> {
        private Task<T> top = null;    // 先端
        private Task<T> end = null;    // 終端
    
        private int capacity = 0;      // 最大タスク数
        private int freeCount = -1;    // 空きタスクインデックス
        private int actCount = 0;      // 稼動タスク数
        private Task<T>[] taskPool = null;    // 生成された全タスク
        private Task<T>[] activeTask = null;  // 待機中のプール
    
        // 稼動数
        public int count { get { return this.actCount; } }
    
    
        // コンストラクタ、引数には最大タスク接続数
        public TaskSystem(int capacity) {
            this.capacity = capacity;
            this.taskPool = new Task<T>[this.capacity];
            this.activeTask = new Task<T>[this.capacity];
            for (int i = 0; i < this.capacity; ++i) {
                this.taskPool[i] = new Task<T>();
                this.activeTask[i] = this.taskPool[i];
            }
            this.freeCount = this.capacity;
        }
        // 初期化
        public void Clear() {
            this.freeCount = this.capacity;
            this.actCount = 0;
            this.top = null;
            this.end = null;
    
            for (int i = 0; i < this.capacity; ++i) {
                this.taskPool[i].prev = null;
                this.taskPool[i].next = null;
                this.activeTask[i] = this.taskPool[i];
            }
        }
        // 接続
        public void Attach(T item) {
            Debug.Assert(item != null, "アタッチエラー");
            Debug.Assert(this.freeCount > 0, "キャパシティオーバー");
    
            Task<T> task = this.activeTask[this.freeCount - 1];
            task.item = item;
    
            if (this.actCount > 0) {
                task.Attach(this.end, null);
                this.end = task;
            } else {
                task.Attach(null, null);
                this.end = task; this.top = task;
            }
    
            --this.freeCount;
            ++this.actCount;
        }
        // 接続解除
        internal void Detach(Task<T> task) {
            if (task == this.top)
                this.top = task.next;
            if (task == this.end)
                this.end = task.prev;
            task.Detach();
    
            --this.actCount;
            ++this.freeCount;
            this.activeTask[this.freeCount-1] = task;
        }
        // 全タスクに指令
        public void Order(OrderHandler<T> order) {
            int no = 0;
            Task<T> now = null;
            for (Task<T> task = this.top; task != null && this.actCount > 0;) {
                now = task;
                task = task.next;
                if (!order(now.item, no))
                    this.Detach(now);
                ++no;
            }
        }
    }
}