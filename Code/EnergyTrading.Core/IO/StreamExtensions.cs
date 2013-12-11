namespace EnergyTrading.IO
{
    using System;
    using System.IO;

    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the contents of the stream from the current position
        /// </summary>
        /// <param name="stream">Stream to use</param>
        /// <param name="encoding">Encoding to use, defaults to UTF-8</param>
        /// <returns>The stream contents as a string and the stream positioned where it was on entry.</returns>
        public static string ToEncodedString(this MemoryStream stream, System.Text.Encoding encoding = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (encoding == null)
            {
                encoding = System.Text.Encoding.UTF8;
            }

            // Grab so we can reset to here
            var posn = stream.Position;
            var reader = new StreamReader(stream, encoding);
            var result = reader.ReadToEnd();

            stream.Position = posn;

            return result;
        }
    }
}
