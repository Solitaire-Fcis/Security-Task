using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            int current_row = 0;
            int current_coulmn = 0;
            plainText = plainText.ToLower();
            int[,] arr = new int[5, 5];
            int[] flag = new int[25];
            for (int i = 0; i < 24; i++)
            {
                flag[i] = 0;
            }
            string C_T = "";
            string alphabet = "abcdefghiklmnopqrstuvwxyz";
            for (int i = 0; i < key.Length; i++)
            {

                if (key[i] == 'j')
                {
                    if (current_coulmn == 4)
                    {
                        current_coulmn = 0;
                        current_row++;
                    }
                    else
                    {
                        current_coulmn++;
                    }
                    flag[8] = 1;
                    arr[current_row, current_coulmn] = 8;
                }
                for (int j = 0; j <= 24; j++)
                {
                    if (key[i] == alphabet[j] && flag[j] == 0)
                    {
                        arr[current_row, current_coulmn] = j;
                        flag[j] = 1;
                        if (current_coulmn == 4)
                        {
                            current_coulmn = 0;
                            current_row++;
                        }
                        else
                        {
                            current_coulmn++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i <= 24; i++)
            {
                if (flag[i] == 0)
                {
                    arr[current_row, current_coulmn] = i;
                    flag[i] = 1;
                    if (current_coulmn == 4)
                    {
                        current_coulmn = 0;
                        current_row++;
                    }
                    else
                    {
                        current_coulmn++;
                    }
                }
            }
            string text = "";
            text += plainText[0];
            for (int i = 0; i < plainText.Length - 1; i++)
            {
                if (plainText[i + 1] != plainText[i])
                {
                    text += plainText[i + 1];
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        text += 'x';
                        text += plainText[i + 1];
                    }
                    else
                    {
                        text += plainText[i + 1];
                    }
                }
            }
            if (text.Length % 2 != 0)
            {
                text += 'x';
            }
            int C_T_index = 0;
            int frow = 0;
            int fcol = 0;
            int srow = 0;
            int scol = 0;
            for (int i = 0; i < text.Length - 1; i += 2)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (text[i] == alphabet[j])
                    {
                        C_T_index = j;
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 0; l < 5; l++)
                            {
                                if (alphabet[arr[k, l]] == alphabet[j])
                                {
                                    frow = k;
                                    fcol = l;
                                }
                            }
                        }
                    }
                    if (text[i + 1] == alphabet[j])
                    {
                        C_T_index = j;
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 0; l < 5; l++)
                            {
                                if (alphabet[arr[k, l]] == alphabet[j])
                                {
                                    srow = k;
                                    scol = l;
                                }
                            }
                        }
                    }

                }
                if (srow == frow)
                {
                    if (fcol == 4)
                    {
                        C_T += alphabet[arr[frow, 0]];
                    }
                    else
                    {
                        C_T += alphabet[arr[frow, fcol + 1]];
                    }
                    if (scol == 4)
                    {
                        C_T += alphabet[arr[srow, 0]];
                    }
                    else
                    {
                        C_T += alphabet[arr[srow, scol + 1]];
                    }
                }
                else if (scol == fcol)
                {
                    if (frow == 4)
                    {
                        C_T += alphabet[arr[0, fcol]];
                    }
                    else
                    {
                        C_T += alphabet[arr[frow + 1, fcol]];
                    }
                    if (srow == 4)
                    {
                        C_T += alphabet[arr[0, scol]];
                    }
                    else
                    {
                        C_T += alphabet[arr[srow + 1, scol]];
                    }
                }
                else
                {
                    C_T += alphabet[arr[frow, scol]];
                    C_T += alphabet[arr[srow, fcol]];
                }
            }
            char x = text[312], y = text[311], z = text[313];
            C_T = C_T.ToUpper();
            return C_T;
        }
    }
}
