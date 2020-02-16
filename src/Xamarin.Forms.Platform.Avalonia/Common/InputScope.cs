using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class InputScope
    {
        public IList Names = new List<InputScopeName>();
    }

    public class InputScopeName
    {
        public InputScopeNameValue NameValue { get; set; }
    }

    //
    // Summary:
    //     Specifies the input scope name which modifies how input from alternative input
    //     methods is interpreted.
    public enum InputScopeNameValue
    {
        //
        // Summary:
        //     The text input pattern for XML.
        Xml = -4,
        //
        // Summary:
        //     The text input pattern for the Speech Recognition Grammar Specification (SRGS).
        Srgs = -3,
        //
        // Summary:
        //     The text input pattern for a regular expression.
        RegularExpression = -2,
        //
        // Summary:
        //     The text input pattern for a phrase list.
        PhraseList = -1,
        //
        // Summary:
        //     The default handling of input commands.
        Default = 0,
        //
        // Summary:
        //     The text input pattern for a Uniform Resource Locator (URL).
        Url = 1,
        //
        // Summary:
        //     The text input pattern for the full path of a file.
        FullFilePath = 2,
        //
        // Summary:
        //     The text input pattern for a file name.
        FileName = 3,
        //
        // Summary:
        //     The text input pattern for an email user name.
        EmailUserName = 4,
        //
        // Summary:
        //     The text input pattern for a Simple Mail Transfer Protocol (SMTP) email address.
        EmailSmtpAddress = 5,
        //
        // Summary:
        //     The text input pattern for a log on name.
        LogOnName = 6,
        //
        // Summary:
        //     The text input pattern for a person's full name.
        PersonalFullName = 7,
        //
        // Summary:
        //     The text input pattern for the prefix of a person's name.
        PersonalNamePrefix = 8,
        //
        // Summary:
        //     The text input pattern for a person's given name.
        PersonalGivenName = 9,
        //
        // Summary:
        //     The text input pattern for a person's middle name.
        PersonalMiddleName = 10,
        //
        // Summary:
        //     The text input pattern for a person's surname.
        PersonalSurname = 11,
        //
        // Summary:
        //     The text input pattern for the suffix of a person's name.
        PersonalNameSuffix = 12,
        //
        // Summary:
        //     The text input pattern for a postal address.
        PostalAddress = 13,
        //
        // Summary:
        //     The text input pattern for a postal code.
        PostalCode = 14,
        //
        // Summary:
        //     The text input pattern for a street address.
        AddressStreet = 15,
        //
        // Summary:
        //     The text input pattern for a state or province.
        AddressStateOrProvince = 16,
        //
        // Summary:
        //     The text input pattern for a city address.
        AddressCity = 17,
        //
        // Summary:
        //     The text input pattern for the name of a country.
        AddressCountryName = 18,
        //
        // Summary:
        //     The text input pattern for the abbreviated name of a country.
        AddressCountryShortName = 19,
        //
        // Summary:
        //     The text input pattern for amount and symbol of currency.
        CurrencyAmountAndSymbol = 20,
        //
        // Summary:
        //     The text input pattern for amount of currency.
        CurrencyAmount = 21,
        //
        // Summary:
        //     The text input pattern for a calendar date.
        Date = 22,
        //
        // Summary:
        //     The text input pattern for the numeric month in a calendar date.
        DateMonth = 23,
        //
        // Summary:
        //     The text input pattern for the numeric day in a calendar date.
        DateDay = 24,
        //
        // Summary:
        //     The text input pattern for the year in a calendar date.
        DateYear = 25,
        //
        // Summary:
        //     The text input pattern for the name of the month in a calendar date.
        DateMonthName = 26,
        //
        // Summary:
        //     The text input pattern for the name of the day in a calendar date.
        DateDayName = 27,
        //
        // Summary:
        //     The text input pattern for digits.
        Digits = 28,
        //
        // Summary:
        //     The text input pattern for a number.
        Number = 29,
        //
        // Summary:
        //     The text input pattern for one character.
        OneChar = 30,
        //
        // Summary:
        //     The text input pattern for a password.
        Password = 31,
        //
        // Summary:
        //     The text input pattern for a telephone number.
        TelephoneNumber = 32,
        //
        // Summary:
        //     The text input pattern for a telephone country code.
        TelephoneCountryCode = 33,
        //
        // Summary:
        //     The text input pattern for a telephone area code.
        TelephoneAreaCode = 34,
        //
        // Summary:
        //     The text input pattern for a telephone local number.
        TelephoneLocalNumber = 35,
        //
        // Summary:
        //     The text input pattern for the time.
        Time = 36,
        //
        // Summary:
        //     The text input pattern for the hour of the time.
        TimeHour = 37,
        //
        // Summary:
        //     The text input pattern for the minutes or seconds of time.
        TimeMinorSec = 38,
        //
        // Summary:
        //     The text input pattern for a full-width number.
        NumberFullWidth = 39,
        //
        // Summary:
        //     The text input pattern for alphanumeric half-width characters.
        AlphanumericHalfWidth = 40,
        //
        // Summary:
        //     The text input pattern for alphanumeric full-width characters.
        AlphanumericFullWidth = 41,
        //
        // Summary:
        //     The text input pattern for Chinese currency.
        CurrencyChinese = 42,
        //
        // Summary:
        //     The text input pattern for the Bopomofo Mandarin Chinese phonetic transcription
        //     system.
        Bopomofo = 43,
        //
        // Summary:
        //     The text input pattern for the Hiragana writing system.
        Hiragana = 44,
        //
        // Summary:
        //     The text input pattern for half-width Katakana characters.
        KatakanaHalfWidth = 45,
        //
        // Summary:
        //     The text input pattern for full-width Katakana characters.
        KatakanaFullWidth = 46,
        //
        // Summary:
        //     The text input pattern for Hanja characters.
        Hanja = 47,

        HangulHalfWidth = 48,
        HangulFullWidth = 49,
        Search = 50,
        Formula = 51,
        SearchIncremental = 52,
        ChineseHalfWidth = 53,
        ChineseFullWidth = 54,
        NativeScript = 55,
        Text = 57,
        Chat = 58,
        NameOrPhoneNumber = 59,
        EmailNameOrAddress = 60,
        Maps = 62,
        NumericPassword = 63,
        NumericPin = 64,
        AlphanumericPin = 65,
        FormulaNumber = 67,
        ChatWithoutEmoji = 68
    }
}
