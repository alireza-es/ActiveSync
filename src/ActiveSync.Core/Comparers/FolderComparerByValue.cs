using System.Collections.Generic;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Comparers
{
    public class FolderComparerByValue : IEqualityComparer<SyncableFolder>
    {
        public bool Equals(SyncableFolder x, SyncableFolder y)
        {
            return x.Id == y.Id &&
                   x.DisplayName == y.DisplayName &&
                   x.ParentId == y.ParentId &&
                   x.Type == y.Type;
        }

        public int GetHashCode(SyncableFolder obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}