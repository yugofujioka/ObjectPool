
namespace TaskSystem {
    // タスク
    internal sealed class Task<T> {
        public T item = default(T);
        public Task<T> prev = null;
        public Task<T> next = null;

        // 接続処理
        /// prev : 接続する前のノード
        /// next : 接続する後ろのノード
        public void Attach(Task<T> prev, Task<T> next) {
            this.prev = prev;
            this.next = next;
            if (prev != null)
                prev.next = this;
            if (next != null)
                next.prev = this;
        }

        /// 切断処理
        public void Detach() {
            if (this.prev != null)
                this.prev.next = this.next;
            if (this.next != null)
                this.next.prev = this.prev;

            this.prev = null;
            this.next = null;
        }
    }
}