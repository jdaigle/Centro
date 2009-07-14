using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Extensions
{
    public static class Versions
    {
        public static readonly Version Empty = new Version(0, 0, 0, 0);

        /// <summary>
        /// Attempts to parse a string into a Version object.
        /// </summary>
        /// <param name="versionValue">The string version value.</param>
        /// <param name="version">The output version.</param>
        /// <returns>True if the version was parsed, false otherwise</returns>
        public static bool TryParse(string versionValue, out Version version)
        {
            try
            {
                version = new Version(versionValue);
                return true;
            }
            catch (FormatException)
            {
            }
            catch (ArgumentNullException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (Exception)
            {
                throw;
            }
            version = null;
            return false;
        }
    }
}
