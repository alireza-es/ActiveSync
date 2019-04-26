using System.Collections.Generic;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Comparers
{
    public class FolderComparerByServerId : IEqualityComparer<SyncableFolder>
    {
        public bool Equals(SyncableFolder x, SyncableFolder y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SyncableFolder obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
