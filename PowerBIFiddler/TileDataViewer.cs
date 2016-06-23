using Fiddler;
using Standard;
using System.Windows.Forms;

namespace PowerBIFiddler
{
    public class TileDataViewer : Inspector2, IResponseInspector2
    {
        JSONResponseViewer jsonResponseViewer;
        HTTPResponseHeaders responseHeaders;
        IDecoder decoder;

        public byte[] body
        {
            get
            {
                return jsonResponseViewer.body;
            }
            set
            {
                if (decoder != null && value != null)
                {
                    value = decoder.Decode(value, encoded, headers);
                }
                jsonResponseViewer.body = value;
            }
        }

        public bool bReadOnly { get { return jsonResponseViewer.bReadOnly; } set { jsonResponseViewer.bReadOnly = value; } }

        public bool bDirty { get { return jsonResponseViewer.bDirty; } }

        public bool encoded { get { return this.responseHeaders.ExistsAndContains("Content-Encoding", "gzip"); } }

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
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
            decoder = DecoderFactory.Get(oS.PathAndQuery);
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
    }
}