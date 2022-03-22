using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        private List<int> ConvertMatrixToList(int[,] matrix)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    list.Add(matrix[i, j]);
            return list;
        }

        private int[,] ConvertkeyToMatrix(List<int> key)
        {
            int[,] keyMatrix;
            int count;
            if (key.Count % 2 == 0)
            {
                keyMatrix = new int[2, 2];
                count = 0;
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        keyMatrix[x, y] = key[count];
                        count++;
                    }
                }
                return keyMatrix;
            }
            keyMatrix = new int[3, 3];
            count = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    keyMatrix[x, y] = key[count];
                    count++;
                }
            }
            return keyMatrix;
        }

        private int Determinent(int[,] keyMatrix)
        {
            int det = 0;
            if (keyMatrix.GetLength(0) == 2)
                det += (keyMatrix[0, 0] * keyMatrix[1, 1]) - (keyMatrix[0, 1] * keyMatrix[1, 0]);
            else
                for (int i = 0; i < 3; i++)
                    det += (keyMatrix[0, i] * (keyMatrix[1, (i + 1) % 3] * keyMatrix[2, (i + 2) % 3] - keyMatrix[1, (i + 2) % 3] * keyMatrix[2, (i + 1) % 3]));
            return det;
        }

        private int FindB(int Det)
        {
            int result = 0;
            for (int i = 2; i < 26; i++)
            {
                if (((i * Det) % 26) == 1)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        private int[,] MinorMatrix(int[,] matrix, int row, int col)
        {
            int[,] minor = new int[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            int m = 0, n = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == row)
                    continue;
                n = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == col)
                        continue;
                    minor[m, n] = matrix[i, j];
                    n++;
                }
                m++;
            }
            return minor;
        }

        private int Mod(int n, int m)
        {
            if (n < 0)
                return ((n % m) + m) % m;
            return n % m;
        }

        private int[,] flip2x2Matrix(int[,] matrix)
        {
            int[,] flip = new int[2,2];
            flip[0, 0] = matrix[1, 1];
            flip[1, 1] = matrix[0, 0];
            flip[0, 1] = 0 - matrix[0, 1];
            flip[1, 0] = 0 - matrix[1, 0];
            return flip;
        }
    

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
            List
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plainText = new List<int>();
            int[,] keyMatrix = this.ConvertkeyToMatrix(key);
            int det = this.Mod(this.Determinent(keyMatrix), 26);
            int[,] keyMatrixInverse = new int[keyMatrix.GetLength(0), keyMatrix.GetLength(1)];
            List<int> keyInverse = new List<int>();

            if (keyMatrix.GetLength(0) == 3)
            {
                int b = this.FindB(det);


                for (int i = 0; i < keyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < keyMatrix.GetLength(1); j++)
                    {
                        int[,] minorMatrix = this.MinorMatrix(keyMatrix, j, i);
                        int subdet = this.Mod(this.Determinent(minorMatrix), 26);
                        keyMatrixInverse[i, j] = Convert.ToInt32(b * Math.Pow(-1, i + j) * subdet);
                        keyMatrixInverse[i, j] = this.Mod(keyMatrixInverse[i, j], 26);
                    }
                }

                keyInverse = this.ConvertMatrixToList(keyMatrixInverse);

                for (int k = 0; k < cipherText.Count; k += 3)
                {
                    for (int i = 0; i < keyInverse.Count; i += 3)
                    {
                        plainText.Add(((keyInverse[i] * cipherText[k]) + (keyInverse[i + 1] * cipherText[k + 1]) + (keyInverse[i + 2] * cipherText[k + 2])) % 26);
                    }
                }
            }
            else
            {
                det = this.Determinent(keyMatrix);
                int[,] flipMatrix = this.flip2x2Matrix(keyMatrix);
                for (int i = 0; i < keyMatrixInverse.GetLength(0); i++)
                {
                    for (int j = 0; j < keyMatrixInverse.GetLength(1); j++)
                    {
                        keyMatrixInverse[i, j] = (1 / det) * flipMatrix[i, j];
                        keyMatrixInverse[i, j] = this.Mod(keyMatrixInverse[i, j], 26);
                    }
                }

                keyInverse = this.ConvertMatrixToList(keyMatrixInverse);

                for (int k = 0; k < cipherText.Count; k += 2)
                {
                    for (int i = 0; i < keyInverse.Count; i += 2)
                    {
                        plainText.Add(((keyInverse[i] * cipherText[k]) + (keyInverse[i + 1] * cipherText[k + 1])) % 26);
                    }
                }


            }
            return plainText;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipherText = new List<int>();
            
            if (key.Count % 2 == 0)
            {
                for (int k = 0; k < plainText.Count; k+=2)
                {
                    for (int i = 0; i < key.Count; i+=2)
                    {
                        cipherText.Add( ( ( key[i] * plainText[k] ) + ( key[i + 1] * plainText[k + 1] ) ) % 26 );
                    }
                }
            }
            else if (key.Count % 3 == 0)
            {
                for (int k = 0; k < plainText.Count; k += 3)
                {
                    for (int i = 0; i < key.Count; i += 3)
                    {
                        cipherText.Add(((key[i] * plainText[k]) + (key[i + 1] * plainText[k + 1]) + (key[i+2] * plainText[k+2]) ) % 26);
                    }
                }
            }

            return cipherText;

        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
