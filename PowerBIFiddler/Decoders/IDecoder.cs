using Fiddler;

namespace PowerBIFiddler
{
    public interface IDecoder
    {
        byte[] Decode(byte[] value, bool encoded, HTTPResponseHeaders headers);
        void DecodeInternal(dynamic json);
    }
}
