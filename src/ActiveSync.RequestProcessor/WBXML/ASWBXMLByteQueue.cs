using System.Collections.Generic;
using System.Text;

namespace ActiveSync.RequestProcessor.WBXML
{
    public class ASWBXMLByteQueue : Queue<byte>
    {
        public ASWBXMLByteQueue(byte[] bytes)
            : base(bytes)
        {
        }

        public int DequeueMultibyteInt()
        {
            int iReturn = 0;
            byte singleByte = 0xFF;

            do
            {
                iReturn <<= 7;

                singleByte = this.Dequeue();
                iReturn += (int)(singleByte & 0x7F);
            }
            while (CheckContinuationBit(singleByte));

            return iReturn;
        }

        private bool CheckContinuationBit(byte byteval)
        {
            byte continuationBitmask = 0x80;
            return (continuationBitmask & byteval) != 0;
        }

        public string DequeueString()
        {
            //HACK: Change to support Utf-8
            #region Commented By AliReza

            //StringBuilder strReturn = new StringBuilder();
            //byte currentByte = 0x00;
            //do
            //{
            //    // TODO: Improve this handling. We are technically UTF-8, meaning
            //    // that characters could be more than one byte long. This will fail if we have
            //    // characters outside of the US-ASCII range
            //    currentByte = this.Dequeue();
            //    if (currentByte != 0x00)
            //    {
            //        strReturn.Append((char)currentByte);
            //    }
            //}
            //while (currentByte != 0x00);

            //return strReturn.ToString();


            #endregion

            #region New Code By AliReza
            byte currentByte = 0x00;
            var byteArray = new List<byte>();
            do
            {
                currentByte = this.Dequeue();
                if (currentByte != 0x00)
                {
                    byteArray.Add(currentByte);
                }
            }
            while (currentByte != 0x00);

            var strReturn = Encoding.UTF8.GetString(byteArray.ToArray());

            return strReturn.ToString();

            #endregion
        }

        public string DequeueString(int length)
        {
            StringBuilder strReturn = new StringBuilder();

            byte currentByte = 0x00;
            for (int i = 0; i < length; i++)
            {
                // TODO: Improve this handling. We are technically UTF-8, meaning
                // that characters could be more than one byte long. This will fail if we have
                // characters outside of the US-ASCII range
                currentByte = this.Dequeue();
                strReturn.Append((char)currentByte);
            }

            return strReturn.ToString();
        }
    }
}
