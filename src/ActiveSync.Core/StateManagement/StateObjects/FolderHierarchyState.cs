using System.Collections.Generic;

namespace ActiveSync.Core.StateManagement.StateObjects
{
    public class FolderHierarchyState : StateObject
    {
        public List<FolderState> Folders { get; set; }
    }

    public class FolderState
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public int FolderType { get; set; }
        public string ParentId { get; set; }
    }
}
