﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            cipherText=cipherText.ToLower();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            int founder = 0;
            char[] key = new char[26];
            bool found = false;
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (plainText[i]==alphabet[j])
                    {
                        key[j] = cipherText[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (alphabet[i]==key[j])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (key[i]!=0)
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            if (key[j]==0)
                            {
                                founder = j;
                                break;
                            }
                        }
                        key[founder] = alphabet[i];
                    }
                    else
                        key[i] = alphabet[i];
                }
                found = false;
            }
            string returnedKey = new string(key);
            return returnedKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            string plainText = "";
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < cipherText.Length; i++)
                plainText += alphabet[key.IndexOf(cipherText[i])];
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToUpper();
            string cipherText = "";
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < plainText.Length; i++)
                cipherText += key[alphabet.IndexOf(plainText[i])];
            return cipherText;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            char[] plainText = new char[cipher.Length];
            double[] frequencyNumbers = { 12.51, 9.25, 8.04, 7.60, 7.26, 7.09, 6.54, 6.12, 5.49, 4.14, 3.99, 3.06, 2.71, 2.53, 2.30, 2.00, 1.96, 1.92, 1.73, 1.54, 0.99, 0.67, 0.19, 0.16, 0.11, 0.09 };
            char[] freqLetters = new char[] { 'E', 'T', 'A', 'O', 'I', 'N', 'S', 'R', 'H', 'L', 'D', 'C', 'U', 'M', 'F', 'P', 'G', 'W', 'Y', 'B', 'V', 'K', 'X', 'J', 'Q', 'Z' };
            double[] sortedFrequency = new double[26];
            string alphabet = "abcdefghijklmnopqrstuvwxyz".ToUpper();
            double counter = 0;
            List<Tuple<char, double>> freqNumberAndLetter = new List<Tuple<char, double>>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < cipher.Length; j++)
                    if (alphabet[i] == cipher[j])
                        counter++;
                sortedFrequency[i] = (counter / cipher.Length) * 100;
                counter = 0;
            }
            for (int i = 0; i < sortedFrequency.Length; i++)
                freqNumberAndLetter.Add(new Tuple<char, double>(alphabet[i], sortedFrequency[i]));
            var sortedFreqNumberAndLetter = freqNumberAndLetter.OrderByDescending(t => t.Item2).ToList();
            for (int i = 0; i < sortedFreqNumberAndLetter.Count; i++)
                for (int j = 0; j < cipher.Length; j++)
                    if (sortedFreqNumberAndLetter[i].Item1 == cipher[j])
                        plainText[j] = freqLetters[i];
            string plainTextStr = new string(plainText);
            plainTextStr = plainTextStr.ToUpper();
            return plainTextStr;
        }
    }
}
