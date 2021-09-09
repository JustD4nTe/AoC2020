using System.Collections.Generic;

namespace AoC2020.Day20
{
    public class Element
    {
        private readonly int _n = 10;
        public char[][] Image;
        public Edge Edge { get; set; }
        public ulong Id { get; set; }

        public Element(char[][] image, ulong id)
        {
            Image = image;

            Id = id;
            CalcEdge();
        }

        public Element(string[] image, ulong id)
        {
            Image = new char[_n][];

            for (var i = 0; i < _n; i++)
                Image[i] = image[i].ToCharArray();

            Id = id;
            CalcEdge();
        }

        private void CalcEdge()
        {
            var up = 0;
            var down = 0;
            var left = 0;
            var right = 0;

            for (var i = 0; i < _n; i++)
            {
                for (var j = 0; j < _n; j++)
                {
                    if (i == 0 && Image[i][j] == '#')
                        up++;

                    if (i == _n - 1 && Image[i][j] == '#')
                        down++;

                    if (j == 0 && Image[i][j] == '#')
                        left++;

                    if (j == _n - 1 && Image[i][j] == '#')
                        right++;
                }
            }

            Edge = new Edge(up, right, down, left);
        }

        public List<Element> GetAllVariantions()
        {
            var rotate90 = MatrixHelper.Rotate(Image, _n);
            var rotate180 = MatrixHelper.Rotate(rotate90, _n);
            var rotate270 = MatrixHelper.Rotate(rotate180, _n);

            var refl1 = MatrixHelper.Flip(Image, _n);
            var refl2 = MatrixHelper.Flip(rotate90, _n);
            var refl3 = MatrixHelper.Flip(rotate180, _n);
            var refl4 = MatrixHelper.Flip(rotate270, _n);

            return new List<Element>(){
                new(rotate90, Id),
                new(rotate180, Id),
                new(rotate270, Id ),
                new(refl1, Id),
                new(refl2, Id),
                new(refl3, Id),
                new(refl4, Id)
            };
        }

        public string[] GetStringImage()
        {
            var res = new string[_n - 2];

            for (var i = 0; i < _n - 2; i++)
                res[i] = new string(Image[i + 1][1..^1]);

            return res;
        }
    }
}