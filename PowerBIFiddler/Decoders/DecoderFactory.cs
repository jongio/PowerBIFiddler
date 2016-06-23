using System.Text.RegularExpressions;

namespace PowerBIFiddler
{
    public static class DecoderFactory
    {
        public static IDecoder Get(string path)
        {
            var regex = new Regex("(?<tiles>/powerbi/metadata/dashboard/(\\d+)/tiles)|(?<metadata>/powerbi/metadata/app)");
            var match = regex.Match(path);

            if (match.Success)
            {
                if (match.Groups["tiles"].Success)
                {
                    return new TilesDecoder();
                }
                else
                {
                    return new MetadataDecoder();
                }
            }

            return null;
        }
    }
}
