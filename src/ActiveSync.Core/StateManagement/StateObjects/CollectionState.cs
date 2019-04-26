using System.Collections.Generic;
using System.Linq;

namespace ActiveSync.Core.StateManagement.StateObjects
{
    public class CollectionState : StateObject
    {
        public CollectionState()
        {
            Collections = new List<SyncItemState>();
        }
        public List<SyncItemState> Collections { get; set; }
        public string FolderId { get; set; }

        public void AddItem(SyncItemState syncItemState)
        {
            if(Collections == null)
                Collections = new List<SyncItemState>();
            var existStateItem = Collections.FirstOrDefault(x => x.ServerId == syncItemState.ServerId);
            
            if (existStateItem != null)
                UpdateItem(existStateItem.ServerId, syncItemState.HashKey);
            else
                Collections.Add(syncItemState);
        }

        public void UpdateItem(string serverId ,string newHashKey)
        {
            var existStateItem = Collections.FirstOrDefault(x => x.ServerId == serverId);

            if (existStateItem != null)
                existStateItem.HashKey = newHashKey;
        }

        public void DeleteItem(string serverId)
        {
            var existStateItem = Collections.FirstOrDefault(x => x.ServerId == serverId);

            if (existStateItem != null)
                Collections.Remove(existStateItem);
        }
    }

    public class SyncItemState
    {
        public string ServerId { get; set; }
        public string HashKey { get; set; }
    }
}
