using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrammarCheck
{
    /// <summary>
    ///     Клас мови
    /// </summary>
    public class Language
    {
        /// <summary>
        ///     Назва мови
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Код мови
        /// </summary>
        public String Code { get; set; }

        public static Language[] GetLanguagesArray()
        {
            return null;
        }
    }
}