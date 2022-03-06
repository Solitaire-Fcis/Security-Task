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
            int it_row_ind = 0, it_col_ind = 0;
            char[,] map = new char[5, 5];
            bool[] alphaTaken = new bool[25];
            string P_T = "", P_T_Clean = "", alphabet = "abcdefghiklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            List<Tuple<char, char>> C_T = new List<Tuple<char, char>>();
            for (int i = 0; i < cipherText.Length - 1; i += 2)
                C_T.Add(new Tuple<char, char>(cipherText[i], cipherText[i + 1]));
            for (int i = 0; i < key.Length; i++)
            {
                if (!alphaTaken[alphabet.IndexOf(key[i])])
                { 
                    if (key[i] == 'j')
                    {
                        alphaTaken[8] = true;
                        map[it_row_ind, it_col_ind] = 'j';
                        if (it_col_ind == 4)
                            it_row_ind++;
                        it_col_ind = (it_col_ind + 1) % 5;
                    }
                    else
                    {
                        map[it_row_ind, it_col_ind] = key[i];
                        alphaTaken[alphabet.IndexOf(key[i])] = true;
                        if (it_col_ind == 4)
                            it_row_ind++;
                        it_col_ind = (it_col_ind + 1) % 5;
                    }
                }
            }
            for (int i = 0; i <= 24; i++)
            {
                if (!alphaTaken[i])
                {
                    map[it_row_ind, it_col_ind] = alphabet[i];
                    int x = i;
                    char a = alphabet[i];
                    alphaTaken[alphabet.IndexOf(alphabet[i])] = true;
                    if (it_col_ind == 4)
                        it_row_ind++;
                    it_col_ind = (it_col_ind + 1) % 5;
                }
            }
            Tuple<int, int> first_ind = new Tuple<int, int>(-1,-1), second_ind = new Tuple<int, int>(-1, -1);
            foreach (var couple in C_T)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                        if (couple.Item1 == map[i, j])
                            first_ind = new Tuple<int, int>(i, j);
                        else if (couple.Item2 == map[i, j])
                            second_ind = new Tuple<int, int>(i, j);
                }
                if (first_ind.Item2 == second_ind.Item2)
                    P_T += map[((first_ind.Item1 - 1) + 5) % 5, first_ind.Item2].ToString() + map[((second_ind.Item1 - 1) + 5) % 5, second_ind.Item2].ToString();
                else if (first_ind.Item1 == second_ind.Item1)
                    P_T += map[first_ind.Item1, ((first_ind.Item2 - 1) + 5) % 5].ToString() + map[second_ind.Item1, ((second_ind.Item2 - 1) + 5) % 5].ToString();
                else
                    P_T += map[first_ind.Item1, second_ind.Item2].ToString() + map[second_ind.Item1, first_ind.Item2].ToString();
            }
            for (int i = 0; i < P_T.Length - 2; i += 2)
                if (P_T[i] == P_T[i + 2] && P_T[i + 1] == 'x' && (i + 1) % 2 == 1)
                    P_T_Clean += P_T[i].ToString();
                else
                    P_T_Clean += P_T[i].ToString() + P_T[i + 1].ToString();
            P_T_Clean += P_T[P_T.Length - 2].ToString() + P_T[P_T.Length - 1].ToString();
            if (P_T_Clean[P_T_Clean.Length - 1] == 'x')
                P_T_Clean = P_T_Clean.Remove(P_T_Clean.Length - 1, 1);
            return P_T_Clean;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            int current_row = 0;
            int current_coulmn = 0;
            plainText = plainText.ToLower();
            int[,] arr = new int[5, 5];
            int[] flag = new int[25];
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
                        current_coulmn++;
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
                        current_coulmn++;
                }
            }
            string text = "";
            text += plainText[0];
            for (int i = 0; i < plainText.Length - 1; i++)
            {
                if (plainText[i + 1] != plainText[i])
                    text += plainText[i + 1];
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
                text += 'x';
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
                        C_T += alphabet[arr[frow, 0]];
                    else
                        C_T += alphabet[arr[frow, fcol + 1]];
                    if (scol == 4)
                        C_T += alphabet[arr[srow, 0]];
                    else
                        C_T += alphabet[arr[srow, scol + 1]];
                }
                else if (scol == fcol)
                {
                    if (frow == 4)
                        C_T += alphabet[arr[0, fcol]];
                    else
                        C_T += alphabet[arr[frow + 1, fcol]];
                    if (srow == 4)
                        C_T += alphabet[arr[0, scol]];
                    else
                        C_T += alphabet[arr[srow + 1, scol]];
                }
                else
                {
                    C_T += alphabet[arr[frow, scol]];
                    C_T += alphabet[arr[srow, fcol]];
                }
            }
            C_T = C_T.ToUpper();
            return C_T;
        }
    }
}
