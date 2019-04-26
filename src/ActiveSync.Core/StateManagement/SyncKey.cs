using ActiveSync.Core.Exceptions;

namespace ActiveSync.Core.StateManagement
{
    //TODO: Rename this Type name
    public class SyncKey
    {
        private const char SYNC_KEY_SEPERATOR = '-';
        public string Key { get; set; }
        public int Counter { get; set; }
        public override string ToString()
        {
            return Key + SYNC_KEY_SEPERATOR + Counter;
        }

        public static bool TryParse(string syncKey, out SyncKey syncKeyObject)
        {
            try
            {
                var syncKeyTemp = (SyncKey) syncKey;
                syncKeyObject = syncKeyTemp;

                return syncKeyTemp != null;
            }
            catch
            {
                syncKeyObject = null;
                return false;
            }
        }
        
        public static implicit operator string(SyncKey value)
        {
            return value != null ? value.ToString() : null;
        }

        public static implicit operator SyncKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var splittedKey = value.Split(SYNC_KEY_SEPERATOR);
            if (splittedKey.Length != 2)
            {
                throw new InvalidSyncKeyException(value);
            }

            int counter;

            if (!int.TryParse(splittedKey[1], out counter))
                throw new InvalidSyncKeyException(value);

            return new SyncKey
            {
                Counter = counter,
                Key = splittedKey[0]
            };
        }

    }
}
