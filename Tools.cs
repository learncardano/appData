using System;
using System.Globalization;

namespace appData
{
    public partial class Library
    {
        public partial class Tools
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Checks if value is a valid DateTime format.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Boolean IsDateTime(String value)
            {
                try
                { DateTime dt = Convert.ToDateTime(value); return true; }
                catch
                { return false; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Checks if value is a valid DateTime based on the given format.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Boolean IsValidDate(String value, String format)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                try
                { DateTime dt = DateTime.ParseExact(value, format, provider); return true; }
                catch
                { return false; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts String value to DateTime of any valid format.
            /// Returns current date if value is not a valid date format.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Nullable<DateTime> ToDate(String value)
            {
                try
                { return Convert.ToDateTime(value); }
                catch
                { return DateTime.Now; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts String value to DateTime with a specific format of "yyyy/MM/dd hh:mm:ss".
            /// Returns 'null' if value is not in proper format.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Nullable<DateTime> ToDateTime(String value)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                try
                { return DateTime.ParseExact(value, "yyyy/MM/dd hh:mm:ss", provider); }
                catch
                { return null; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts String value to Decimal.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Decimal ToDecimal(String value)
            {
                try
                { return Convert.ToDecimal(value); }
                catch
                { return 0; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts String value to Int32.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 ToInt32(String value)
            {
                try
                { return Convert.ToInt32(value); }
                catch
                { return 0; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts a sentence to array of words.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public String[] ToArray(String sentence)
            {
                try
                { return sentence.Split(' '); }
                catch
                { return new String[0]; }
            }
        }
    }
}