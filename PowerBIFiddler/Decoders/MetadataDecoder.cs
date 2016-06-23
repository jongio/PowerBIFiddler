namespace PowerBIFiddler
{
    public class MetadataDecoder : DecoderBase
    {
        public override void DecodeInternal(dynamic json)
        {
            if (json.dashboards != null)
            {
                foreach (var dashboard in json.dashboards)
                {
                    if (dashboard.tiles != null)
                    {
                        DecodeTiles(dashboard.tiles);
                    }
                }
            }
        }
    }
}
