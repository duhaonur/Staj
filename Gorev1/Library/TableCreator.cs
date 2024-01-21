namespace Library
{
    internal class TableCreator
    {
        public string GenerateTableHeader(int[] minLengths, int[] maxLengths, params string[] columnHeaders)
        {
            string format = "|";
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                format += $"{{{i},-{Math.Max(minLengths[i], maxLengths[i])}}}|";
            }

            return string.Format(format, columnHeaders);
        }

        public string GenerateTableRow(int[] minLengths, int[] maxLengths, params string[] values)
        {
            string format = "|";
            for (int i = 0; i < values.Length; i++)
            {
                format += $"{{{i},-{Math.Max(minLengths[i], maxLengths[i])}}}|";
            }

            return string.Format(format, values);
        }

        public string GenerateHorizontalLine(int[] minLengths, int[] maxLengths)
        {
            string line = "|";
            for (int i = 0; i < maxLengths.Length; i++)
            {
                line += new string('-', Math.Max(minLengths[i], maxLengths[i])) + "|";
            }

            return line;
        }
    }
}
