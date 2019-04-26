﻿using System.Collections.Generic;

namespace ActiveSync.RequestProcessor.WBXML
{
    public class ASWBXMLCodePage
    {
        private string strNamespace = "";
        private string strXmlns = "";
        private Dictionary<byte, string> tokenLookup = new Dictionary<byte, string>();
        private Dictionary<string, byte> tagLookup = new Dictionary<string, byte>();

        public string Namespace
        {
            get
            {
                return strNamespace;
            }
            set
            {
                strNamespace = value;
            }
        }

        public string Xmlns
        {
            get
            {
                return strXmlns;
            }
            set
            {
                strXmlns = value;
            }
        }

        public void AddToken(byte token, string tag)
        {
            tokenLookup.Add(token, tag);
            tagLookup.Add(tag, token);
        }

        public byte GetToken(string tag)
        {
            if (tagLookup.ContainsKey(tag))
                return tagLookup[tag];

            return 0xFF;
        }

        public string GetTag(byte token)
        {
            if (tokenLookup.ContainsKey(token))
                return tokenLookup[token];

            return null;
        }
    }
}