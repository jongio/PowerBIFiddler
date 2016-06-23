using Fiddler;
using Newtonsoft.Json;
using System;
using System.Text;

namespace PowerBIFiddler
{
    public class DecoderBase : IDecoder
    {
        public byte[] Decode(byte[] body, bool encoded, HTTPResponseHeaders headers)
        {
            if (encoded)
            {
                // decode body
                byte[] numArray = Utilities.Dupe(body);
                Utilities.utilDecodeHTTPBody(headers, ref numArray);
                body = numArray;
            }

            // get body as string
            string raw = Encoding.Default.GetString(body);

            // convert to json
            dynamic json = JsonConvert.DeserializeObject(raw);

            // decode and decompress the json
            DecodeInternal(json);

            // serialize json back to bytes
            body = Encoding.Default.GetBytes(JsonConvert.SerializeObject(json));

            return encoded ? Util.Compress(body) : body;
        }

        public virtual void DecodeInternal(dynamic json) { }

        public void DecodeTiles(dynamic json)
        {
            // loop through each tile and extra the base64/gziped data
            foreach (var tile in json)
            {
                // Trim off the curlys
                string trimmedData = ((string)tile.tileDataBinaryBase64Encoded).Trim("{}".ToCharArray());

                // Convert to bytes for decompressing
                byte[] base64Bytes = Convert.FromBase64String(trimmedData);

                // Decompress to bytes
                byte[] decompressedBytes = Util.Decompress(base64Bytes);

                // Convert to string to save back to json
                string decompressedString = Encoding.Default.GetString(decompressedBytes);

                tile.tileData = decompressedString;
            }
        }
    }
}
