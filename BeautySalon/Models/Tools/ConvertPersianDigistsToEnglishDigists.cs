using System.Collections.Generic;

namespace BeautySalon.Models.Tools
{
    public static class ConvertPersianDigistsToEnglishDigists
    {
        public static string Convertor(this string number)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in number)
            {
                number = number.Replace(item, LettersDictionary[item]);
            }
            return number;
        }
    }
}
