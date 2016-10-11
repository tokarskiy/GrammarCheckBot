using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace GrammarCheck
{
    /// <summary>
    ///     Клас орфографічної помилки
    /// </summary>
    public class Mistake
    {
        /// <summary>
        ///     Слово, у якому знаходиться помилка
        /// </summary>
        public String Word { get; set; }

        /// <summary>
        ///     Позиція першої букви слова, у якому знаходиться помилка
        /// </summary>
        public Int32 Offset { get; set; }

        /// <summary>
        ///     Довжина слова, у якому знаходиться помилка  
        /// </summary>
        public Int32 Length { get; set; }

        /// <summary>
        ///     Варіанти правильних еквівалентів слова із помилкою
        /// </summary>
        public String[] Replacements { get; set; }

        /// <summary>
        ///     Знайти всі орфографічні помилки у слові
        /// </summary>
        /// <param name="message">Повідомлення, що перевіряється</param>
        /// <param name="lang">Мова повідомлення</param>
        /// <returns>Масив об'єктів класу Mistake</returns>
        public static Mistake[] FindMistakes(String message, String lang)
        {
            using (var client = new System.Net.WebClient())
            {
                client.Encoding = Encoding.UTF8;
                //client.Headers["Content-Type"] = "application/json;charset=\"utf-8\"";
                //client.Headers.Add("Content-Type", "application/json;charset=\"utf-8\"");
                var data = new NameValueCollection();
                data.Add("text", message);
                data.Add("language", lang);
                data.Add("enabledOnly", "false");
                data.Add("disabledRules", "UPPERCASE_SENTENCE_START");

                var result = client.UploadValues(
                    @"https://languagetool.org/api/v2/check",
                    data
                    );

                var strRes = Encoding.Default.GetString(result);
                var jobject = JObject.Parse(strRes);

                var mistakes =
                    from x in jobject["matches"]
                    let offset = (Int32)x["offset"]
                    let length = (Int32)x["length"]
                    let word = message.Substring(offset, length)
                    let replacements = (
                        from replacement in x["replacements"]
                        let replString = (String)replacement["value"]
                        where !replString.Contains(' ')
                        select replString
                        ).ToArray()
                    select new Mistake
                    {
                        Word = word,
                        Offset = offset,
                        Length = length,
                        Replacements = replacements
                    };

                return mistakes.ToArray();
            }
        }

        /// <summary>
        ///     Метод переводить об'єкт помилки у строковий формат у
        ///     виді, в якому її отримає користувач.
        /// </summary>
        /// <returns>Помилка у строковому форматі</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Mistake in the word '{Word}'. ");
            Int32 length = Replacements.Length;
            if (length != 0)
            {
                sb.Append($"Possible replacements: ");
                for (Int32 i = 0; i < length; i++)
                {
                    sb.Append(String.Format("{0}{1} ", Replacements[i], i == length - 1 ? '.' : ','));
                }
            }
            else
            {
                sb.Append("This word and words like it do not exist");
            }
            return sb.ToString();
        }
    }
}