using System;
using System.Collections.Generic;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.MockImplementation.Service
{
    public class FolderService:IFolderService
    {
        public CreateFolderResult CreateFolder(UserCredential userCredential, SyncableFolder syncFolder, out string createdFolderId)
        {
            throw new NotImplementedException();
        }

        public UpdateFolderResult UpdateFolder(UserCredential userCredential, SyncableFolder syncFolder)
        {
            throw new NotImplementedException();
        }

        public DeleteFolderResult DeleteFolder(UserCredential userCredential, string id)
        {
            throw new NotImplementedException();
        }

        public SyncableFolder GetFolder(UserCredential userCredential, string id)
        {
            throw new NotImplementedException();
        }

        public IList<SyncableFolder> GetAllFolders(UserCredential userCredential)
        {
            throw new NotImplementedException();
        }

        public void EmptyFolderContents(UserCredential userCredential, string folderId)
        {
            throw new NotImplementedException();
        }
    }
}
