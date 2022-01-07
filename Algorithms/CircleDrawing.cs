using System.Collections.Generic;

namespace Seyren.Algorithms
{
    public class CircleDrawing
    {
        public static List<int[]> HaflIntegerCentered(int r)
        {
            List<int[]> result = new List<int[]>();
            int x = r;
            int y = 0;
            result.Add(new int[] { x, y });
            if (r > 0)
            {
                result.Add(new int[] { y, x });
            }
            int p = 1 - r;
            while (x > y)
            {
                y++;
                if (p <= 0)
                {
                    p = p + 2 * y + 1;
                }
                else
                {
                    x--;
                    p = p + 2 * y - 2 * x + 1;
                }

                if (x < y) break;
                result.Add(new int[] { x, y});
                // If the generated point is on the
                // line x = y then the perimeter points
                // have already been printed
                if (x != y)
                {
                    result.Add(new int[] { y, x});
                }
            }
            return result;
        }

        public static List<int[]> HaflIntegerCentered(int x_centre, int y_centre, int r)
        {
            List<int[]> result = new List<int[]>();
            int x = r;
            int y = 0;

            // Printing the initial point on the
            // axes after translation
            result.Add(new int[] { x + x_centre, y + y_centre });
            // When radius is zero only a single
            // point will be printed
            if (r > 0)
            {
                result.Add(new int[] { x + x_centre, -y + y_centre });
                result.Add(new int[] { y + x_centre, x + y_centre });
                result.Add(new int[] { -y + x_centre, x + y_centre });
            }

            int p = 1 - r;
            while (x > y)
            {
                y++;
                if (p <= 0)
                {
                    p = p + 2 * y + 1;
                }
                else
                {
                    x--;
                    p = p + 2 * y - 2 * x + 1;
                }

                if (x < y) break;
                result.Add(new int[] { x + x_centre, y + y_centre });
                result.Add(new int[] { -x + x_centre, y + y_centre });
                result.Add(new int[] { x + x_centre, -y + y_centre });
                result.Add(new int[] { -x + x_centre, -y + y_centre });
                // If the generated point is on the
                // line x = y then the perimeter points
                // have already been printed
                if (x != y)
                {
                    result.Add(new int[] { y + x_centre, x + y_centre });
                    result.Add(new int[] { -y + x_centre, x + y_centre });
                    result.Add(new int[] { y + x_centre, -x + y_centre });
                    result.Add(new int[] { -y + x_centre, -x + y_centre });
                }
            }

            return result;
        }
    }
}