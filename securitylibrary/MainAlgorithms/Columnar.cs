using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            char f = cipherText[0];
            char s = cipherText[1];
            int num1=0;
            int diff = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                if (f==plainText[i])
                {
                    num1 = i;
                }
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                if (s == plainText[i] && Math.Abs(num1 - i) > 2)
                {
                    diff = i;
                }
            }
            int coulmns = Math.Abs(diff - num1);
            double tmp = (double)plainText.Length / (double)coulmns;
            int rows = (int)Math.Ceiling(tmp);
            char[,] pt = new char[rows, coulmns];
            char[,] pt2 = new char[rows, coulmns];
            int current_row = 0;
            int current_coulmn = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                pt[current_row, current_coulmn] = plainText[i];
                pt2[current_row, current_coulmn] = cipherText[i];
                if (current_coulmn == coulmns - 1)
                {
                    current_coulmn = 0;
                    current_row++;

                }

                else
                {
                    current_coulmn++;
                }
            }
            current_coulmn = 0;
            current_row = 0;
            List<int> key = new List<int>(coulmns);
            bool flag = true;
            for (int i = 0; i < coulmns; i++)
            {
                for (int j = 0; j < coulmns; j++)
                {


                    for (int k = 0; k < rows; k++)
                    {
                        if (pt[k, i] != pt2[k, j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        key.Insert(i, j);
                    }
                    
                }
            }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            string plain = "";
            int coulmns = key.Count;
            double size = cipherText.Length;
            double arr_size = size / coulmns;
            arr_size = Math.Ceiling(arr_size);
            size = (int)arr_size;
            char[,] pt = new char[(int)size, coulmns];

            int current_row = 0;
            int current_coulmn = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                pt[current_row, current_coulmn] = cipherText[i];
                if (current_row == (int)size - 1)
                {
                    current_row = 0;
                    current_coulmn++;

                }

                else
                {
                    current_row++;
                }
            }
            for (int i = 0; i < (int)size; i++)
            {
                for (int j = 0; j < coulmns; j++)
                {
                    if (pt[i, j] == '\0')
                        pt[i, j] = 'x';
                }
            }
            for (int i = 0; i < (int)size; i++)
            {
                
                for (int j = 0; j < (int)coulmns; j++)
                {
                    int col = key.ElementAt(j);
                    plain += pt[i, col-1];
                }
            }
            return plain;
        }

        public string Encrypt(string plainText, List<int> key)
        { string cipher = "";
            int coulmns = key.Count;
            double size = plainText.Length;
            double arr_size = size / coulmns;
            arr_size = Math.Ceiling(arr_size);
            size = (int)arr_size;
            char[,] pt = new char[(int)size, coulmns];
            
            int current_row = 0;
            int current_coulmn = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                pt[current_row, current_coulmn] = plainText[i];
                if (current_coulmn == coulmns-1)
                {
                    current_coulmn = 0;
                    current_row++;

                }

                else
                {
                    current_coulmn++;
                }
            }
            for (int i = 0; i < (int)size; i++)
            {
                for (int j = 0; j <coulmns; j++)
                {
                    if (pt[i, j] == '\0')
                        pt[i, j] = 'x';
                }
            }
            for (int i = 0; i < coulmns; i++)
            {
                int col = key.IndexOf(i + 1);
                for (int j = 0; j < (int)size; j++)
                {
                    cipher += pt[j, col];
                }
            }
            return cipher;
        }
    }
}
