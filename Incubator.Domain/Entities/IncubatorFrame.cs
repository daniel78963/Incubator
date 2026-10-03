namespace Incubator.Domain.Entities
{
    public class IncubatorFrame
    {
        public int Id { get; set; }
        public string RawFrame { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Position { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime InitialTime { get; set; }
        public DateTime FinalTime { get; set; }
        public string QrData { get; set; } = string.Empty;
    }

    public static class FrameDecoder
    {
        public static IncubatorFrame Decode(string frame)
        {
            if (!frame.StartsWith("A1A")) throw new ArgumentException("Invalid frame format.");

            var result = new IncubatorFrame { RawFrame = frame };

            // Result: 2 is Negative, 4 is Positive
            result.Result = frame.Substring(3, 1) == "2" ? "Negative" : "Positive";

            // Position: 1 to 8
            result.Position = int.Parse(frame.Substring(4, 1));

            // Final time: hours and minutes
            int finalHours = int.Parse(frame.Substring(7, 2));
            int finalMinutes = int.Parse(frame.Substring(9, 2));

            // Date: day, month, year (last 2 digits)
            int day = int.Parse(frame.Substring(11, 2));
            int month = int.Parse(frame.Substring(13, 2));
            int year = 2000 + int.Parse(frame.Substring(15, 2));

            // Initial time: minutes and hours
            int initialMinutes = int.Parse(frame.Substring(17, 2));
            int initialHours = int.Parse(frame.Substring(19, 2));

            result.FinalTime = new DateTime(year, month, day, finalHours, finalMinutes, 0);
            result.InitialTime = new DateTime(year, month, day, initialHours, initialMinutes, 0);

            // Serial number: exactly 4 digits max
            result.SerialNumber = frame.Substring(21, 4);

            // Check for QR (if frame has more data before the terminators)
            if (frame.Length > 29 && !frame.Contains("----"))
            {
                result.QrData = frame.Substring(25).Replace("\r\n", "");
            }

            return result;
        }
    }
}
