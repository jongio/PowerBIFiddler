namespace PowerBIFiddler
{
    public class TilesDecoder : DecoderBase
    {
        public override void DecodeInternal(dynamic json)
        {
            if (json != null)
            {
                DecodeTiles(json);
            }
        }
    }
}
