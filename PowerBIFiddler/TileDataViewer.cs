using Fiddler;
using Newtonsoft.Json;
using Standard;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PowerBIFiddler
{
    public class TileDataViewer : Inspector2, IResponseInspector2
    {
        JSONResponseViewer jsonResponseViewer;
        HTTPResponseHeaders responseHeaders;
        bool isTileResponse = false;

        public byte[] body
        {
            get
            {
                return jsonResponseViewer.body;
            }
            set
            {
                if (isTileResponse)
                {
                    try
                    {
                        var isEncoded = this.responseHeaders.ExistsAndContains("Content-Encoding", "gzip");
                        if (isEncoded)
                        {
                            // decode body
                            byte[] numArray = Utilities.Dupe(value);
                            Utilities.utilDecodeHTTPBody(this.headers, ref numArray);
                            value = numArray;
                        }

                        // get body as string
                        string raw = Encoding.Default.GetString(value);

                        // convert to json
                        dynamic json = JsonConvert.DeserializeObject(raw);

                        // loop through each tile and extra the base64/gziped data
                        foreach (var tile in json)
                        {
                            // Trim off the curlys
                            string trimmedData = ((string)tile.tileDataBinaryBase64Encoded).Trim("{}".ToCharArray());

                            // Convert to bytes for decompressing
                            byte[] base64Bytes = Convert.FromBase64String(trimmedData);

                            // Decompress to bytes
                            byte[] decompressedBytes = Decompress(base64Bytes);

                            // Convert to string to save back to json
                            string decompressedString = Encoding.Default.GetString(decompressedBytes);

                            tile.tileData = decompressedString;
                        }

                        // serialize json back to bytes
                        var jsonBytes = Encoding.Default.GetBytes(JsonConvert.SerializeObject(json));
                        value = isEncoded ? Compress(jsonBytes) : jsonBytes;
                    }
                    catch { }
                }
                jsonResponseViewer.body = value;
            }
        }

        static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        static byte[] Compress(byte[] data)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return memory.ToArray();
            }
        }

        public bool bReadOnly { get { return jsonResponseViewer.bReadOnly; } set { jsonResponseViewer.bReadOnly = value; } }

        public bool bDirty { get { return jsonResponseViewer.bDirty; } }

        public HTTPResponseHeaders headers
        {
            get
            {
                return responseHeaders;
            }

            set
            {
                responseHeaders = value;
                jsonResponseViewer.headers = value;
            }
        }

        public void AssignMessage(WebSocketMessage oWSM)
        {
            jsonResponseViewer.AssignMessage(oWSM);
        }

        public override void AssignSession(Session oS)
        {
            SetIsTileResponse(oS);
            base.AssignSession(oS);
        }

        public override void AddToTab(TabPage o)
        {
            jsonResponseViewer = new JSONResponseViewer();
            jsonResponseViewer.AddToTab(o);
            o.Text = "Power BI";

        }

        public void Clear()
        {
            jsonResponseViewer.Clear();
        }

        public override int GetOrder()
        {
            return jsonResponseViewer.GetOrder();
        }

        public override int ScoreForContentType(string sMIMEType)
        {
            return jsonResponseViewer.ScoreForContentType(sMIMEType);
        }

        public override void SetFontSize(float flSizeInPoints)
        {
            jsonResponseViewer.SetFontSize(flSizeInPoints);
        }

        public void SetIsTileResponse(Session oS)
        {
            string pathRegEx = "/powerbi/metadata/dashboard/(\\d+)/tiles";
            isTileResponse = Regex.IsMatch(oS.PathAndQuery, pathRegEx);
        }
    }
}