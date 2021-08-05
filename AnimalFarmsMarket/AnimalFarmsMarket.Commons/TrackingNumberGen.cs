using System;
using System.Text;

namespace AnimalFarmsMarket.Commons
{
    public static class TrackingNumberGen {
        // Instantiate random number generator.  
        // It is better to keep a single Random instance 
        // and keep using Next on the same instance.  
        private static readonly Random _random = new Random();


        // Generates a random string with a given size.    
        public static string RandomString(bool lowerCase = false) {
            var builder = new StringBuilder(16);
            builder.Append("LV247-");

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            char numOffset = '0';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < 4; i++) {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            for(var i = 0; i<=6; i++)
            {
                var @char = (char)_random.Next(numOffset, numOffset + 10);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
